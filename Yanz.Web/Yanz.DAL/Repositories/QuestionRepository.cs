using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yanz.DAL.EF;
using Yanz.DAL.Entities;
using Yanz.DAL.Interfaces;

namespace Yanz.DAL.Repositories
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(AppDbContext dbContext)
            :base(dbContext)
        {

        }

        public async Task<Question> GetWithChoices(string id)
        {
            return await db.Questions.Include(q => q.Choices).FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<Question>> GetWithChoicesByQuestionSet(string questionSetId)
        {
            return await db.Questions.Include(q => q.Choices).Where(q => q.QuestionSetId == questionSetId).ToListAsync();
        }
    }
}
