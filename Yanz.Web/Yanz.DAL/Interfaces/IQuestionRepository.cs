using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yanz.DAL.Entities;

namespace Yanz.DAL.Interfaces
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<Question> GetWithChoices(string id);
        Task<IEnumerable<Question>> GetWithChoicesByQuestionSet(string questionSetId);
    }
}
