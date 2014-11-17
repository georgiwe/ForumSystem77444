namespace ForumSystem.Web.ViewModels.Questions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using AutoMapper;

    using ForumSystem.Data.Models;
    using ForumSystem.Web.Infrastructure.Mapping;

    [Bind(Exclude = "CreatedOn, AuthorName, ModifiedOn, IsDeleted")]
    public class QuestionAdministrationViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public int? Id { get; set; }

        public string AuthorName { get; set; }

        public string Title { get; set; }

        [UIHint("tinymce_full")]
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Post, QuestionAdministrationViewModel>().ForMember(vm => vm.AuthorName, o => o.MapFrom(p => p.Author.UserName)).ReverseMap();
        }
    }
}