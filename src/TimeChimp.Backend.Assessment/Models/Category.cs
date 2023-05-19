using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeChimp.Backend.Assessment.Models
{
    public class Category
    {
        public Category() { }

        [Key]
        public int Id { get; set; }
        public DateTime LastBuildDate { get; set; }
        public string Name { get; set; }

        [NotMapped]
        public IList<Feed> Feeds { get; set; }  
    }
}
