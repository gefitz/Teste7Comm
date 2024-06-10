using AutoMapper;
using Teste7Comm.API.Model;

namespace Teste7Comm.API.DTO.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<EnderecoModel, EnderecoDTO>().ReverseMap();
            CreateMap<PessoaModel, PessoaDTO>().ReverseMap();
            CreateMap<PessoaInsertModel, PessoaDTO>().ReverseMap();
        }
    }
}
