using Microsoft.EntityFrameworkCore;
using TCC.Domain.Entities;
using TCC.Domain.Enums;
using TCC.Domain.Interfaces;
using TCC.Infra.Data.Context;

namespace TCC.Infra.Data.Repositories
{
    public class UsuarioRoleRepository : IUsuarioRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UsuarioRoleHistorico>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.UsuariosRoles
                .Where(ur => ur.UsuarioId == usuarioId)
                .OrderByDescending(ur => ur.DataCriacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<UsuarioRole>> GetActiveRolesByUsuarioIdAsync(int usuarioId)
        {
            return await _context.UsuariosRoles
                .Where(ur => ur.UsuarioId == usuarioId && ur.Ativo)
                .Select(ur => ur.Role)
                .Distinct()
                .ToListAsync();
        }

        public async Task<UsuarioRoleHistorico> AssignRoleAsync(int usuarioId, UsuarioRole role)
        {
            var existingRole = await _context.UsuariosRoles
                .FirstOrDefaultAsync(ur => ur.UsuarioId == usuarioId && ur.Role == role);

            if (existingRole != null)
            {
                existingRole.Ativo = true;
                existingRole.DataAtualizacao = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return existingRole;
            }

            var usuarioRole = new UsuarioRoleHistorico
            {
                UsuarioId = usuarioId,
                Role = role,
                Ativo = true,
                DataCriacao = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
            };

            await _context.UsuariosRoles.AddAsync(usuarioRole);
            await _context.SaveChangesAsync();

            return usuarioRole;
        }

        public async Task<bool> RemoveRoleAsync(int usuarioId, UsuarioRole role)
        {
            var usuarioRole = await _context.UsuariosRoles
                .FirstOrDefaultAsync(ur => ur.UsuarioId == usuarioId && ur.Role == role);

            if (usuarioRole == null)
                return false;

            _context.UsuariosRoles.Remove(usuarioRole);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> HasRoleAsync(int usuarioId, UsuarioRole role)
        {
            return await _context.UsuariosRoles
                .AnyAsync(ur => ur.UsuarioId == usuarioId && ur.Role == role && ur.Ativo);
        }

        public async Task<bool> ActivateRoleAsync(int usuarioId, UsuarioRole role)
        {
            var usuarioRole = await _context.UsuariosRoles
                .FirstOrDefaultAsync(ur => ur.UsuarioId == usuarioId && ur.Role == role);

            if (usuarioRole == null)
                return false;

            usuarioRole.Ativo = true;
            usuarioRole.DataAtualizacao = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeactivateRoleAsync(int usuarioId, UsuarioRole role)
        {
            var usuarioRole = await _context.UsuariosRoles
                .FirstOrDefaultAsync(ur => ur.UsuarioId == usuarioId && ur.Role == role);

            if (usuarioRole == null)
                return false;

            usuarioRole.Ativo = false;
            usuarioRole.DataAtualizacao = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
