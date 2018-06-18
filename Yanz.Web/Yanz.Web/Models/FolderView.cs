using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Yanz.DAL.Entities;

namespace Yanz.Web.Models
{
    public class FolderView
    {
        public string Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Minimun title length 1", MinimumLength = 1)]
        public string Title { get; set; }

        [Required]
        public string ParentId { get; set; }
        public System.Collections.Generic.List<Breadcrumb> Breadcrumbs { get; set; }
        public System.Collections.Generic.List<Item> Items { get; set; }

        public FolderView()
        {

        }
    }
}
