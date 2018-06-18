namespace Yanz.DAL.Entities
{
    public class QuestionSet
    {
        public string Id { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public System.DateTime Created { get; set; }

        public string Title { get; set; }
        public string Desc { get; set; }

        /// <summary>
        /// Превью
        /// </summary>
        public string Image { get; set; }

        #region Связи
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public Folder Folder { get; set; }
        public string FolderId { get; set; }
        public System.Collections.Generic.List<Question> Questions { get; set; }
        #endregion
    }
}
