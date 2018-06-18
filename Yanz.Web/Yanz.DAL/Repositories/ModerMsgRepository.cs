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
    }
}
