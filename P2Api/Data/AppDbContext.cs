using Microsoft.EntityFrameworkCore;
using P2Api.Models;

namespace P2Api.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("DataSource = app.db; Cache = Shared");
        }
    }
}
