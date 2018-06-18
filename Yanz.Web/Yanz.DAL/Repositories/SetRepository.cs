using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Set>> GetAllWithQuestions(string userId)
        {
            return await db.Sets.Include(s => s.Questions).Where(s => s.AppUserId == userId).ToListAsync();
        }

        public async Task<Set> GetByUserAsync(string id, string userId)
        {
            return await db.Sets.FirstOrDefaultAsync(s => s.Id == id && s.AppUserId == userId);
        }
    }
}
