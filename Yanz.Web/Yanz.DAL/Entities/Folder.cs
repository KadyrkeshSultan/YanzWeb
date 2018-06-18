namespace Yanz.DAL.Entities
{
    public class Folder
    {
        public string Id { get; set; }
        public string Title { get; set; }

        public string ParentId { get; set; }

        public System.Collections.Generic.List<QuestionSet> QuestionSets { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
    }
}
