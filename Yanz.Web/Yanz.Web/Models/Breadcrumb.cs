using Yanz.DAL.Entities;

namespace Yanz.Web.Models
{
    public class Breadcrumb
    {
        public string Id { get; set; }
        public string Title { get; set; }

        public Breadcrumb(Folder folder)
        {
            Id = folder.Id;
            Title = folder.Title;
        }
    }
}
