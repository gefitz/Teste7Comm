using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json;
using Teste7Comm.Web.Model;
using Teste7Comm.Web.Models;

namespace Teste7Comm.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _url = "https://localhost:7259/api/Pessoa";
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<ActionResult> Index(string? nome, string? cpf)
        {
            List<PessoaModel> model = new List<PessoaModel>();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(_url + $"?nome={nome}&cpf={cpf}");

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    model = JsonSerializer.Deserialize<List<PessoaModel>>(responseData);
                }

            }
            return View(model);
        }

        public IActionResult CadastrarPessoa()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CadastrarPessoa(PessoaModel pessoa)
        {
            using (HttpClient client = new HttpClient())
            {
                if (pessoa.endereco.complemento == null) pessoa.endereco.complemento = "";

                var pessoaJson = JsonSerializer.Serialize(pessoa);
                StringContent content = new StringContent(pessoaJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_url + "/CadastrarPessoa", content);

                if (response.IsSuccessStatusCode)
                {
                     return RedirectToAction("Index");
                }
                string responseContent = await response.Content.ReadAsStringAsync();

            }
            return BadRequest();
        }
        public async Task<IActionResult> UpdatePessoa(string cpf)
        {
            var model = await BuscaPessoaPorCPF(cpf);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(PessoaModel pessoa)
        {
            using (HttpClient client = new HttpClient())
            {
                if (pessoa.endereco.complemento == null) pessoa.endereco.complemento = "";

                var pessoaJson = JsonSerializer.Serialize(pessoa);
                StringContent content = new StringContent(pessoaJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(_url+"/Update" , content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                string responseContent = await response.Content.ReadAsStringAsync();

            }
            return BadRequest();
        }
        public async Task<IActionResult> DeletarPessoa(string cpf) 
        {
            var model = await BuscaPessoaPorCPF(cpf);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DeletePessoa(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.DeleteAsync(_url +"/Remover?id="+id);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

            }
            return BadRequest();
        }
        public async Task<ActionResult<EnderecoModel>> BuscaEnderecoCEP(string cep)
        {
            EnderecoModel model = new EnderecoModel();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(_url + "/BuscaCep?cep=" + cep);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    model = JsonSerializer.Deserialize<EnderecoModel>(responseData);
                }

            }
            return Ok(model);

        }
        private async Task<PessoaModel> BuscaPessoaPorCPF(string cpf)
        {
            PessoaModel model = new PessoaModel();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(_url + $"?cpf={cpf}");

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    var json = JsonSerializer.Deserialize<List<PessoaModel>>(responseData);
                    model = json.FirstOrDefault();
                }

            }
            return model;
        }


    }
}
