namespace ForumSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using ForumSystem.Data.Common.Repository;
    using ForumSystem.Data.Models;
    using ForumSystem.Web.ViewModels.Questions;

    using Kendo.Mvc.UI;

    using Microsoft.AspNet.Identity;

    public class PostsController : KendoGridAdministrationController
    {
        public PostsController(IDeletableEntityRepository<Post> data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, QuestionAdministrationViewModel model)
        {
            var databaseModel = base.Create<Post>(model);

            if (databaseModel != null)
            {
                model.Id = databaseModel.Id;
                databaseModel.AuthorId = this.User.Identity.GetUserId();
            }

            return this.GridOperation(model, request);
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, QuestionAdministrationViewModel model)
        {
            base.Update<Post, QuestionAdministrationViewModel>(model, model.Id);
            return this.GridOperation(model, request);
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, QuestionAdministrationViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                this.Data.Delete(model.Id.Value);
                this.Data.SaveChanges();
            }

            return this.GridOperation(model, request);
        }

        protected override IEnumerable GetData()
        {
            return this.Data.All()
                .Project()
                .To<QuestionAdministrationViewModel>();
        }

        protected override T GetById<T>(object id)
        {
            return this.Data.GetById((int)id) as T;
        }
    }
}