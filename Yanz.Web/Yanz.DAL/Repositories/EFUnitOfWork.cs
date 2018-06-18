using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Yanz.DAL.EF;
using Yanz.DAL.Interfaces;

namespace Yanz.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private AppDbContext db;
        private IFolderRepository folderRepository;
        private IQuestionRepository questionRepository;
        private IChoiceRepository choiceRepository;
        private IQuestionSetRepository questionSetRepository;
        private ISetRepository setRepository;
        private IModerMsgRepository moderMsgRepository;

        public EFUnitOfWork(DbContextOptions<AppDbContext> options)
        {
            db = new AppDbContext(options);
        }

        public IChoiceRepository Choices
        {
            get
            {
                if (choiceRepository == null)
                    choiceRepository = new ChoiceRepository(db);
                return choiceRepository;
            }
        }

        public IQuestionRepository Questions
        {
            get
            {
                if (questionRepository == null)
                    questionRepository = new QuestionRepository(db);
                return questionRepository;
            }
        }

        public IFolderRepository Folders
        {
            get
            {
                if (folderRepository == null)
                    folderRepository = new FolderRepository(db);
                return folderRepository;
            }
        }

        public IQuestionSetRepository QuestionSets
        {
            get
            {
                if (questionSetRepository == null)
                    questionSetRepository = new QuestionSetRepository(db);
                return questionSetRepository;
            }
        }

        public ISetRepository Sets
        {
            get
            {
                if (setRepository == null)
                    setRepository = new SetRepository(db);
                return setRepository;
            }
        }

        public IModerMsgRepository ModerMsgs
        {
            get
            {
                if (moderMsgRepository == null)
                    moderMsgRepository = new ModerMsgRepository(db);
                return moderMsgRepository;
            }
        }

        public void Save() => db.SaveChanges();

        public async Task SaveAsync() => await db.SaveChangesAsync();

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    db.Dispose();// TODO: освободить управляемое состояние (управляемые объекты).
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~EFUnitOfWork() {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
