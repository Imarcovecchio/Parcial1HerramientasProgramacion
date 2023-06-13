using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Parcial.Data
{
    public class AutorContext : IdentityDbContext
    {
        public AutorContext (DbContextOptions<AutorContext> options)
            : base(options)
        {
        }

        public DbSet<Autor> Autor { get; set; } = default!;

        public DbSet<Book> Book { get; set; } = default!;

        public DbSet<Categoria> Categoria {get;set;} = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<Autor>()
            .HasMany(p=> p.Books)
            .WithOne(p=> p.Autor)
            .HasForeignKey(p=> p.AutorId);

             modelBuilder.Entity<Book>()
            .HasMany(b => b.Categorias)
            .WithMany(c => c.Libros)
            .UsingEntity(j => j.ToTable("BookCategorias"));

            base.OnModelCreating(modelBuilder);
        }
    }
}
