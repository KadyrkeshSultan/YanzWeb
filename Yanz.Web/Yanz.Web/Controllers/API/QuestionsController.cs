using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Yanz.DAL.Entities;
using Yanz.DAL.Interfaces;
using Yanz.Web.Models;

namespace Yanz.Controllers.API
{
    [Produces("application/json")]
    [Route("api/Questions")]
    [Authorize]
    public class QuestionsController : ControllerBase
    {
        IUnitOfWork db;
        UserManager<AppUser> userManager;
        readonly string[] kinds = { "choice", "multiple", "text", "sorter" };
        readonly int MaxQuestionCount = 30;

        public QuestionsController(IUnitOfWork _db, UserManager<AppUser> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }

        // GET: api/Questions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var qst = await db.Questions.GetWithChoices(id);
            if (qst == null)
                return NotFound();
            qst.Choices = qst.Choices.OrderBy(c => c.Order).ToList();
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Question, QuestionView>();
            }).CreateMapper();

            QuestionView view = mapper.Map<Question, QuestionView>(qst);
            return Ok(view);
        }

        // POST: api/Questions
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]QuestionView question)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string checkKind = CheckKind(question);
            if (!string.IsNullOrEmpty(checkKind))
                return BadRequest(checkKind);

            var set = await db.QuestionSets.GetWithQuestionsAsync(question.QuestionSetId);
            if (set == null)
                return NotFound(question.QuestionSetId);

            if (set.Questions.Count >= MaxQuestionCount)
                return BadRequest($"Max {MaxQuestionCount} questions in set");

            List<Choice> choices = new List<Choice>();
            if (question.Kind != "text")
                foreach (var c in question.Choices.OrderBy(q => q.Order))
                    choices.Add(new Choice()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Image = c.Image,
                        Order = c.Order,
                        IsCorrect = c.IsCorrect,
                        Content = c.Content,
                        QuestionId = question.QuestionSetId
                    });

            question.Order = set.Questions.Count;
            Question qst = new Question()
            {
                Id = Guid.NewGuid().ToString(),
                Title = question.Title,
                Image = question.Image,
                IsPoll = false,
                IsTrueCorrect = question.IsTrueCorrect,
                Weight = question.Weight,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                Kind = question.Kind,
                Content = question.Content,
                Explanation = question.Explanation,
                QuestionSetId = question.QuestionSetId,
                Order = question.Order,
                Choices = choices
            };
            db.Questions.Add(qst);
            await db.SaveAsync();


            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Question, QuestionView>();
            }).CreateMapper();

            QuestionView view = mapper.Map<Question, QuestionView>(qst);
            return CreatedAtAction("Get", new { id = view.Id }, Ok(view));
        }

        // POST: api/Questions
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(string id, [FromBody]QuestionView question)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var quest = await db.Questions.GetWithChoices(id);
            if (quest == null)
                return NotFound();

            string checkKind = CheckKind(question);
            if (!string.IsNullOrEmpty(checkKind))
                return BadRequest(checkKind);

            var set = await db.QuestionSets.GetWithQuestionsAsync(question.QuestionSetId);
            if (set == null)
                return NotFound(question.QuestionSetId);

            if (set.Questions.Count >= MaxQuestionCount && quest.QuestionSetId != question.QuestionSetId)
                return BadRequest($"Max {MaxQuestionCount} questions in set");
            //Удаляем старые
            db.Choices.RemoveRange(quest.Choices);

            List<Choice> choices = new List<Choice>();
            if (question.Kind != "text")
                foreach (var c in question.Choices.OrderBy(q => q.Order))
                    choices.Add(new Choice() {
                        Id = Guid.NewGuid().ToString(),
                        Image = c.Image,
                        Order = c.Order,
                        IsCorrect = c.IsCorrect,
                        Content = c.Content,
                        QuestionId = question.QuestionSetId
                    });

            Question nQuestion = new Question()
            {
                Title = question.Title,
                Image = question.Image,
                IsPoll = false,
                IsTrueCorrect = question.IsTrueCorrect,
                Weight = question.Weight,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                Kind = question.Kind,
                Content = question.Content,
                Explanation = question.Explanation,
                QuestionSetId = question.QuestionSetId,
                Order = question.Order,
                Choices = choices
            };

            UpdateQst(quest, nQuestion);

            db.Questions.Update(quest);
            await db.SaveAsync();

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Question, QuestionView>();
            }).CreateMapper();

            QuestionView view = mapper.Map<Question, QuestionView>(quest);
            return Ok(view);
        }

        private string CheckKind(QuestionView question)
        {
            if (kinds.FirstOrDefault(k => k == question.Kind) == null)
                return $"Kind {question.Kind} does not exist";

            if (question.Choices == null || question.Choices?.Count < 2 && question.Kind != "text")
                return "At least two choices, if this is not text";

            if (question.Kind == "choice" && question.Choices.Count(c => c.IsCorrect == true) != 1)
                return "For the Kind choice, there must be only one isCorrect";

            if (question.Kind == "multiple" && question.Choices.Count(c => c.IsCorrect == true) < 1)
                return "For the Kind multiple, at least one isCorrect";
            return string.Empty;
        }

        /// <summary>
        /// Обновляем все свойства кроме Id
        /// </summary>
        /// <param name="old">То что нужно обновить</param>
        /// <param name="newQ">На то что нужно обновить</param>
        private void UpdateQst(Question old, Question newQ)
        {
            old.Choices = newQ.Choices;
            old.Content = newQ.Content;
            old.Created = newQ.Created;
            old.Explanation = newQ.Explanation;
            old.Image = newQ.Image;
            old.IsPoll = newQ.IsPoll;
            old.IsTrueCorrect = newQ.IsTrueCorrect;
            old.Kind = newQ.Kind;
            old.Modified = newQ.Modified;
            old.QuestionSetId = newQ.QuestionSetId;
            old.Title = newQ.Title;
            old.Weight = newQ.Weight;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var qst = await db.Questions.GetAsync(id);
            if (qst == null)
                return NotFound();

            db.Questions.Remove(qst);
            await db.SaveAsync();
            return NoContent();
        }
    }
}
