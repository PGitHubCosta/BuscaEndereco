using Microsoft.AspNetCore.Mvc;
using BuscaEndereco.Models;
using System.Text.Json;

namespace BuscaEndereco.Controllers
{
    public class EnderecoController : Controller
    {
        // Exibe o formulário de endereço
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // Recebe os dados do formulário preenchidos via Model Binding (Server Side)
        [HttpPost]
        public IActionResult Salvar(EnderecoViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Serializa o objeto completo (dados digitados + dados incrementados pela API do ViaCEP)
                TempData["EnderecoSalvoJson"] = JsonSerializer.Serialize(model);
                
                // Redireciona para a view somente leitura de confirmação
                return RedirectToAction("Confirmacao");
            }

            // Se houver erros de validação do lado do servidor, retorna para o formulário
            return View("Index", model);
        }

        // Exibe a tela de confirmação (Somente Leitura)
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
