namespace ForumSystem.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;

    using ForumSystem.Data.Common.Repository;
    using ForumSystem.Data.Models;

    [Authorize]
    public abstract class AdminController : Controller
    {
        private IDeletableEntityRepository<Post> data;

        public AdminController(IDeletableEntityRepository<Post> data)
        {
            this.data = data;
        }

        protected IDeletableEntityRepository<Post> Data
        {
            get
            {
                return this.data;
            }

            set
            {
                this.data = value;
            }
        }
    }
}