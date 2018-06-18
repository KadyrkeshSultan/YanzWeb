using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yanz.DAL.EF;
using Yanz.DAL.Entities;
using Yanz.DAL.Interfaces;

namespace Yanz.DAL.Repositories
{
    public class ModerMsgRepository : Repository<ModerMsg>, IModerMsgRepository
    {
        public ModerMsgRepository(AppDbContext dbContext)
            :base(dbContext)
        {

        }

        public async Task<ModerMsg> GetByQuestionSetId(string questionSetId)
        {
            return await db.ModerMsgs.FirstOrDefaultAsync(m => m.QuestionSetId == questionSetId);
        }

        public async Task<IEnumerable<ModerMsg>> GetByUserId(string userId)
        {
            return await db.ModerMsgs.Where(m => m.AppUserId == userId).OrderByDescending(m => m.Create).ToListAsync();
        }

        public async Task<IEnumerable<ModerMsg>> GetOnModerationAsync()
        {
            return await db.ModerMsgs.Where(m => m.Status == Status.Moderation).OrderByDescending(o => o.Create).ToListAsync();
        }
    }
}
