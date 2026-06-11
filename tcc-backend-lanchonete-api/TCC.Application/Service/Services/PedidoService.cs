using TCC.Application.Models.Requests.Pedido;
using TCC.Application.Models.Responses.Pedido;
using TCC.Application.Service.Interfaces;
using TCC.Domain.Entities;
using TCC.Domain.Enums;
using TCC.Domain.Interfaces;

namespace TCC.Application.Service.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IUnidadeRepository _unidadeRepository;
        private readonly ICardapioRepository _cardapioRepository;
        private readonly IEstoqueRepository _estoqueRepository;
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoService(
            IUnidadeRepository unidadeRepository,
            ICardapioRepository cardapioRepository,
            IEstoqueRepository estoqueRepository,
            IPedidoRepository pedidoRepository)
        {
            _unidadeRepository = unidadeRepository;
            _cardapioRepository = cardapioRepository;
            _estoqueRepository = estoqueRepository;
            _pedidoRepository = pedidoRepository;
        }

        public async Task<PedidoResponse> CreateAsync(CreatePedidoRequest request, int? usuarioLogadoId = null)
        {
            try
            {
                if (request.Itens == null || request.Itens.Count == 0)
                {
                    return new PedidoResponse
                    {
                        Success = false,
                        Error = "O pedido deve conter ao menos um item"
                    };
                }

                var unidade = await _unidadeRepository.GetByIdAsync(request.UnidadeId);
                if (unidade == null)
                {
                    return new PedidoResponse
                    {
                        Success = false,
                        Error = "Unidade nao encontrada"
                    };
                }

                if (!unidade.Ativo)
                {
                    return new PedidoResponse
                    {
                        Success = false,
                        Error = "A unidade informada esta inativa"
                    };
                }

                var canalHabilitado = unidade.CanaisAtendimento
                    .Any(c => c.Ativo && c.Canal == request.CanalPedido);

                if (!canalHabilitado)
                {
                    return new PedidoResponse
                    {
                        Success = false,
                        Error = "O canal informado nao esta habilitado para esta unidade"
                    };
                }

                var itensAgrupados = request.Itens
                    .GroupBy(i => i.ProdutoId)
                    .Select(g => new
                    {
                        ProdutoId = g.Key,
                        Quantidade = g.Sum(x => x.Quantidade),
                        ObservacaoItem = string.Join("; ", g.Select(x => x.ObservacaoItem).Where(x => !string.IsNullOrWhiteSpace(x)))
                    })
                    .ToList();

                var numeroPedido = GenerateNumeroPedido();

                var pedido = new Pedido
                {
                    NumeroPedido = numeroPedido,
                    UnidadeId = request.UnidadeId,
                    CanalPedido = request.CanalPedido,
                    ClienteId = request.ClienteId ?? usuarioLogadoId,
                    Observacao = request.Observacao,
                    StatusPedido = StatusPedido.AGUARDANDO_PAGAMENTO
                };

                var estoqueItensAtualizados = new List<EstoqueItem>();
                var movimentacoesEstoque = new List<EstoqueMovimentacao>();
                decimal valorTotal = 0;

                foreach (var item in itensAgrupados)
                {
                    var cardapioItem = await _cardapioRepository.GetByUnidadeAndProdutoAsync(request.UnidadeId, item.ProdutoId);
                    if (cardapioItem == null)
                    {
                        return new PedidoResponse
                        {
                            Success = false,
                            Error = $"Produto {item.ProdutoId} nao esta no cardapio da unidade"
                        };
                    }

                    if (!cardapioItem.Produto.Ativo)
                    {
                        return new PedidoResponse
                        {
                            Success = false,
                            Error = $"Produto {item.ProdutoId} esta inativo"
                        };
                    }

                    var estoque = await _estoqueRepository.GetByUnidadeAndProdutoAsync(request.UnidadeId, item.ProdutoId);
                    if (estoque == null || !estoque.Ativo)
                    {
                        return new PedidoResponse
                        {
                            Success = false,
                            Error = $"Produto {item.ProdutoId} sem controle de estoque ativo na unidade"
                        };
                    }

                    if (estoque.QuantidadeDisponivel < item.Quantidade)
                    {
                        return new PedidoResponse
                        {
                            Success = false,
                            Error = $"Estoque insuficiente para o produto {item.ProdutoId}"
                        };
                    }

                    var valorUnitario = cardapioItem.PrecoPraticado ?? cardapioItem.Produto.PrecoBase;
                    var subtotal = valorUnitario * item.Quantidade;

                    pedido.Itens.Add(new PedidoItem
                    {
                        CardapioItemId = cardapioItem.Id,
                        Quantidade = item.Quantidade,
                        ValorUnitario = valorUnitario,
                        Subtotal = subtotal,
                        ObservacaoItem = string.IsNullOrWhiteSpace(item.ObservacaoItem) ? null : item.ObservacaoItem
                    });

                    valorTotal += subtotal;

                    estoque.QuantidadeDisponivel -= item.Quantidade;
                    estoque.DataAtualizacao = DateTime.UtcNow;
                    estoqueItensAtualizados.Add(estoque);

                    movimentacoesEstoque.Add(new EstoqueMovimentacao
                    {
                        EstoqueItemId = estoque.Id,
                        TipoMovimentacao = TipoMovimentacaoEstoque.SAIDA,
                        Quantidade = item.Quantidade,
                        UsuarioResponsavelId = usuarioLogadoId,
                        Observacao = $"Saida por criacao do pedido {numeroPedido}"
                    });
                }

                pedido.ValorTotal = valorTotal;

                var pedidoCriado = await _pedidoRepository.CreateWithStockAsync(pedido, estoqueItensAtualizados, movimentacoesEstoque);

                return MapToResponse(pedidoCriado);
            }
            catch (Exception ex)
            {
                return new PedidoResponse
                {
                    Success = false,
                    Error = $"Erro ao criar pedido: {ex.Message}"
                };
            }
        }

        public async Task<PedidoResponse> UpdateStatusAsync(int pedidoId, UpdatePedidoStatusRequest request, int? usuarioLogadoId = null)
        {
            try
            {
                var pedido = await _pedidoRepository.GetByIdAsync(pedidoId);
                if (pedido == null)
                {
                    return new PedidoResponse
                    {
                        Success = false,
                        Error = "Pedido nao encontrado"
                    };
                }

                if (request.NovoStatus == StatusPedido.CANCELADO)
                {
                    return new PedidoResponse
                    {
                        Success = false,
                        Error = "Use o endpoint de cancelamento do pedido para alterar esse status"
                    };
                }

                if (request.NovoStatus == StatusPedido.PAGO || request.NovoStatus == StatusPedido.RECUSADO || request.NovoStatus == StatusPedido.AGUARDANDO_PAGAMENTO)
                {
                    return new PedidoResponse
                    {
                        Success = false,
                        Error = "Esse status nao pode ser atualizado por este endpoint"
                    };
                }

                if (pedido.StatusPedido == request.NovoStatus)
                {
                    return new PedidoResponse
                    {
                        Success = false,
                        Error = "O pedido ja esta no status informado"
                    };
                }

                if (!IsTransicaoPermitida(pedido.StatusPedido, request.NovoStatus))
                {
                    return new PedidoResponse
                    {
                        Success = false,
                        Error = $"Transicao invalida de {pedido.StatusPedido} para {request.NovoStatus}"
                    };
                }

                var statusAnterior = pedido.StatusPedido;
                pedido.StatusPedido = request.NovoStatus;

                var historico = new PedidoStatusHistorico
                {
                    PedidoId = pedido.Id,
                    StatusAnterior = statusAnterior,
                    StatusNovo = request.NovoStatus,
                    UsuarioResponsavelId = usuarioLogadoId,
                    Observacao = request.Observacao
                };

                var pedidoAtualizado = await _pedidoRepository.UpdateStatusAsync(pedido, historico);

                return MapToResponse(pedidoAtualizado);
            }
            catch (Exception ex)
            {
                return new PedidoResponse
                {
                    Success = false,
                    Error = $"Erro ao atualizar status do pedido: {ex.Message}"
                };
            }
        }

        public async Task<PedidoResponse> CancelAsync(int pedidoId, CancelPedidoRequest request, int? usuarioLogadoId = null, IEnumerable<string>? rolesUsuario = null)
        {
            try
            {
                var pedido = await _pedidoRepository.GetByIdAsync(pedidoId);
                if (pedido == null)
                {
                    return new PedidoResponse
                    {
                        Success = false,
                        Error = "Pedido nao encontrado"
                    };
                }

                if (pedido.StatusPedido == StatusPedido.CANCELADO)
                {
                    return new PedidoResponse
                    {
                        Success = false,
                        Error = "O pedido ja esta cancelado"
                    };
                }

                if (pedido.StatusPedido != StatusPedido.AGUARDANDO_PAGAMENTO && pedido.StatusPedido != StatusPedido.PAGO)
                {
                    return new PedidoResponse
                    {
                        Success = false,
                        Error = "O pedido so pode ser cancelado antes do inicio do preparo"
                    };
                }

                if (!UsuarioPodeCancelarPedido(pedido, usuarioLogadoId, rolesUsuario))
                {
                    return new PedidoResponse
                    {
                        Success = false,
                        Error = "Usuario nao autorizado a cancelar este pedido"
                    };
                }

                var estoqueItensAtualizados = new List<EstoqueItem>();
                var movimentacoesEstoque = new List<EstoqueMovimentacao>();

                foreach (var item in pedido.Itens)
                {
                    var produtoId = item.CardapioItem?.ProdutoId;
                    if (!produtoId.HasValue)
                    {
                        return new PedidoResponse
                        {
                            Success = false,
                            Error = "Nao foi possivel identificar os itens do pedido para estorno"
                        };
                    }

                    var estoque = await _estoqueRepository.GetByUnidadeAndProdutoAsync(pedido.UnidadeId, produtoId.Value);
                    if (estoque == null)
                    {
                        return new PedidoResponse
                        {
                            Success = false,
                            Error = $"Estoque nao encontrado para o produto {produtoId.Value} durante o cancelamento"
                        };
                    }

                    estoque.QuantidadeDisponivel += item.Quantidade;
                    estoque.DataAtualizacao = DateTime.UtcNow;
                    estoqueItensAtualizados.Add(estoque);

                    movimentacoesEstoque.Add(new EstoqueMovimentacao
                    {
                        EstoqueItemId = estoque.Id,
                        TipoMovimentacao = TipoMovimentacaoEstoque.ESTORNO,
                        Quantidade = item.Quantidade,
                        UsuarioResponsavelId = usuarioLogadoId,
                        Observacao = $"Estorno por cancelamento do pedido {pedido.NumeroPedido}"
                    });
                }

                var statusAnterior = pedido.StatusPedido;
                pedido.StatusPedido = StatusPedido.CANCELADO;

                var historico = new PedidoStatusHistorico
                {
                    PedidoId = pedido.Id,
                    StatusAnterior = statusAnterior,
                    StatusNovo = StatusPedido.CANCELADO,
                    UsuarioResponsavelId = usuarioLogadoId,
                    Observacao = request.Observacao
                };

                var pedidoCancelado = await _pedidoRepository.CancelWithStockReversalAsync(
                    pedido,
                    historico,
                    estoqueItensAtualizados,
                    movimentacoesEstoque);

                return MapToResponse(pedidoCancelado);
            }
            catch (Exception ex)
            {
                return new PedidoResponse
                {
                    Success = false,
                    Error = $"Erro ao cancelar pedido: {ex.Message}"
                };
            }
        }

        private static PedidoResponse MapToResponse(Pedido pedido)
        {
            return new PedidoResponse
            {
                Success = true,
                Id = pedido.Id,
                NumeroPedido = pedido.NumeroPedido,
                UnidadeId = pedido.UnidadeId,
                CanalPedido = pedido.CanalPedido,
                StatusPedido = pedido.StatusPedido,
                ValorTotal = pedido.ValorTotal,
                DataCriacao = pedido.DataCriacao,
                Itens = pedido.Itens.Select(i => new PedidoItemResponse
                {
                    ProdutoId = i.CardapioItem?.ProdutoId ?? 0,
                    NomeProduto = i.CardapioItem?.Produto?.NomeProduto ?? string.Empty,
                    Quantidade = i.Quantidade,
                    ValorUnitario = i.ValorUnitario,
                    Subtotal = i.Subtotal
                }).ToList()
            };
        }

        private static string GenerateNumeroPedido()
        {
            return $"PED-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
        }

        private static bool IsTransicaoPermitida(StatusPedido atual, StatusPedido novo)
        {
            return (atual, novo) switch
            {
                (StatusPedido.PAGO, StatusPedido.EM_PREPARO) => true,
                (StatusPedido.EM_PREPARO, StatusPedido.PRONTO) => true,
                (StatusPedido.PRONTO, StatusPedido.ENTREGUE) => true,
                _ => false
            };
        }

        private static bool UsuarioPodeCancelarPedido(Pedido pedido, int? usuarioLogadoId, IEnumerable<string>? rolesUsuario)
        {
            if (rolesUsuario == null)
            {
                return true;
            }

            var roles = rolesUsuario.ToHashSet(StringComparer.OrdinalIgnoreCase);

            if (roles.Contains("ATENDENTE"))
            {
                return true;
            }

            if (roles.Contains("CLIENTE"))
            {
                return usuarioLogadoId.HasValue && pedido.ClienteId == usuarioLogadoId.Value;
            }

            return false;
        }
    }
}
