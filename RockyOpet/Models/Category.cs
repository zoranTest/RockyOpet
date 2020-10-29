using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RockyOpet.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]  // za validaciju 
        public string Name { get; set; }


        [DisplayName("Display order")]
        [Required]
        [Range(1,int.MaxValue,ErrorMessage ="Display order must be greater than 0")]
        public int DisplayOrder { get; set; }

    }
}
