namespace Yanz.DAL.Entities
{
    /// <summary>
    /// Public QuestionSet
    /// </summary>
    public class Set
    {
        public string Id { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public System.DateTime Created { get; set; }

        public int CopyCount { get; set; }
        public string Desc { get; set; }
        public string Title { get; set; }

        /// <summary>
        /// Превью
        /// </summary>
        public string Image { get; set; }

        #region Связи
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public System.Collections.Generic.List<Question> Questions { get; set; }
        #endregion

    }
}
