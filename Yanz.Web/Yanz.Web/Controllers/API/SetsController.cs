using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Yanz.DAL.Entities;
using Yanz.DAL.Interfaces;
using Yanz.Web.Models;

namespace Yanz.Controllers.API
{
    [Produces("application/json")]
    [Route("api/Sets")]
    [Authorize]
    public class SetsController : ControllerBase
    {
        IUnitOfWork db;
        UserManager<AppUser> userManager;

        public SetsController(IUnitOfWork _db, UserManager<AppUser> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyPublicSetsAsync()
        {
            var userId = userManager.GetUserId(User);
            var sets = await db.Sets.GetAllWithQuestions(userId);
            List<SetView> views = new List<SetView>();
            foreach (var s in sets)
                views.Add(new SetView(s, User.Identity.Name));
            return Ok(views);
        }

        [HttpGet("messages")]
        public async Task<IActionResult> GetMyModerMsgs()
        {
            var userId = userManager.GetUserId(User);
            var messages = await db.ModerMsgs.GetByUserId(userId);
            return Ok(messages);
        }
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = userManager.GetUserId(User);
            var set = await db.Sets.GetByUserAsync(id, userId);
            if (set == null)
                return NotFound(id);
            db.Sets.Remove(set);
            await db.SaveAsync();
            return new StatusCodeResult(204);
        }
    }
}
