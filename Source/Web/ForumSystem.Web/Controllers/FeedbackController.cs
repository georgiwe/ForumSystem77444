namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Caching;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using ForumSystem.Common.Constants;
    using ForumSystem.Common.Extensions;
    using ForumSystem.Common.Pagers;
    using ForumSystem.Data.Common.Repository;
    using ForumSystem.Data.Models;
    using ForumSystem.Web.Infrastructure;
    using ForumSystem.Web.InputModels.Feedback;
    using ForumSystem.Web.ViewModels.Feedbacks;
    using Microsoft.AspNet.Identity;

    [Authorize]
    public class FeedbackController : Controller
    {
        private IDeletableEntityRepository<Feedback> data;
        private ISanitizer sanitizer;

        public FeedbackController(IDeletableEntityRepository<Feedback> data, ISanitizer sanitizer)
        {
            this.data = data;
            this.sanitizer = sanitizer;
        }

        // GET: Feedback/Create
        [HttpGet]
        public ActionResult Create()
        {
            var model = new FeedbackInputModel();
            return this.View(model);
        }

        // POST: Feedback/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FeedbackInputModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var databaseModel = Mapper.Map<Feedback>(model);
                databaseModel.AuthorId = this.User.Identity.GetUserId();

                this.data.Add(databaseModel);
                this.data.SaveChanges();

                this.ClearFbCache();

                this.TempData[Const.Success] = Const.ThanksForFeedback;
                return this.RedirectToAction("Index", "Home");
            }

            return this.View(model);
        }

        [HttpGet]
        public ActionResult All(int page = 1, int pageSize = 4)
        {
            if (page < 1)
            {
                page = 1;
            }

            this.UpdatePager(ref page, pageSize);
            var feedbacks = this.Populate(page, pageSize);

            return this.View(feedbacks);
        }

        private void UpdatePager(ref int page, int pageSize)
        {
            if (this.Session[Const.Pager] == null)
            {
                this.Session[Const.Pager] = new PagingHelper();
            }

            var pager = this.Session[Const.Pager] as PagingHelper;

            var totalFeedbacks = this.data.All().Count();
            pager.CurrentPage = page;
            pager.TotalPages = totalFeedbacks / pageSize;

            if (totalFeedbacks % pageSize > 0)
            {
                pager.TotalPages++;
            }

            if (page > pager.TotalPages)
            {
                page = pager.TotalPages;
            }

            pager.PageSize = pageSize;
            pager.CurrentPage = page;

            this.Session[Const.Pager] = pager;
        }

        private IEnumerable<FeedbackViewModel> Populate(int page, int pageSize)
        {
            IEnumerable<FeedbackViewModel> feedbacks = null;

            if (HttpContext.Cache[Const.FeedbackCacheKey + page] == null)
            {
                feedbacks = this.data.All()
                    .Project()
                    .To<FeedbackViewModel>()
                    .OrderBy(f => f.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList()
                    .ForEach<FeedbackViewModel>(f => f.Content = this.sanitizer.Sanitize(f.Content));

                this.HttpContext.Cache.Add(
                    Const.FeedbackCacheKey + page, feedbacks, null, DateTime.Now.AddMinutes(1), TimeSpan.Zero, CacheItemPriority.Default, null);
            }

            feedbacks = HttpContext.Cache[Const.FeedbackCacheKey + page] as IEnumerable<FeedbackViewModel>;

            return feedbacks;
        }

        private void ClearFbCache()
        {
            // TODO: Implement
            // Please rebuild or delete bin/obj in order
            // to clear the cache and see that Create does work
            // Thank you
        }
    }
}