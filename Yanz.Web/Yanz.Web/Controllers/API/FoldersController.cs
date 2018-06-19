using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Yanz.DAL.Entities;
using Yanz.DAL.Interfaces;
using Yanz.Web.Models;

namespace Yanz.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FoldersController : ControllerBase
    {
        UserManager<AppUser> userManager;
        IUnitOfWork db; 

        public FoldersController(IUnitOfWork unit, UserManager<AppUser> manager)
        {
            userManager = manager;
            db = unit;
        }

        [HttpGet]
        [Authorize]
        [Route("root")]
        public async Task<IActionResult> Root()
        {
            var userId = userManager.GetUserId(User);
            return Ok(await GetItemsAsync(userId, null));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(string Id)
        {
            var userId = userManager.GetUserId(User);
            Folder folder = await db.Folders.GetAsync(userId, Id);
            if (folder == null)
                return NotFound(Id);

            FolderView view = new FolderView()
            {
                Id = folder.Id,
                Title = folder.Title,
                ParentId = folder.ParentId,
                Breadcrumbs = await GetBreadcrumbsAsync(userId, folder.Id),
                Items = await GetItemsAsync(userId, folder.Id)
            };
            return Ok(view);
        }

        // POST: api/Folders
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]FolderView folder)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = userManager.GetUserId(User);
            string parentId = (await db.Folders.GetAsync(userId, folder.ParentId))?.Id;
            if (folder.ParentId != "root" && parentId == null)
                return BadRequest($"Not found folder {folder.ParentId}");

            var nFolder = new Folder()
            {
                Title = folder.Title,
                AppUserId = userId,
                ParentId = parentId,
                Id = Guid.NewGuid().ToString()
            };

            await db.Folders.AddAsync(nFolder);
            await db.SaveAsync();

            FolderView view = new FolderView()
            {
                Id = nFolder.Id,
                Title = nFolder.Title,
                ParentId = nFolder.ParentId,
                Breadcrumbs = await GetBreadcrumbsAsync(userId, nFolder.Id),
                Items = await GetItemsAsync(userId, nFolder.Id)
            };

            return CreatedAtAction("Get", new { id = view.Id }, view);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string Id)
        {
            string userId = userManager.GetUserId(User);
            var folder = await db.Folders.GetAsync(userId, Id);
            if (folder == null)
                return NotFound(Id);

            var childFolders = new List<Folder>();

            await GetChildFolder(userId, folder.Id, childFolders);

            //Каскадное удаление QuestionSets. Подробнее в методе OnModelCreating
            db.Folders.RemoveRange(childFolders);
            db.Folders.Remove(folder);
            await db.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{id}/move")]
        [Authorize]
        public async Task<IActionResult> Move(string Id, string moveFolderId)
        {
            if (Id == moveFolderId)
                return BadRequest("Сan not be moved to the same folder");
            var userId = userManager.GetUserId(User);

            var folder = await db.Folders.GetAsync(userId, Id);
            if (folder == null)
                return NotFound(Id);
            var parentFolder = await db.Folders.GetAsync(userId, moveFolderId);

            if (moveFolderId != "root" && parentFolder == null)
                return NotFound(moveFolderId);

            if (await IsSubFolderAsync(userId, Id, parentFolder?.Id))
                return BadRequest($"Folder {parentFolder.Id} is subfolder {Id}");

            folder.ParentId = parentFolder?.Id;
            db.Folders.Update(folder);
            await db.SaveAsync();
            FolderView view = new FolderView()
            {
                Id = folder.Id,
                Title = folder.Title,
                ParentId = folder.ParentId,
                Breadcrumbs = await GetBreadcrumbsAsync(userId, folder.Id),
                Items = await GetItemsAsync(userId, folder.Id)
            };
            return Ok(view);
        }

        [HttpPatch("{id}/rename")]
        [Authorize]
        public async Task<IActionResult> Rename(string Id, string titleNew)
        {
            if (string.IsNullOrEmpty(titleNew))
                return BadRequest("Title length is less than 1");
            var userId = userManager.GetUserId(User);
            var folder = await db.Folders.GetAsync(userId, Id);
            if (folder == null)
                return NotFound(Id);

            folder.Title = titleNew;
            db.Folders.Update(folder);
            await db.SaveAsync();

            FolderView view = new FolderView()
            {
                Id = folder.Id,
                Title = folder.Title,
                ParentId = folder.ParentId,
                Breadcrumbs = await GetBreadcrumbsAsync(userId, folder.Id),
                Items = await GetItemsAsync(userId, folder.Id)
            };
            return Ok(view);
        }

        /// <summary>
        /// Получаем все элементы(набор вопросов, подпапки) в папке
        /// </summary>
        /// <param name="folderId">ID папки</param>
        /// <returns></returns>
        private async Task<List<Item>> GetItemsAsync(string userId, string folderId)
        {
            var listFolders = (await db.Folders.GetAllByUserAsync(userId))
                .Where(f => f.ParentId == folderId)
                .OrderBy(f => f.Title)
                .ToList();

            var items = new List<Item>();
            foreach (var folder in listFolders)
                items.Add(new Item(folder));

            var listQstSets = db.Folders.GetQuestionSets(folderId).OrderBy(q => q.Title).ToList();

            foreach (var qstSet in listQstSets)
                items.Add(new Item(qstSet));

            return items;
        }

        /// <summary>
        /// Получаем путь вложенности папки до корня(null)
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        private async Task<List<Breadcrumb>> GetBreadcrumbsAsync(string userId, string folderId)
        {
            if (folderId == null)
                return new List<Breadcrumb>();

            var breadcrumbs = new List<Breadcrumb>();
            Folder folder = await db.Folders.GetAsync(userId, folderId);
            Folder parentFolder = await db.Folders.GetAsync(userId, folder.ParentId);
            while (parentFolder != null)
            {
                breadcrumbs.Add(new Breadcrumb(parentFolder));
                parentFolder = await db.Folders.GetAsync(userId, parentFolder.ParentId);
            }
            breadcrumbs.Reverse();
            return breadcrumbs;
        }

        /// <summary>
        /// Получаем подпапки папки с folderId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="folderId"></param>
        /// <param name="childFolders">Список который содержит подпапки</param>
        private async Task GetChildFolder(string userId, string folderId, List<Folder> childFolders)
        {
            var folders = await db.Folders.GetChildFoldersAsync(userId, folderId);
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
            //Если перемещение в ту же папку
            if (folderId == moveFolderId)
                return true;
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