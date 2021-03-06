﻿using System;
using System.Collections.Generic;
using System.Linq;
using Yanz.DAL.Entities;

namespace Yanz.Web.Models
{
    public class SetView
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
        public string Owner { get; set; }
        public System.Collections.Generic.List<QuestionView> Questions { get; set; }
        #endregion

        public SetView()
        {

        }
    }
}
