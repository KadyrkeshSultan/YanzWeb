namespace Yanz.DAL.Entities
{
    public class Choice
    {
        public string Id { get; set; }

        /// <summary>
        /// Ссылка на изображение
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Порядок расположения выбора
        /// </summary>
        public int Order { get; set; }

        public bool IsCorrect { get; set; }

        /// <summary>
        /// Тескс ответа, можно HTML
        /// </summary>
        public string Content { get; set; }

        public Question Question { get; set; }
        public string QuestionId { get; set; }
    }
}
