using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yanz.DAL.Entities;
using Yanz.DAL.Interfaces;

namespace Yanz.Controllers.Admin
{
    [Produces("application/json")]
    [Route("api/Moder")]
    [Authorize(Roles = "admin")]
    public class ModerController : ControllerBase
    {
        IUnitOfWork db;

        public ModerController(IUnitOfWork _db)
        {
            db = _db;
        }
        // GET: api/Moder
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var msgs = await db.ModerMsgs.GetOnModerationAsync();
            return Ok(msgs);
        }

        // POST: api/Moder
        [Authorize(Roles = "admin")]
        [HttpPost("{id}/public")]
        public async Task<IActionResult> Public(string id)
        {
            var msg = await db.ModerMsgs.GetAsync(id);
            if (msg == null)
                return NotFound(id);
            var qstSet = await db.QuestionSets.GetAsync(msg.QuestionSetId);
            if (qstSet == null)
                return NotFound($"Not found question set {msg.QuestionSetId}");

            Set set = new Set()
            {
                Id = Guid.NewGuid().ToString(),
                Created = DateTime.Now,
                Desc = qstSet.Desc,
                Image = qstSet.Image,
                AppUserId = qstSet.AppUserId
            };
            List<Question> questions = new List<Question>();
            var setQsts = await db.Questions.GetWithChoicesByQuestionSet(qstSet.Id);
            foreach (var q in setQsts)
            {
                Question qst = new Question()
                {
                    Id = Guid.NewGuid().ToString(),
                    Content = q.Content,
                    Order = q.Order,
                    Created = DateTime.Now,
                    Explanation = q.Explanation,
                    Image = q.Image,
                    IsPoll = q.IsPoll,
                    IsTrueCorrect = q.IsTrueCorrect,
                    Kind = q.Kind,
                    Modified = DateTime.Now,
                    Title = q.Title,
                    Weight = q.Weight,
                    SetId = set.Id
                };
                var choices = new List<Choice>();
                foreach (var c in q.Choices)
                    choices.Add(new Choice()
                    {
                        Content = c.Content,
                        Id = Guid.NewGuid().ToString(),
                        Image = c.Image,
                        Order = c.Order,
                        IsCorrect = c.IsCorrect,
                        QuestionId = qst.Id
                    });

                qst.Choices = choices;
                questions.Add(qst);
            }

            set.Questions = questions;
            db.Questions.AddRange(questions);
            msg.Status = Status.Public;
            db.Sets.Add(set);
            db.ModerMsgs.Update(msg);
            await db.SaveAsync();
            return Ok();
        }

        // POST: api/Moder
        [HttpPost("{id}/disapproved")]
        public async Task<IActionResult> Disapproved(string id, string text)
        {
            var msg = await db.ModerMsgs.GetAsync(id);
            if (msg == null)
                return NotFound(id);
            msg.Status = Status.Disapproved;
            msg.Text = text;
            db.ModerMsgs.Update(msg);
            await db.SaveAsync();
            return Ok(msg);
        }
    }
}
