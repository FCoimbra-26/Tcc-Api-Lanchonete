using System;
using Microsoft.EntityFrameworkCore;
using TCC.Domain.Entities;
using TCC.Domain.Interfaces;
using TCC.Infra.Data.Context;

namespace TCC.Infra.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _context.Usuarios
                .Include(u => u.Pessoa)
                    .ThenInclude(p => p.Endereco)
                .Include(r => r.Roles)
                    .Where(r => r.Ativo)
                .FirstOrDefaultAsync(u => u.EmailNormalizado == email.ToUpper());
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Pessoa)
                    .ThenInclude(p => p.Endereco)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario> CreateAsync(Usuario usuario)
        {
            usuario.DataCriacao = DateTime.UtcNow;
            usuario.DataAtualizacao = DateTime.UtcNow;
            usuario.Pessoa.DataCriacao = DateTime.UtcNow;
            usuario.Pessoa.DataAtualizacao = DateTime.UtcNow;

            if (usuario.Pessoa.Endereco != null)
            {
                usuario.Pessoa.Endereco.DataCriacao = DateTime.UtcNow;
                usuario.Pessoa.Endereco.DataAtualizacao = DateTime.UtcNow;
            }

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.EmailNormalizado == email.ToUpper());
        }

        public async Task<bool> ExistsByCpfAsync(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            return await _context.Usuarios
                .Include(u => u.Pessoa)
                .AnyAsync(u => u.Pessoa.Cpf == cpf);
        }
    }
}
