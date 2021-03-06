﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yanz.DAL.Entities;

namespace Yanz.DAL.Interfaces
{
    public interface IQuestionSetRepository : IRepository<QuestionSet>
    {
        Task<QuestionSet> GetWithQuestionsNoTrackAsync(string userId, string id);
        Task<QuestionSet> GetWithQuestionsAsync(string userId, string id);

        Task<IEnumerable<QuestionSet>> GetAllWithQuestionsNoTrack(string userId);
        Task<IEnumerable<QuestionSet>> GetAllWithQuestions(string userId);

        Task<QuestionSet> GetWithQuestionsAsync(string id);
        Task<QuestionSet> GetWithQuestionsNoTrackAsync(string id);

        Task<IEnumerable<QuestionSet>> GetWithQstByFolder(string folderId);
    }
}
