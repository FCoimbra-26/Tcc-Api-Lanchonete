using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TCC.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigrationFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Enderecos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Logradouro = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Numero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Complemento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Bairro = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Cidade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Uf = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: false),
                    Cep = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enderecos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProdutosGlobais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeProduto = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PrecoBase = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Categoria = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ImagemUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutosGlobais", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pessoas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sobrenome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EnderecoId = table.Column<int>(type: "int", nullable: true),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pessoas_Enderecos_EnderecoId",
                        column: x => x.EnderecoId,
                        principalTable: "Enderecos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Unidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    EnderecoId = table.Column<int>(type: "int", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Unidades_Enderecos_EnderecoId",
                        column: x => x.EnderecoId,
                        principalTable: "Enderecos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EmailNormalizado = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SenhaHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    PessoaId = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Pessoas_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardapioItens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnidadeId = table.Column<int>(type: "int", nullable: false),
                    ProdutoId = table.Column<int>(type: "int", nullable: false),
                    PrecoPraticado = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardapioItens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardapioItens_ProdutosGlobais_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "ProdutosGlobais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardapioItens_Unidades_UnidadeId",
                        column: x => x.UnidadeId,
                        principalTable: "Unidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EstoqueItens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnidadeId = table.Column<int>(type: "int", nullable: false),
                    ProdutoId = table.Column<int>(type: "int", nullable: false),
                    QuantidadeDisponivel = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    EstoqueMinimo = table.Column<int>(type: "int", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstoqueItens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstoqueItens_ProdutosGlobais_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "ProdutosGlobais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EstoqueItens_Unidades_UnidadeId",
                        column: x => x.UnidadeId,
                        principalTable: "Unidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UnidadesCanais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnidadeId = table.Column<int>(type: "int", nullable: false),
                    Canal = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadesCanais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnidadesCanais_Unidades_UnidadeId",
                        column: x => x.UnidadeId,
                        principalTable: "Unidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Acao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Recurso = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntidadeId = table.Column<int>(type: "int", nullable: true),
                    UsuarioId = table.Column<int>(type: "int", nullable: true),
                    UnidadeId = table.Column<int>(type: "int", nullable: true),
                    Sucesso = table.Column<bool>(type: "bit", nullable: false),
                    Detalhes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Unidades_UnidadeId",
                        column: x => x.UnidadeId,
                        principalTable: "Unidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroPedido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UnidadeId = table.Column<int>(type: "int", nullable: false),
                    CanalPedido = table.Column<int>(type: "int", nullable: false),
                    StatusPedido = table.Column<int>(type: "int", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false, defaultValue: 0m),
                    ClienteId = table.Column<int>(type: "int", nullable: true),
                    Observacao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pedidos_Unidades_UnidadeId",
                        column: x => x.UnidadeId,
                        principalTable: "Unidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pedidos_Usuarios_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuariosRoles_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstoqueMovimentacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstoqueItemId = table.Column<int>(type: "int", nullable: false),
                    TipoMovimentacao = table.Column<int>(type: "int", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    PedidoId = table.Column<int>(type: "int", nullable: true),
                    UsuarioResponsavelId = table.Column<int>(type: "int", nullable: true),
                    Observacao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstoqueMovimentacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstoqueMovimentacoes_EstoqueItens_EstoqueItemId",
                        column: x => x.EstoqueItemId,
                        principalTable: "EstoqueItens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstoqueMovimentacoes_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EstoqueMovimentacoes_Usuarios_UsuarioResponsavelId",
                        column: x => x.UsuarioResponsavelId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Pagamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoId = table.Column<int>(type: "int", nullable: false),
                    StatusAtual = table.Column<int>(type: "int", nullable: false),
                    ValorTotalCobrado = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    MetodoPagamentoFinal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OrigemConfirmacaoFinal = table.Column<int>(type: "int", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagamentos_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PedidoItens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoId = table.Column<int>(type: "int", nullable: false),
                    CardapioItemId = table.Column<int>(type: "int", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    ObservacaoItem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoItens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidoItens_CardapioItens_CardapioItemId",
                        column: x => x.CardapioItemId,
                        principalTable: "CardapioItens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PedidoItens_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PedidoStatusHistoricos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoId = table.Column<int>(type: "int", nullable: false),
                    StatusAnterior = table.Column<int>(type: "int", nullable: true),
                    StatusNovo = table.Column<int>(type: "int", nullable: false),
                    UsuarioResponsavelId = table.Column<int>(type: "int", nullable: true),
                    Observacao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoStatusHistoricos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidoStatusHistoricos_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoStatusHistoricos_Usuarios_UsuarioResponsavelId",
                        column: x => x.UsuarioResponsavelId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PagamentoTentativas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PagamentoId = table.Column<int>(type: "int", nullable: false),
                    SequenciaTentativa = table.Column<int>(type: "int", nullable: false),
                    OrigemConfirmacao = table.Column<int>(type: "int", nullable: false),
                    StatusTentativa = table.Column<int>(type: "int", nullable: false),
                    ValorCobrado = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    DataSolicitacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataRetorno = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PayloadRetorno = table.Column<string>(type: "text", nullable: true),
                    UsuarioConfirmacaoId = table.Column<int>(type: "int", nullable: true),
                    MetodoPagamento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Observacao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagamentoTentativas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PagamentoTentativas_Pagamentos_PagamentoId",
                        column: x => x.PagamentoId,
                        principalTable: "Pagamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PagamentoTentativas_Usuarios_UsuarioConfirmacaoId",
                        column: x => x.UsuarioConfirmacaoId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_DataCriacao",
                table: "AuditLogs",
                column: "DataCriacao");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_Recurso_Acao",
                table: "AuditLogs",
                columns: new[] { "Recurso", "Acao" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UnidadeId",
                table: "AuditLogs",
                column: "UnidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UsuarioId",
                table: "AuditLogs",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_CardapioItens_ProdutoId",
                table: "CardapioItens",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_CardapioItens_UnidadeId_ProdutoId",
                table: "CardapioItens",
                columns: new[] { "UnidadeId", "ProdutoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstoqueItens_ProdutoId",
                table: "EstoqueItens",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_EstoqueItens_UnidadeId_ProdutoId",
                table: "EstoqueItens",
                columns: new[] { "UnidadeId", "ProdutoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstoqueMovimentacoes_EstoqueItemId",
                table: "EstoqueMovimentacoes",
                column: "EstoqueItemId");

            migrationBuilder.CreateIndex(
                name: "IX_EstoqueMovimentacoes_PedidoId",
                table: "EstoqueMovimentacoes",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_EstoqueMovimentacoes_UsuarioResponsavelId",
                table: "EstoqueMovimentacoes",
                column: "UsuarioResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_PedidoId",
                table: "Pagamentos",
                column: "PedidoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PagamentoTentativas_PagamentoId",
                table: "PagamentoTentativas",
                column: "PagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_PagamentoTentativas_UsuarioConfirmacaoId",
                table: "PagamentoTentativas",
                column: "UsuarioConfirmacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItens_CardapioItemId",
                table: "PedidoItens",
                column: "CardapioItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItens_PedidoId",
                table: "PedidoItens",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_ClienteId",
                table: "Pedidos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_NumeroPedido",
                table: "Pedidos",
                column: "NumeroPedido",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_UnidadeId",
                table: "Pedidos",
                column: "UnidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoStatusHistoricos_PedidoId",
                table: "PedidoStatusHistoricos",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoStatusHistoricos_UsuarioResponsavelId",
                table: "PedidoStatusHistoricos",
                column: "UsuarioResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_Pessoas_Cpf",
                table: "Pessoas",
                column: "Cpf");

            migrationBuilder.CreateIndex(
                name: "IX_Pessoas_EnderecoId",
                table: "Pessoas",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_Unidades_Codigo",
                table: "Unidades",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unidades_EnderecoId",
                table: "Unidades",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_UnidadesCanais_UnidadeId_Canal",
                table: "UnidadesCanais",
                columns: new[] { "UnidadeId", "Canal" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_EmailNormalizado",
                table: "Usuarios",
                column: "EmailNormalizado",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_PessoaId",
                table: "Usuarios",
                column: "PessoaId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosRoles_UsuarioId_Role",
                table: "UsuariosRoles",
                columns: new[] { "UsuarioId", "Role" });

            var dataBase = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var senhaPadraoHash = BCrypt.Net.BCrypt.HashPassword("senha123");

            migrationBuilder.InsertData(
                table: "Enderecos",
                columns: new[] { "Id", "Logradouro", "Numero", "Complemento", "Bairro", "Cidade", "Uf", "Cep", "DataCriacao", "DataAtualizacao" },
                values: new object[,]
                {
                    { 1, "Av. Paulista", "1000", "Conjunto 101", "Bela Vista", "Sao Paulo", "SP", "01310100", dataBase, dataBase },
                    { 2, "Rua Vergueiro", "500", null, "Liberdade", "Sao Paulo", "SP", "01504000", dataBase, dataBase }
                });

            migrationBuilder.InsertData(
                table: "Unidades",
                columns: new[] { "Id", "Codigo", "Nome", "Ativo", "EnderecoId", "DataCriacao", "DataAtualizacao" },
                values: new object[] { 1, "UN001", "Unidade Centro", true, 1, dataBase, dataBase });

            migrationBuilder.InsertData(
                table: "UnidadesCanais",
                columns: new[] { "Id", "UnidadeId", "Canal", "Ativo", "DataCriacao", "DataAtualizacao" },
                values: new object[,]
                {
                    { 1, 1, 0, true, dataBase, dataBase },
                    { 2, 1, 2, true, dataBase, dataBase },
                    { 3, 1, 3, true, dataBase, dataBase },
                    { 4, 1, 4, true, dataBase, dataBase },
                    { 5, 1, 5, true, dataBase, dataBase }
                });

            migrationBuilder.InsertData(
                table: "ProdutosGlobais",
                columns: new[] { "Id", "NomeProduto", "PrecoBase", "Ativo", "Descricao", "Categoria", "ImagemUrl", "DataCriacao", "DataAtualizacao" },
                values: new object[,]
                {
                    { 1, "X-Burger", 28.90m, true, "Hamburguer com queijo", "Lanche", null, dataBase, dataBase },
                    { 2, "Batata Frita", 14.50m, true, "Porcao media", "Acompanhamento", null, dataBase, dataBase },
                    { 3, "Refrigerante Lata", 7.00m, true, "350ml", "Bebida", null, dataBase, dataBase }
                });

            migrationBuilder.InsertData(
                table: "CardapioItens",
                columns: new[] { "Id", "UnidadeId", "ProdutoId", "PrecoPraticado", "DataCriacao", "DataAtualizacao" },
                values: new object[,]
                {
                    { 1, 1, 1, 28.90m, dataBase, dataBase },
                    { 2, 1, 2, 14.50m, dataBase, dataBase },
                    { 3, 1, 3, 7.00m, dataBase, dataBase }
                });

            migrationBuilder.InsertData(
                table: "EstoqueItens",
                columns: new[] { "Id", "UnidadeId", "ProdutoId", "QuantidadeDisponivel", "Ativo", "EstoqueMinimo", "DataCriacao", "DataAtualizacao" },
                values: new object[,]
                {
                    { 1, 1, 1, 100, true, 10, dataBase, dataBase },
                    { 2, 1, 2, 120, true, 15, dataBase, dataBase },
                    { 3, 1, 3, 150, true, 20, dataBase, dataBase }
                });

            migrationBuilder.InsertData(
                table: "Pessoas",
                columns: new[] { "Id", "Nome", "Sobrenome", "Cpf", "Telefone", "EnderecoId", "DataNascimento", "DataCriacao", "DataAtualizacao" },
                values: new object[,]
                {
                    { 1, "Joao", "Silva", "12345678901", "11999990001", 2, new DateTime(1995, 5, 10, 0, 0, 0, DateTimeKind.Utc), dataBase, dataBase },
                    { 2, "Ana", "Atendente", "12345678902", "11999990002", 2, new DateTime(1992, 3, 15, 0, 0, 0, DateTimeKind.Utc), dataBase, dataBase },
                    { 3, "Carlos", "Gerente", "12345678903", "11999990003", 2, new DateTime(1990, 8, 20, 0, 0, 0, DateTimeKind.Utc), dataBase, dataBase }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Email", "EmailNormalizado", "SenhaHash", "Ativo", "PessoaId", "DataCriacao", "DataAtualizacao" },
                values: new object[,]
                {
                    { 1, "joao.silva@email.com", "joao.silva@email.com", senhaPadraoHash, true, 1, dataBase, dataBase },
                    { 2, "atendente@test.com", "atendente@test.com", senhaPadraoHash, true, 2, dataBase, dataBase },
                    { 3, "gerente@test.com", "gerente@test.com", senhaPadraoHash, true, 3, dataBase, dataBase }
                });

            migrationBuilder.InsertData(
                table: "UsuariosRoles",
                columns: new[] { "Id", "UsuarioId", "Role", "Ativo", "DataCriacao", "DataAtualizacao" },
                values: new object[,]
                {
                    { 1, 1, 0, true, dataBase, dataBase },
                    { 2, 2, 1, true, dataBase, dataBase },
                    { 3, 3, 3, true, dataBase, dataBase }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "EstoqueMovimentacoes");

            migrationBuilder.DropTable(
                name: "PagamentoTentativas");

            migrationBuilder.DropTable(
                name: "PedidoItens");

            migrationBuilder.DropTable(
                name: "PedidoStatusHistoricos");

            migrationBuilder.DropTable(
                name: "UnidadesCanais");

            migrationBuilder.DropTable(
                name: "UsuariosRoles");

            migrationBuilder.DropTable(
                name: "EstoqueItens");

            migrationBuilder.DropTable(
                name: "Pagamentos");

            migrationBuilder.DropTable(
                name: "CardapioItens");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "ProdutosGlobais");

            migrationBuilder.DropTable(
                name: "Unidades");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Pessoas");

            migrationBuilder.DropTable(
                name: "Enderecos");
        }
    }
}
