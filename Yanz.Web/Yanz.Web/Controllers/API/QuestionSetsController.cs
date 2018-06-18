using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yanz.DAL.Entities;
using Yanz.DAL.Interfaces;
using Yanz.Web.Models;

namespace Yanz.Web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/QuestionSets")]
    public class QuestionSetsController : ControllerBase
    {
        IUnitOfWork db;
        UserManager<AppUser> userManager;

        public QuestionSetsController(IUnitOfWork _db, UserManager<AppUser> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }

        // GET: api/Images
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var user = await userManager.GetUserAsync(User);
            var sets = db.QuestionSets.GetAllWithQuestionsNoTrack(user.Id);
            List<QuestionSetView> views = new List<QuestionSetView>();
            foreach (var set in sets)
                views.Add(new QuestionSetView(set, user.UserName, await GetBreadcrumbsAsync(set.Id)));
            return Ok(views);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(string Id)
        {
            var user = await userManager.GetUserAsync(User);
            QuestionSet set = await db.QuestionSets.GetWithQuestionsNoTrackAsync(Id);
            if (set == null)
                return NotFound(Id);
            //set.Questions = set.Questions.OrderBy(q => q.Order).ToList();
            QuestionSetView view = new QuestionSetView(set, user.UserName, await GetBreadcrumbsAsync(Id));
            return Ok(view);
        }

        // POST: api/QuestionSets
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]QuestionSetView set)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.GetUserAsync(User);
            var folderId = (await db.Folders.GetAsync(set.FolderId))?.Id;
            if (set.FolderId != "root" && folderId == null)
                return BadRequest($"Not found folder {set.FolderId}");

            set.FolderId = folderId;

            QuestionSet questionSet = new QuestionSet()
            {
                Id = Guid.NewGuid().ToString(),
                AppUserId = user.Id,
                Created = DateTime.Now,
                FolderId = set.FolderId,
                Image = set.Image,
                Desc = set.Desc,
                Title = set.Title,
                Questions = new List<Question>()
            };

            await db.QuestionSets.AddAsync(questionSet);
            await db.SaveAsync();
            QuestionSetView view = new QuestionSetView(questionSet, user.UserName, await GetBreadcrumbsAsync(questionSet.Id));
            return Ok(view);
        }

        [HttpPost("{id}/associate")]
        [Authorize]
        public async Task<IActionResult> Associate(string id, string[] qsts)
        {
            var set = await db.QuestionSets.GetAsync(id);
            if (set == null)
                return NotFound(id);
            List<Question> questions = new List<Question>();
            foreach (var quest in qsts)
            {
                var q = await db.Questions.GetAsync(quest);
                if (q == null)
                    return BadRequest(quest);
                q.QuestionSetId = set.Id;
                questions.Add(q);
            }

            db.Questions.UpdateRange(questions);
            await db.SaveAsync();
            return Ok(qsts);
        }

        [HttpPost("{id}/reorder")]
        [Authorize]
        public async Task<IActionResult> Reorder(string id, string[] qsts)
        {
            var set = await db.QuestionSets.GetAsync(id);
            if (set == null)
                return NotFound(id);

            List<Question> questions = new List<Question>();
            int order = 0;
            foreach (var quest in qsts)
            {
                var q = await db.Questions.GetAsync(quest);
                if (q == null)
                    return BadRequest(quest);
                q.Order = order;
                questions.Add(q);
                order++;
            }
            db.Questions.UpdateRange(questions);
            await db.SaveAsync();
            return Ok(qsts);
        }

        [HttpPost("{id}/publish")]
        [Authorize]
        public async Task<IActionResult> Publish(string id)
        {
            var set = await db.QuestionSets.GetAsync(id);
            if (set == null)
                return NotFound(id);
            string userId = userManager.GetUserId(User);
            var msg = await db.ModerMsgs.GetByQuestionSetId(id);
            if (msg == null)
            {
                ModerMsg m = new ModerMsg()
                {
                    Id = Guid.NewGuid().ToString(),
                    Create = DateTime.Now,
                    Status = Status.Moderation,
                    AppUserId = userId,
                    QuestionSetId = id
                };
                db.ModerMsgs.Add(m);
                await db.SaveAsync();
                return Ok(m);
            }
            if (msg?.Status == Status.Moderation)
                return BadRequest($"{id} on moderation");
            msg.Status = Status.Moderation;
            db.ModerMsgs.Update(msg);
            await db.SaveAsync();
            return Ok(msg);
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> Patch(string Id, [FromBody]QuestionSetView set)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            QuestionSet questionSet = await db.QuestionSets.GetWithQuestionsAsync(Id);

            if (questionSet == null)
                return NotFound(Id);
            Folder folder = await db.Folders.GetAsync(set.FolderId);
            if (set.FolderId != "root" && folder == null)
                return BadRequest($"Not found folder {set.FolderId}");

            //Опасная зона
            set.FolderId = folder?.Id;
            questionSet.FolderId = set.FolderId;
            questionSet.Title = set.Title;
            questionSet.Image = set.Image;
            questionSet.Desc = set.Desc;

            db.QuestionSets.Update(questionSet);
            await db.SaveAsync();

            QuestionSetView view = new QuestionSetView(questionSet,User.Identity.Name ,await GetBreadcrumbsAsync(questionSet.Id));
            return Ok(view);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string Id)
        {
            var set = await db.QuestionSets.GetAsync(Id);
            if (set == null)
                return NotFound(Id);
            db.QuestionSets.Remove(set);
            await db.SaveAsync();
            return new StatusCodeResult(204);
        }

        private async Task<List<Breadcrumb>> GetBreadcrumbsAsync(string setId)
        {
            if (setId == null)
                return new List<Breadcrumb>();

            var userId = userManager.GetUserId(User);
            var breadcrumbs = new List<Breadcrumb>();
            QuestionSet set = await db.QuestionSets.GetAsync(setId);
            Folder parentFolder = await db.Folders.GetAsync(set.FolderId);
            while (parentFolder != null)
            {
                breadcrumbs.Add(new Breadcrumb(parentFolder));
                parentFolder = await db.Folders.GetAsync(parentFolder.ParentId);
            }
            breadcrumbs.Reverse();
            return breadcrumbs;
        }
    }
}