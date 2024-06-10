using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Teste7Comm.API.DTO;
using Teste7Comm.API.Model;
using Teste7Comm.API.Service;

namespace Teste7Comm.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoaController : Controller
    {
        private readonly IMapper _mapper;
        private readonly PessoaService _service;

        public PessoaController(IMapper mapper, PessoaService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PessoaDTO>>> ListarPessoa(string? nome,string? cpf) 
        {
            List<PessoaModel> pessoas = (List<PessoaModel>)await _service.BuscaPessoa(nome, cpf);

            return Ok(_mapper.Map<IEnumerable<PessoaDTO>>(pessoas));
        }
        [HttpPost("CadastrarPessoa")]
        public async Task<ActionResult<PessoaDTO>> CadastraPessoa(PessoaDTO pessoa)
        {
            if(pessoa == null) { return BadRequest("Favor preencher todas as informações"); }
            var pessoaModel = _mapper.Map<PessoaModel>(pessoa);
            return Ok(_mapper.Map<PessoaDTO>(await _service.CadastrarPessoa(pessoaModel)));
        }
        [HttpGet("BuscaCep")]
        public async Task<ActionResult<EnderecoDTO>> BuscarCep(string cep)
        {
            return _mapper.Map<EnderecoDTO>(await _service.BuscaEnderecoCEP(cep));
        }
        [HttpDelete("Remover")]
        public ActionResult RemoverPessoa(int id)
        {
            if (_service.RemoverPessoa(id)) return Ok();
            return BadRequest();

        }
        [HttpPut("Update")]
        public async Task<ActionResult> UpdatePessoa(PessoaDTO pessoa)
        {
            if (await _service.UpdatePessoa(_mapper.Map<PessoaModel>(pessoa))) return Ok();
            return BadRequest();

        }
    }
}
