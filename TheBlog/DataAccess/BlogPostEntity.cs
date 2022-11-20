using System.ComponentModel.DataAnnotations;

namespace TheBlog.DataAccess
{
    public class BlogPostEntity
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Accepted maximum length of 30 characters")]
        [MinLength(2, ErrorMessage = "Required minimum length of 2 characters.")]
        public string Headline { get; set; }

        [Required]
        [StringLength(2000, ErrorMessage = "Accepted maximum length of 2000 characters")]
        [MinLength(5, ErrorMessage = "Required minimum length of 5 characters")]
        public string Text { get; set; }

        public DateTime Date { get; set; }
    }
}