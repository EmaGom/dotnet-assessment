using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TimeChimp.Backend.Assessment.Models
{
    [ExcludeFromCodeCoverage]
    public class Feed
    {
        public Feed() { }

        [Key]
        public int Id { get; set; }
        public DateTime PostedDateTime { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
