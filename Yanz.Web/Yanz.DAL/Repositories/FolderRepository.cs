using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yanz.DAL.EF;
using Yanz.DAL.Entities;
using Yanz.DAL.Interfaces;

namespace Yanz.DAL.Repositories
{
    public class FolderRepository : Repository<Folder>, IFolderRepository
    {
        public FolderRepository(AppDbContext dbContext)
            :base(dbContext)
        {
        }

        public IEnumerable<Folder> GetAllByUser(string userId)
        {
            return db.Folders.Where(f => f.AppUserId == userId).ToList();
        }

        public async Task<IEnumerable<Folder>> GetAllByUserAsync(string userId)
        {
            return await db.Folders.Where(f => f.AppUserId == userId).ToListAsync();
        }

        public async Task<Folder> GetAsync(string userId, string folderId)
        {
            return await db.Folders.FirstOrDefaultAsync(f => f.Id == folderId && f.AppUserId == userId);
        }

        public async Task<IEnumerable<Folder>> GetChildFoldersAsync(string userId, string folderId)
        {
            return await db.Folders.Where(f => f.ParentId == folderId && f.AppUserId == userId).ToListAsync();
        }

        public IEnumerable<QuestionSet> GetQuestionSets(string folderId)
        {
            return db.QuestionSets.Include(q => q.Questions).Where(q => q.FolderId == folderId);
        }

        public async Task<IEnumerable<Folder>> GetRootFolderAsync(string userId)
        {
            return await db.Folders
               .Where(f => f.ParentId == null && f.AppUserId == userId)
               .ToListAsync();
        }
    }
}
