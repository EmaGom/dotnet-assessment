using Microsoft.EntityFrameworkCore;
using TimeChimp.Backend.Assessment.Models;

namespace TimeChimp.Backend.Assessment.Repositories
{
    public class ContextEF : DbContext
    {
        public ContextEF(DbContextOptions<ContextEF> options)
           : base(options)
        {

        }
        private DbSet<Feed> Feed { get; set; }
    }
}
