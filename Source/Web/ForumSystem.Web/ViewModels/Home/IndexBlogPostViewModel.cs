namespace ForumSystem.Web.ViewModels.Home
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;

    using ForumSystem.Data.Models;
    using ForumSystem.Web.Infrastructure.Mapping;
    using ForumSystem.Web.ViewModels.Tags;

    public class IndexBlogPostViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public IList<TagViewModel> Tags { get; set; }

        public int? VoteRating { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Post, IndexBlogPostViewModel>()
                .ForMember(vm => vm.VoteRating, o => o.MapFrom(p => p.Votes.Sum(v => v.Value)));
        }
    }
}