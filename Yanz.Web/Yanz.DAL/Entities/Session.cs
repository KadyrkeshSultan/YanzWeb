namespace Yanz.DAL.Entities
{
    public class Session
    {
        public string Id { get; set; }
        /// <summary>
        /// Код для сессии
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Показывать ответы сразу (per-question - да) / (manual - нет)
        /// </summary>
        public string FeedbackMode { get; set; }

        /// <summary>
        /// Активна ли сессия в данный момент
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Удалена ли(для пользователя) или архивирована ли (для админа) 
        /// </summary>
        public bool IsArchived { get; set; }

        /// <summary>
        /// Разрешить только одну попытку на вопрос
        /// </summary>
        public bool IsOneAttempt { get; set; }

        /// <summary>
        /// Частичное оценивание
        /// </summary>
        public bool IsPartialGrading { get; set; }

        /// <summary>
        /// Раскрыть ответы
        /// </summary>
        public bool IsRevealed { get; set; }

        /// <summary>
        /// Перемешать варианты ответов
        /// </summary>
        public bool IsShuffleChoices { get; set; }

        /// <summary>
        /// Пошаговая сессия. Показывать каждый следующий вопрос по одному в фиксированном порядке (round-based - да) / (flexible - нет)
        /// </summary>
        public string Mode { get; set; }
        public string PlatformOnly { get; set; }

        /// <summary>
        /// Название сессии
        /// </summary>
        public string Title { get; set; }

        //TODO : заменить
        //public System.Collections.Generic.List<Question> Questions { get; set; }

        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
    }
}
