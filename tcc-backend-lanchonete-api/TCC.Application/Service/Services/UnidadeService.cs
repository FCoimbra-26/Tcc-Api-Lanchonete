using TCC.Application.Models.Requests.Unidade;
using TCC.Application.Models.Responses.Unidade;
using TCC.Application.Service.Interfaces;
using TCC.Domain.Entities;
using TCC.Domain.Enums;
using TCC.Domain.Interfaces;

namespace TCC.Application.Service.Services
{
    public class UnidadeService : IUnidadeService
    {
        private readonly IUnidadeRepository _unidadeRepository;

        public UnidadeService(IUnidadeRepository unidadeRepository)
        {
            _unidadeRepository = unidadeRepository;
        }

        public async Task<UnidadeResponse> CreateAsync(CreateUnidadeRequest request)
        {
            try
            {
                // RN008: Código único
                if (await _unidadeRepository.ExistsByCodigoAsync(request.Codigo))
                {
                    return new UnidadeResponse
                    {
                        Success = false,
                        Error = "Já existe uma unidade com este código"
                    };
                }

                // RN009: Ao menos um canal de atendimento
                if (request.CanaisAtendimento == null || !request.CanaisAtendimento.Any())
                {
                    return new UnidadeResponse
                    {
                        Success = false,
                        Error = "É obrigatório informar ao menos um canal de atendimento"
                    };
                }

                var unidade = new Unidade
                {
                    Codigo = request.Codigo,
                    Nome = request.Nome,
                    Ativo = true
                };

                if (request.Endereco != null)
                {
                    unidade.Endereco = new Endereco
                    {
                        Logradouro = request.Endereco.Logradouro,
                        Numero = request.Endereco.Numero,
                        Complemento = request.Endereco.Complemento,
                        Bairro = request.Endereco.Bairro,
                        Cidade = request.Endereco.Cidade,
                        Uf = request.Endereco.Uf.ToUpper(),
                        Cep = request.Endereco.Cep
                    };
                }

                foreach (var canal in request.CanaisAtendimento.Distinct())
                {
                    unidade.CanaisAtendimento.Add(new UnidadeCanal
                    {
                        Canal = canal,
                        Ativo = true
                    });
                }

                var unidadeCriada = await _unidadeRepository.CreateAsync(unidade);

                return MapToResponse(unidadeCriada);
            }
            catch (Exception ex)
            {
                return new UnidadeResponse
                {
                    Success = false,
                    Error = $"Erro ao criar unidade: {ex.Message}"
                };
            }
        }

        public async Task<UnidadeResponse> UpdateAsync(int id, UpdateUnidadeRequest request)
        {
            try
            {
                var unidade = await _unidadeRepository.GetByIdAsync(id);
                if (unidade == null)
                {
                    return new UnidadeResponse
                    {
                        Success = false,
                        Error = "Unidade não encontrada"
                    };
                }

                unidade.Nome = request.Nome;

                if (request.Endereco != null)
                {
                    if (unidade.Endereco == null)
                    {
                        unidade.Endereco = new Endereco();
                    }

                    unidade.Endereco.Logradouro = request.Endereco.Logradouro;
                    unidade.Endereco.Numero = request.Endereco.Numero;
                    unidade.Endereco.Complemento = request.Endereco.Complemento;
                    unidade.Endereco.Bairro = request.Endereco.Bairro;
                    unidade.Endereco.Cidade = request.Endereco.Cidade;
                    unidade.Endereco.Uf = request.Endereco.Uf.ToUpper();
                    unidade.Endereco.Cep = request.Endereco.Cep;
                }

                var unidadeAtualizada = await _unidadeRepository.UpdateAsync(unidade);

                return MapToResponse(unidadeAtualizada);
            }
            catch (Exception ex)
            {
                return new UnidadeResponse
                {
                    Success = false,
                    Error = $"Erro ao atualizar unidade: {ex.Message}"
                };
            }
        }

        public async Task<UnidadeResponse> GetByIdAsync(int id)
        {
            try
            {
                var unidade = await _unidadeRepository.GetByIdAsync(id);
                if (unidade == null)
                {
                    return new UnidadeResponse
                    {
                        Success = false,
                        Error = "Unidade não encontrada"
                    };
                }

                return MapToResponse(unidade);
            }
            catch (Exception ex)
            {
                return new UnidadeResponse
                {
                    Success = false,
                    Error = $"Erro ao buscar unidade: {ex.Message}"
                };
            }
        }

        public async Task<UnidadeResponse> GetByCodigoAsync(string codigo)
        {
            try
            {
                var unidade = await _unidadeRepository.GetByCodigoAsync(codigo);
                if (unidade == null)
                {
                    return new UnidadeResponse
                    {
                        Success = false,
                        Error = "Unidade não encontrada"
                    };
                }

                return MapToResponse(unidade);
            }
            catch (Exception ex)
            {
                return new UnidadeResponse
                {
                    Success = false,
                    Error = $"Erro ao buscar unidade: {ex.Message}"
                };
            }
        }

