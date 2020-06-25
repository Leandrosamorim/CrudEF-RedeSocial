using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Models.Models;

namespace Infrastructure.Context.WebApplication14.Data
{
    public class WebApplication14Context : DbContext
    {
        public WebApplication14Context (DbContextOptions<WebApplication14Context> options)
            : base(options)
        {
        }

        public WebApplication14Context()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = @"Data Source=tcp:tp3appdbserver.database.windows.net,1433;Initial Catalog=WebApplication14_db;User Id=Leandro@tp3appdbserver;Password=Santos13";

            optionsBuilder.UseSqlServer(connectionString);

            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Professor> Professor { get; set; }
        public DbSet<Post> Post { get; set; }
    }
}
