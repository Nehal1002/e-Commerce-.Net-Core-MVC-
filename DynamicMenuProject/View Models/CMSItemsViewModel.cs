using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicMenuProject.View_Models
{
    public class CMSItemsViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Remote("IsPageNameExist", "CMS", AdditionalFields = "Id",
                ErrorMessage = "Page name already exists.")]
        public string PageName { get; set; }

        [Required]
        [Remote("IsPageUrlExist", "CMS", AdditionalFields = "Id",
                ErrorMessage = "Page url already exists.")]
        public string PageUrl { get; set; }

        [Required(ErrorMessage = "Please Fill Description.")]
        public string Description { get; set; }

        public string BannerImage { get; set; }

        [Required(ErrorMessage = "Please Fill BannerImage.")]
        public IFormFile BannerImageFile { get; set; }
        
    }
}
