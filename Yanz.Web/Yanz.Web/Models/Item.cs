using Yanz.DAL.Entities;

namespace Yanz.Web.Models
{
    public class Item
    {
        public string Id { get; set; }

        /// <summary>
        /// Например Set или Folder
        /// </summary>
        public string Kind { get; set; }

        public int? QuestionsCount { get; set; }

        public string Title { get; set; }

        public Item(Folder folder)
        {
            Id = folder.Id;
            Kind = "folder";
            QuestionsCount = null;
            Title = folder.Title;
        }

        public Item(QuestionSet set)
        {
            Id = set.Id;
            Kind = "set";
            QuestionsCount = set.Questions.Count;
            Title = set.Title;
        }
    }
}
