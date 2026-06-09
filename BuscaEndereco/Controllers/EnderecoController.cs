using Microsoft.AspNetCore.Mvc;
using BuscaEndereco.Models;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace BuscaEndereco.Controllers
{
    public class EnderecoController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new EnderecoViewModel());
        }

        
        [HttpGet]
public async Task<IActionResult> BuscarCep(string cep)
{
    if (string.IsNullOrEmpty(cep)) return Json(new { erro = true });

    // Deixa apenas os números limpos
    cep = cep.Replace("-", "").Replace(" ", "").Trim();

    // Se não tiver 8 números após a limpa, nem faz a busca
    if (cep.Length != 8) return Json(new { erro = true });

    try
    {
        using (var client = new HttpClient())
        {
            // CRÍTICO: Adiciona o cabeçalho para o ViaCEP aceitar a requisição do Codespaces
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AspNetCoreMvcApp/1.0");

            var response = await client.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
            
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                
                // Se o ViaCEP respondeu mas retornou o marcador de cep inexistente
                if (jsonString.Contains("\"erro\"") || jsonString.Contains("true"))
                {
                    return Json(new { erro = true });
                }

                return Content(jsonString, "application/json");
            }
        }
    }
    catch
    {
        // Se houver alguma falha de rede do servidor, cai aqui
    }

    return Json(new { erro = true });
        }

        [HttpPost]
        public IActionResult Salvar(EnderecoViewModel model)
        {
            if (ModelState.IsValid)
            {
                TempData["EnderecoSalvoJson"] = JsonSerializer.Serialize(model);
                return RedirectToAction("Confirmacao");
            }
            return View("Index", model);
        }

        [HttpGet]
        public IActionResult Confirmacao()
        {
            if (TempData["EnderecoSalvoJson"] is string jsonText)
            {
                var model = JsonSerializer.Deserialize<EnderecoViewModel>(jsonText);
                return View(model);
            }
            return RedirectToAction("Index");
        }
    }
}
