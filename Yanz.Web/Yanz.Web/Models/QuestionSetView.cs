using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Yanz.DAL.Entities;

namespace Yanz.Web.Models
{
    public class QuestionSetView
    {
        public string Id { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public System.DateTime Created { get; set; }

        /// <summary>
        /// Владелец
        /// </summary>
        public string Owner { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Min length 1")]
        public string Title { get; set; }

        [Required]
        public string Desc { get; set; }
        /// <summary>
        /// Превью
        /// </summary>
        public string Image { get; set; }

        [Required]
        public string FolderId { get; set; }
        public System.Collections.Generic.List<Breadcrumb> Breadcrumbs { get; set; }

        #region Связи
        public List<QuestionView> Questions { get; set; }
        #endregion

        public QuestionSetView()
        {

        }
    }
}
