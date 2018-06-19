using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yanz.DAL.EF;
using Yanz.DAL.Entities;
using Yanz.DAL.Interfaces;

namespace Yanz.DAL.Repositories
{
    public class QuestionSetRepository : Repository<QuestionSet>, IQuestionSetRepository
    {
        public QuestionSetRepository(AppDbContext dbContext)
            :base(dbContext)
        {

        }

        public async Task<IEnumerable<QuestionSet>> GetAllWithQuestions(string userId)
        {
            return await db.QuestionSets.Include(q => q.Questions).Where(q => q.AppUserId == userId).OrderBy(o => o.Title).ToListAsync();
        }

        public async Task<IEnumerable<QuestionSet>> GetAllWithQuestionsNoTrack(string userId)
        {
            return await db.QuestionSets
                .AsNoTracking()
                .Include(q => q.Questions)
                .Where(q => q.AppUserId == userId)
                .OrderBy(o => o.Title).ToListAsync();
        }

        public async Task<IEnumerable<QuestionSet>> GetWithQstByFolder(string folderId)
        {
            return await db.QuestionSets.Include(q => q.Questions).Where(q => q.FolderId == folderId).ToListAsync();
        }

        public async Task<QuestionSet> GetWithQuestionsAsync(string id)
        {
            return await db.QuestionSets.Include(q => q.Questions).FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<QuestionSet> GetWithQuestionsAsync(string userId, string id)
        {
            return await db.QuestionSets.Include(q => q.Questions).FirstOrDefaultAsync(q => q.Id == id && q.AppUserId == userId);
        }

        public async Task<QuestionSet> GetWithQuestionsNoTrackAsync(string userId, string id)
        {
            return await db.QuestionSets.AsNoTracking().Include(q => q.Questions).FirstOrDefaultAsync(q => q.Id == id && q.AppUserId == userId);
        }

        public async Task<QuestionSet> GetWithQuestionsNoTrackAsync(string id)
        {
            return await db.QuestionSets.AsNoTracking().Include(q => q.Questions).FirstOrDefaultAsync(q => q.Id == id);
        }
    }
}
