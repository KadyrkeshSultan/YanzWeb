using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Yanz.DAL.Entities;

namespace Yanz.Web.Models
{
    public class QuestionView
    {
        public string Id { get; set; }

        [StringLength(255, MinimumLength = 1, ErrorMessage = "Min length 1")]
        public string Title { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public string Content { get; set; }
        public string Kind { get; set; }
        public string Image { get; set; }
        //public bool IsOnBoarding { get; set; }
        public System.DateTime Modified { get; set; }
        public System.DateTime Created { get; set; }

        [Required]
        public string QuestionSetId { get; set; }
        public string Explanation { get; set; }
        public bool? IsTrueCorrect { get; set; }
        //public string OnBoardingId { get; set; }
        public int Weight { get; set; }
        public bool IsPoll { get; set; }
        public int Order { get; set; }

        public List<ChoiceView> Choices { get; set; }

        public QuestionView()
        {
            Weight = 1;
        }
    }
}
