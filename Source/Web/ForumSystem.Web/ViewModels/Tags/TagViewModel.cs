namespace ForumSystem.Web.ViewModels.Tags
{
    using System.Collections.Generic;

    using ForumSystem.Data.Models;
    using ForumSystem.Web.Infrastructure.Mapping;
    
    public class TagViewModel : IMapFrom<Tag>
    {
        public string Name { get; set; }
    }
}