using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Yanz.DAL.EF;
using Yanz.DAL.Entities;
using Yanz.DAL.Interfaces;

namespace Yanz.DAL.Repositories
{
    public class SetRepository : Repository<Set>, ISetRepository
    {
        public SetRepository(AppDbContext dbContext)
            :base(dbContext)
        {

        }
    }
}
