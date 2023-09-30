using DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Data
{
    public class eLeilaoDbContext : DbContext
    {
        public eLeilaoDbContext()
        {

        }
        public eLeilaoDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Obtenha a string de conexão do arquivo de configuração específico do DAL 
                var connectionString = "Data Source=DEV_PC;Initial Catalog=M3_eLeilao;Trusted_Connection=True;TrustServerCertificate=True;"; //cn martelado
                //var connectionString = DalConfiguration.ConnectionString; // cn dinâmico
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public DbSet<AuctionModel> Auctions { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<AssociationModel> Associations { get; set; }
        public DbSet<BidModel> Bids { get; set; }
    }
}
