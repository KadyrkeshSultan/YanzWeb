using System.ComponentModel.DataAnnotations;
using Yanz.DAL.Entities;

namespace Yanz.Web.Models
{
    public class ChoiceView
    {
        public string Id { get; set; }
        public string Image { get; set; }
        public int Order { get; set; }
        public bool IsCorrect { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Min length 1")]
        public string Content { get; set; }

        public ChoiceView()
        {

        }

        public ChoiceView(Choice choice)
        {
            Id = choice.Id;
            Image = choice.Image;
            Order = choice.Order;
            IsCorrect = choice.IsCorrect;
            Content = choice.Content;
        }
    }
}
