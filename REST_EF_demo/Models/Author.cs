using System.ComponentModel.DataAnnotations;

namespace REST_EF_demo.Models
{
    public class Author
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }

        // One-to-many relationship with books
        public List<Book>? Books { get; set; }
    }
}
