using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yanz.DAL.Entities;

namespace Yanz.DAL.Interfaces
{
    public interface IModerMsgRepository : IRepository<ModerMsg>
    {
        Task<ModerMsg> GetByQuestionSetId(string questionSetId);
        Task<IEnumerable<ModerMsg>> GetByUserId(string userId);
        Task<IEnumerable<ModerMsg>> GetOnModerationAsync();
    }
}
