using TCC.Application.Models.Requests.Estoque;
using TCC.Application.Models.Responses.Estoque;

namespace TCC.Application.Service.Interfaces
{
    public interface IEstoqueService
    {
        Task<EstoqueEntradaResponse> RegistrarEntradaAsync(RegistrarEntradaEstoqueRequest request, int? usuarioLogadoId = null);
    }
}