using System;
using System.Threading.Tasks;
using Yanz.DAL.Repositories;

namespace Yanz.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IChoiceRepository Choices { get; }
        IQuestionRepository Questions { get; }
        IFolderRepository Folders { get; }
        IQuestionSetRepository QuestionSets { get; }
        ISetRepository Sets { get; }
        IModerMsgRepository ModerMsgs { get; }

        void Save();
        Task SaveAsync();
    }
}
