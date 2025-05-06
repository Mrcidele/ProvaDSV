using Microsoft.EntityFrameworkCore;
using Prova.Models;

namespace Prova.Data;

public class AppDataContext : DbContext
{
    public AppDataContext(DbContextOptions options) : base(options) {}


        public DbSet<Usuario> Usuarios { get; set; } 
        public DbSet<Evento> Eventos { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Email)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Senha)
                .IsRequired();

            modelBuilder.Entity<Evento>()
                .Property(e => e.Nome)
                .IsRequired();

            modelBuilder.Entity<Evento>()
                .Property(e => e.Local)
                .IsRequired();

            modelBuilder.Entity<Evento>()
                .HasOne(e => e.Usuario)
                .WithMany(u => u.Eventos)
                .HasForeignKey(e => e.UsuarioId);
        }
    }
