namespace Yanz.DAL.Entities
{
    public class Question
    {
        public string Id { get; set; }

        #region Свойства
        /// <summary>
        /// Текст вопроса
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Ссылка на изображение
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Для голосования
        /// </summary>
        public bool IsPoll { get; set; }

        /// <summary>
        /// Правда является правильной? Для True/False
        /// </summary>
        public bool? IsTrueCorrect { get; set; }

        /// <summary>
        /// Балл за правильный ответ
        /// </summary>
        public int Weight { get; set; }

        public System.DateTime Created { get; set; }
        public System.DateTime Modified { get; set; }

        /// <summary>
        /// Тип вопроса, select or checkbox
        /// </summary>
        public string Kind { get; set; }


        /// <summary>
        /// Опциональный текст вопроса, можно HTML
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Пояснение, можно HTML
        /// </summary>
        public string Explanation { get; set; }

        public int Order { get; set; }
        #endregion

        #region Связи
        public QuestionSet QuestionSet { get; set; }
        public string QuestionSetId { get; set; }

        public Set Set { get; set; }
        public string SetId { get; set; }
        /// <summary>
        /// Варианты ответа
        /// </summary>
        public System.Collections.Generic.List<Choice> Choices { get; set; }
        #endregion
    }
}
