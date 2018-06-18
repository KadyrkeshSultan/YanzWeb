using System.Collections.Generic;
using System.Threading.Tasks;
using Yanz.DAL.Entities;

namespace Yanz.DAL.Interfaces
{
    public interface IFolderRepository : IRepository<Folder>
    {
        IEnumerable<Folder> GetAllByUser(string userId);
        Task<IEnumerable<Folder>> GetAllByUserAsync(string userId);
        Task<Folder> GetAsync(string userId, string folderId);
        IEnumerable<QuestionSet> GetQuestionSets(string folderId);
        Task<IEnumerable<Folder>> GetChildFoldersAsync(string userId, string folderId);

        Task<IEnumerable<Folder>> GetRootFolderAsync(string userId);
    }
}
