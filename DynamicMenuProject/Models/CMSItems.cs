using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DynamicMenuProject.Models
{
    public partial class CMSItems
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Fill PageName.")]
        public string PageName { get; set; }
        [Required(ErrorMessage = "Please Fill PageUrl.")]
        public string PageUrl { get; set; }
        [Required(ErrorMessage = "Please Fill Description.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please Fill BannnerImage.")]
        public string BannerImage { get; set; }
    }
}
