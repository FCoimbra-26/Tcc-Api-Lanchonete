using Microsoft.EntityFrameworkCore;
using TCC.Domain.Entities;
using TCC.Domain.Enums;
using TCC.Domain.Interfaces;
using TCC.Infra.Data.Context;

namespace TCC.Infra.Data.Repositories
{
    public class UnidadeRepository : IUnidadeRepository
    {
        private readonly ApplicationDbContext _context;

        public UnidadeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unidade?> GetByIdAsync(int id)
        {
            return await _context.Unidades
                .Include(u => u.Endereco)
                .Include(u => u.CanaisAtendimento)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Unidade?> GetByCodigoAsync(string codigo)
        {
            return await _context.Unidades
                .Include(u => u.Endereco)
                .Include(u => u.CanaisAtendimento)
                .FirstOrDefaultAsync(u => u.Codigo == codigo);
        }

        public async Task<IEnumerable<Unidade>> GetAllAsync(bool? apenasAtivas = null)
        {
            var query = _context.Unidades
                .Include(u => u.Endereco)
                .Include(u => u.CanaisAtendimento)
                .AsQueryable();

            if (apenasAtivas.HasValue)
            {
                query = query.Where(u => u.Ativo == apenasAtivas.Value);
            }

            return await query
                .OrderBy(u => u.Nome)
                .ToListAsync();
        }

        public async Task<Unidade> CreateAsync(Unidade unidade)
        {
            unidade.DataCriacao = DateTime.UtcNow;
            unidade.DataAtualizacao = DateTime.UtcNow;

            if (unidade.Endereco != null)
            {
                unidade.Endereco.DataCriacao = DateTime.UtcNow;
                unidade.Endereco.DataAtualizacao = DateTime.UtcNow;
            }

            foreach (var canal in unidade.CanaisAtendimento)
            {
                canal.DataCriacao = DateTime.UtcNow;
                canal.DataAtualizacao = DateTime.UtcNow;
            }

            await _context.Unidades.AddAsync(unidade);
            await _context.SaveChangesAsync();

            return unidade;
        }

        public async Task<Unidade> UpdateAsync(Unidade unidade)
        {
            unidade.DataAtualizacao = DateTime.UtcNow;

            _context.Unidades.Update(unidade);
            await _context.SaveChangesAsync();

            return unidade;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var unidade = await _context.Unidades.FindAsync(id);
            if (unidade == null)
                return false;

            _context.Unidades.Remove(unidade);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsByCodigoAsync(string codigo)
        {
            return await _context.Unidades
                .AnyAsync(u => u.Codigo == codigo);
        }

        public async Task<bool> ActivateAsync(int id)
        {
            var unidade = await _context.Unidades.FindAsync(id);
            if (unidade == null)
                return false;

            unidade.Ativo = true;
            unidade.DataAtualizacao = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var unidade = await _context.Unidades.FindAsync(id);
            if (unidade == null)
                return false;

            unidade.Ativo = false;
            unidade.DataAtualizacao = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddCanalAsync(int unidadeId, CanalAtendimento canal)
        {
            var existingCanal = await _context.UnidadesCanais
                .FirstOrDefaultAsync(uc => uc.UnidadeId == unidadeId && uc.Canal == canal);

            if (existingCanal != null)
            {
                existingCanal.Ativo = true;
                existingCanal.DataAtualizacao = DateTime.UtcNow;
            }
            else
            {
                var unidadeCanal = new UnidadeCanal
                {
                    UnidadeId = unidadeId,
                    Canal = canal,
                    Ativo = true,
                    DataCriacao = DateTime.UtcNow,
                    DataAtualizacao = DateTime.UtcNow
                };

                await _context.UnidadesCanais.AddAsync(unidadeCanal);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveCanalAsync(int unidadeId, CanalAtendimento canal)
        {
            var unidadeCanal = await _context.UnidadesCanais
                .FirstOrDefaultAsync(uc => uc.UnidadeId == unidadeId && uc.Canal == canal);

            if (unidadeCanal == null)
                return false;

            _context.UnidadesCanais.Remove(unidadeCanal);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<CanalAtendimento>> GetCanaisAtivosAsync(int unidadeId)
        {
            return await _context.UnidadesCanais
                .Where(uc => uc.UnidadeId == unidadeId && uc.Ativo)
                .Select(uc => uc.Canal)
                .ToListAsync();
        }
    }
}
