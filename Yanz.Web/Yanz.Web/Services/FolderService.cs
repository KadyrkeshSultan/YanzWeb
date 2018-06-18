using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yanz.Web.Models;
using Microsoft.EntityFrameworkCore;
using Yanz.DAL.Interfaces;
using Yanz.DAL.Entities;

namespace Yanz.Web.Services
{
    public class FolderService
    {
        IUnitOfWork db;

        public FolderService(IUnitOfWork dbContext)
        {
            db = dbContext;
        }

        public async Task<IEnumerable<Folder>> GetRootFolderAsync(string userId)
        {
            return await db.Folders.GetRootFolderAsync(userId);
        }

        /// <summary>
        /// Получить папки пользователя с userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Folder>> GetFoldersAsync(string userId)
        {
            return await db.Folders.GetAllByUserAsync(userId);
        }

        /// <summary>
        /// Получить папку с folderId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="folderId"></param>
        /// <returns></returns>
        public async Task<Folder> GetFolderAsync(string userId, string folderId)
        {
            return await db.Folders.GetAsync(userId, folderId);
        }

        /// <summary>
        /// Получить набор вопросов с questions, по folderId
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        public IEnumerable<QuestionSet> GetQuestionSets(string folderId)
        {
            return db.QuestionSets.GetWithQstByFolder(folderId);
        }

        /// <summary>
        /// Добавляем в таблицу Folders - folder
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public async Task AddAsync(Folder folder)
        {
            await db.Folders.AddAsync(folder);
        }

        public async Task<string> MoveAsync(string userId, string folderId, string moveFolderId)
        {
            var folder = await GetFolderAsync(userId, folderId);
            if (folder == null)
                return $"Not found folderId = {folderId}";
            var parentFolder = await GetFolderAsync(userId, moveFolderId);
            if (moveFolderId != "root" && parentFolder == null)
                return $"Not found moveFolderId = {moveFolderId}";
            if (await IsSubFolderAsync(userId, folderId, moveFolderId))
                return $"Folder {moveFolderId} is subfolder {folderId}";

            folder.ParentId = parentFolder?.Id;
            db.Folders.Update(folder);
            return null;
        }

        public async Task<string> RenameAsync(string userId, string folderId, string newTitle)
        {
            var folder = await GetFolderAsync(userId, folderId);
            if (folder == null)
                return $"Not found folderId = {folderId}";
            if (newTitle.Length < 1)
                return $"Title length is less than 1";
            folder.Title = newTitle;
            db.Folders.Update(folder);
            return null;
        }

        /// <summary>
        /// Удаляем папку с folderId, со всеми подпапками и наборами вопросов.
        /// False если папка не нашлась
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="folderId"></param>
        /// <returns>False если папка не нашлась</returns>
        public async Task<bool> RemoveAsync(string userId, string folderId)
        {
            var folder = await GetFolderAsync(userId, folderId);
            if (folder == null)
                return false;
            var childFolders = new List<Folder>();

            await GetChildFolder(userId, folderId, childFolders);

            //Каскадное удаление QuestionSets. Подробнее в методе OnModelCreating
            db.Folders.RemoveRange(childFolders);
            db.Folders.Remove(folder);
            return true;
        }

        public async Task SaveAsync()
        {
            await db.SaveAsync();
        }

        /// <summary>
        /// Получаем подпапки папки с folderId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="folderId"></param>
        /// <param name="childFolders">Список который содержит подпапки</param>
        private async Task GetChildFolder(string userId, string folderId, List<Folder> childFolders)
        {
            var folders = (await GetFoldersAsync(userId)).Where(f => f.ParentId == folderId).ToList();
            childFolders.AddRange(folders);
            foreach (var folder in folders)
                await GetChildFolder(userId, folder.Id, childFolders);
        }

        /// <summary>
        /// Является ли папка в которую надо переместить - подпапкой
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="folderId">Id папки которую нужно переместить</param>
        /// <param name="moveFolderId">Id папки в которую нужно переместить</param>
        /// <returns></returns>
        private async Task<bool> IsSubFolderAsync(string userId, string folderId, string moveFolderId)
        {
            //Если папка в которую нужно переместить, является корнем
            if (moveFolderId == null)
                return false;
            var subFolders = new List<Folder>();
            await GetChildFolder(userId, folderId, subFolders);
            foreach (var folder in subFolders)
                if (folder.Id == moveFolderId)
                    return true;
            return false;
        }
    }
}