        public async Task<UnidadeListResponse> GetAllAsync(bool? apenasAtivas = null)
        {
            try
            {
                var unidades = await _unidadeRepository.GetAllAsync(apenasAtivas);

                return new UnidadeListResponse
                {
                    Success = true,
                    Unidades = unidades.Select(u => new UnidadeItemResponse
                    {
                        Id = u.Id,
                        Codigo = u.Codigo,
                        Nome = u.Nome,
                        Ativo = u.Ativo,
                        Cidade = u.Endereco?.Cidade,
                        TotalCanais = u.CanaisAtendimento?.Count(c => c.Ativo) ?? 0
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                return new UnidadeListResponse
                {
                    Success = false,
                    Error = $"Erro ao listar unidades: {ex.Message}"
                };
            }
        }

        public async Task<UnidadeResponse> DeleteAsync(int id)
        {
            try
            {
                var deleted = await _unidadeRepository.DeleteAsync(id);
                if (!deleted)
                {
                    return new UnidadeResponse
                    {
                        Success = false,
                        Error = "Unidade não encontrada"
                    };
                }

                return new UnidadeResponse
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new UnidadeResponse
                {
                    Success = false,
                    Error = $"Erro ao deletar unidade: {ex.Message}"
                };
            }
        }

        public async Task<UnidadeResponse> ActivateAsync(int id)
        {
            try
            {
                var activated = await _unidadeRepository.ActivateAsync(id);
                if (!activated)
                {
                    return new UnidadeResponse
                    {
                        Success = false,
                        Error = "Unidade não encontrada"
                    };
                }

                return new UnidadeResponse
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new UnidadeResponse
                {
                    Success = false,
                    Error = $"Erro ao ativar unidade: {ex.Message}"
                };
            }
        }

        public async Task<UnidadeResponse> DeactivateAsync(int id)
        {
            try
            {
                // RN010: Verificação implementada no momento de criar pedido
                var deactivated = await _unidadeRepository.DeactivateAsync(id);
                if (!deactivated)
                {
                    return new UnidadeResponse
                    {
                        Success = false,
                        Error = "Unidade não encontrada"
                    };
                }

                return new UnidadeResponse
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new UnidadeResponse
                {
                    Success = false,
                    Error = $"Erro ao desativar unidade: {ex.Message}"
                };
            }
        }

        public async Task<UnidadeResponse> AddCanalAsync(int id, CanalAtendimento canal)
        {
            try
            {
                var unidade = await _unidadeRepository.GetByIdAsync(id);
                if (unidade == null)
                {
                    return new UnidadeResponse
                    {
                        Success = false,
                        Error = "Unidade não encontrada"
                    };
                }

                await _unidadeRepository.AddCanalAsync(id, canal);

                var unidadeAtualizada = await _unidadeRepository.GetByIdAsync(id);
                return MapToResponse(unidadeAtualizada!);
            }
            catch (Exception ex)
            {
                return new UnidadeResponse
                {
                    Success = false,
                    Error = $"Erro ao adicionar canal: {ex.Message}"
                };
            }
        }

        public async Task<UnidadeResponse> RemoveCanalAsync(int id, CanalAtendimento canal)
        {
            try
            {
                var unidade = await _unidadeRepository.GetByIdAsync(id);
                if (unidade == null)
                {
                    return new UnidadeResponse
                    {
                        Success = false,
                        Error = "Unidade não encontrada"
                    };
                }

                // RN009: Verificar se não é o último canal
                var canaisAtivos = await _unidadeRepository.GetCanaisAtivosAsync(id);
                if (canaisAtivos.Count() == 1 && canaisAtivos.Contains(canal))
                {
                    return new UnidadeResponse
                    {
                        Success = false,
                        Error = "Não é possível remover o último canal de atendimento da unidade"
                    };
                }

                await _unidadeRepository.RemoveCanalAsync(id, canal);

                var unidadeAtualizada = await _unidadeRepository.GetByIdAsync(id);
                return MapToResponse(unidadeAtualizada!);
            }
            catch (Exception ex)
            {
                return new UnidadeResponse
                {
                    Success = false,
                    Error = $"Erro ao remover canal: {ex.Message}"
                };
            }
        }

        private UnidadeResponse MapToResponse(Unidade unidade)
        {
            var response = new UnidadeResponse
            {
                Success = true,
                Id = unidade.Id,
                Codigo = unidade.Codigo,
                Nome = unidade.Nome,
                Ativo = unidade.Ativo,
                DataCriacao = unidade.DataCriacao,
                CanaisAtendimento = unidade.CanaisAtendimento?
                    .Where(c => c.Ativo)
                    .Select(c => c.Canal)
                    .ToList() ?? new List<CanalAtendimento>()
            };

            if (unidade.Endereco != null)
            {
                response.Endereco = new EnderecoResponse
                {
                    Id = unidade.Endereco.Id,
                    Logradouro = unidade.Endereco.Logradouro,
                    Numero = unidade.Endereco.Numero,
                    Complemento = unidade.Endereco.Complemento,
                    Bairro = unidade.Endereco.Bairro,
                    Cidade = unidade.Endereco.Cidade,
                    Uf = unidade.Endereco.Uf,
                    Cep = unidade.Endereco.Cep
                };
            }

            return response;
        }
    }
}
