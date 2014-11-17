namespace ForumSystem.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ForumSystem.Data.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;

            // TODO: Remove in production
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            if (context.Posts.Any())
            {
                return;
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = new ApplicationUser()
            {
                UserName = "test@test.test",
            };

            userManager.Create(user, "qweqwe");

            for (int i = 0; i < 14; i++)
            {
                var newPost = new Post
                {
                    Title = "Random Post Title " + (i + 1),
                    Content = "Random Post Content Random Post Content Random Post Content Random Post Content Random Post Content Random Post Content",
                    Author = user,
                    Tags = new HashSet<Tag>(),
                };

                for (int j = 0; j < 4; j++)
                {
                    var newTag = new Tag
                    {
                        Name = "Test tag " + (j + 1),
                    };

                    newPost.Tags.Add(newTag);
                }

                context.Posts.Add(newPost);
            }

            context.SaveChanges();
        }
    }
}
