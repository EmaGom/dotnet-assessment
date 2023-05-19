using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TimeChimp.Backend.Assessment.Models
{
    [ExcludeFromCodeCoverage]
    public class Feed
    {
        public Feed() { }

        [Key]
        public int Id { get; set; }
        public DateTime PublishDate { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        [ForeignKey("Id")]
        public int CategoryId { get; set; }
        [NotMapped]
        public Category Category { get; set; }
    }
}
