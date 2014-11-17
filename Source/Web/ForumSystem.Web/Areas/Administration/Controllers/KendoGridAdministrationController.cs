namespace ForumSystem.Web.Areas.Administration.Controllers
{
    using System.Collections;
    using System.Data.Entity;
    using System.Web.Mvc;

    using AutoMapper;
    using ForumSystem.Data.Common.Repository;
    using ForumSystem.Data.Models;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;

    public abstract class KendoGridAdministrationController : AdminController
    {
        public KendoGridAdministrationController(IDeletableEntityRepository<Post> data)
            : base(data)
        {
        }

        [HttpPost]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var ads =
                this.GetData()
                .ToDataSourceResult(request);

            return this.Json(ads);
        }

        protected abstract IEnumerable GetData();

        protected abstract T GetById<T>(object id) where T : class;

        [NonAction]
        protected virtual T Create<T>(object model) where T : class
        {
            if (model != null && ModelState.IsValid)
            {
                var databaseModel = Mapper.Map<T>(model);
                this.ChangeEntityStateAndSave(databaseModel, EntityState.Added);
                return databaseModel;
            }

            return null;
        }

        [NonAction]
        protected virtual void Update<TModel, TViewModel>(TViewModel model, object id)
            where TModel : class
            where TViewModel : class
        {
            if (model != null && ModelState.IsValid)
            {
                var databaseModel = this.GetById<TModel>(id);
                Mapper.Map<TViewModel, TModel>(model, databaseModel);
                this.ChangeEntityStateAndSave(databaseModel, EntityState.Modified);
            }
        }

        protected JsonResult GridOperation<T>(T model, [DataSourceRequest]DataSourceRequest request)
        {
            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }

        private void ChangeEntityStateAndSave(object databaseModel, EntityState state)
        {
            var entry = this.Data.Context.Entry(databaseModel);
            entry.State = state;
            this.Data.SaveChanges();
        }
    }
}