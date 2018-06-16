using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore.Models.Product
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        [Range(1,int.MaxValue)]
        public int Price { get; set; }
        [Required]
        [Range(0,int.MaxValue)]
        public int Quantity { get; set; }
        [Display(Name="Image")]
        [Required]
        public String ImgURL { get; set; }
    }
}