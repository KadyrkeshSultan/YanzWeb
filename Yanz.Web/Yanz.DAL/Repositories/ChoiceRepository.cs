using Yanz.DAL.EF;
using Yanz.DAL.Entities;
using Yanz.DAL.Interfaces;

namespace Yanz.DAL.Repositories
{
    public class ChoiceRepository : Repository<Choice>, IChoiceRepository
    {
        public ChoiceRepository(AppDbContext dbContext)
            :base(dbContext)
        {
        }
        
    }
}
