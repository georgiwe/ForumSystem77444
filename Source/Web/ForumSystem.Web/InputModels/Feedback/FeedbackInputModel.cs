namespace ForumSystem.Web.InputModels.Feedback
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using ForumSystem.Data.Models;
    using ForumSystem.Web.Infrastructure.Mapping;

    public class FeedbackInputModel : IMapFrom<Feedback>
    {
        [Required]
        [MaxLength(20)]
        [AllowHtml]
        [UIHint("SinglelineText")]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        [UIHint("MultilineText")]
        public string Content { get; set; }
    }
}