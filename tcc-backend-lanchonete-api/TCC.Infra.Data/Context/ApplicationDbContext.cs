using Microsoft.EntityFrameworkCore;
using TCC.Domain.Entities;
using TCC.Infra.Data.EntitiesConfiguration;

namespace TCC.Infra.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<UsuarioRoleHistorico> UsuariosRoles { get; set; }
        public DbSet<Unidade> Unidades { get; set; }
        public DbSet<UnidadeCanal> UnidadesCanais { get; set; }
        public DbSet<ProdutoGlobal> ProdutosGlobais { get; set; }
        public DbSet<CardapioItem> CardapioItens { get; set; }
        public DbSet<EstoqueItem> EstoqueItens { get; set; }
        public DbSet<EstoqueMovimentacao> EstoqueMovimentacoes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoItem> PedidoItens { get; set; }
        public DbSet<PedidoStatusHistorico> PedidoStatusHistoricos { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<PagamentoTentativa> PagamentoTentativas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new PessoaConfiguration());
            modelBuilder.ApplyConfiguration(new EnderecoConfiguration());
            modelBuilder.ApplyConfiguration(new UsuarioRoleHistoricoConfiguration());
            modelBuilder.ApplyConfiguration(new UnidadeConfiguration());
            modelBuilder.ApplyConfiguration(new UnidadeCanalConfiguration());
            modelBuilder.ApplyConfiguration(new ProdutoGlobalConfiguration());
            modelBuilder.ApplyConfiguration(new CardapioItemConfiguration());
            modelBuilder.ApplyConfiguration(new EstoqueItemConfiguration());
            modelBuilder.ApplyConfiguration(new EstoqueMovimentacaoConfiguration());
            modelBuilder.ApplyConfiguration(new PedidoConfiguration());
            modelBuilder.ApplyConfiguration(new PedidoItemConfiguration());
            modelBuilder.ApplyConfiguration(new PedidoStatusHistoricoConfiguration());
            modelBuilder.ApplyConfiguration(new PagamentoConfiguration());
            modelBuilder.ApplyConfiguration(new PagamentoTentativaConfiguration());
        }
    }
}
