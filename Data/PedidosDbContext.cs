using Microsoft.EntityFrameworkCore;
using Api_Pedidos.Models;


namespace Api_Pedidos.Data
{
    public class PedidosDbContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;userid=root;password=0000;database=pedidos",
                ServerVersion.Parse("10.4.21-mariadb"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
