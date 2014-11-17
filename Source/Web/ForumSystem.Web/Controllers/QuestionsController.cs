namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using ForumSystem.Data.Common.Repository;
    using ForumSystem.Data.Models;
    using ForumSystem.Web.Infrastructure;
    using ForumSystem.Web.InputModels.Questions;
    using ForumSystem.Web.ViewModels.Questions;

    using Microsoft.AspNet.Identity;

    public class QuestionsController : Controller
    {
        private readonly IDeletableEntityRepository<Post> posts;

        private readonly ISanitizer sanitizer;

        public QuestionsController(IDeletableEntityRepository<Post> posts, ISanitizer sanitizer)
        {
            this.posts = posts;
            this.sanitizer = sanitizer;
        }

        // /questions/26864653/mysql-workbench-crash-on-start-on-windows
        public ActionResult Display(int id, string url, int page = 1)
        {
            var postViewModel = this.posts.All().Where(x => x.Id == id)
                .Project().To<QuestionDisplayViewModel>().FirstOrDefault();

            if (postViewModel == null)
            {
                return this.HttpNotFound("No such post");
            }

            return this.View(postViewModel);
        }

        // /questions/tagged/javascript
        public ActionResult GetByTag(string tag)
        {
            return this.Content(tag);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Ask()
        {
            var model = new AskInputModel();
            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Ask(AskInputModel input)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.Identity.GetUserId();

                var post = new Post
                    {
                        Title = input.Title,
                        Content = this.sanitizer.Sanitize(input.Content),
                        AuthorId = userId,

                        // TODO: Tags
                        // TODO: Author
                    };

                this.posts.Add(post);
                this.posts.SaveChanges();
                return this.RedirectToAction("Display", new { id = post.Id, url = "new" });
            }

            return this.View(input);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Upvote(int id, string url)
        {
            var post = this.posts.GetById(id);

            if (post == null)
            {
                return this.HttpNotFound();
            }

            var userId = this.User.Identity.GetUserId();
            var userVote = this.GetUserVote(post, userId);

            userVote.Value = userVote.Value >= 0 ? 1 : 0;

            this.posts.SaveChanges();
            var rating = post.Votes.Sum(v => v.Value);

            return this.PartialView("_VotesPartial", rating);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Downvote(int id, string url)
        {
            var post = this.posts.GetById(id);

            if (post == null)
            {
                return this.HttpNotFound();
            }

            var userId = this.User.Identity.GetUserId();
            var userVote = this.GetUserVote(post, userId);

            userVote.Value = userVote.Value == -1 ? -1 : userVote.Value - 1;
            this.posts.SaveChanges();
            
            var rating = post.Votes.Sum(v => v.Value);

            return this.PartialView("_VotesPartial", rating);
        }

        private Vote GetUserVote(Post post, string userId)
        {
            var userVote = post.Votes
                .FirstOrDefault(v => v.AuthorId == userId);

            if (userVote == null)
            {
                userVote = new Vote
                {
                    AuthorId = userId,
                    CreatedOn = DateTime.Now,
                    Post = post,
                    Value = 0
                };

                post.Votes.Add(userVote);
            }

            return userVote;
        }
    }
}