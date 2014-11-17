namespace ForumSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ForumSystem.Data.Common.Models;

    public class Post : AuditInfo, IDeletableEntity
    {
        private ICollection<Tag> tags;
        private ICollection<Vote> votes;

        public Post()
        {
            this.votes = new HashSet<Vote>();
            this.tags = new HashSet<Tag>();
        }

        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Title { get; set; }

        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }
 
        [Required]
        public string Content { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<Tag> Tags
        {
            get
            {
                return this.tags;
            }

            set
            {
                this.tags = value;
            }
        }

        public virtual ICollection<Vote> Votes
        {
            get
            {
                return this.votes;
            }

            set
            {
                this.votes = value;
            }
        }
    }
}
