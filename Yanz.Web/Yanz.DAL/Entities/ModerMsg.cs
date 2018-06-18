namespace Yanz.DAL.Entities
{
    /// <summary>
    /// Moder message
    /// </summary>
    public class ModerMsg
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public System.DateTime Create { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public QuestionSet QuestionSet { get; set; }
        public string QuestionSetId { get; set; }
        public Status Status { get; set; }
    }
    public enum Status
    {
        Public,
        Moderation,
        Disapproved
    }
}
