using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicMenuProject.View_Models
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            //Claims = new List<string>();
            Roles = new List<string>();
        }
        public string Id { get; set; }
        [Required]

        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Display(Name = "Profile Picture")]
        public string ProfilePicture { get; set; }
        
        [Display(Name = "Profile Picture")]
        public IFormFile ProfilePictureFile { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [Display(Name = "State")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [Display(Name = "City")]
        public int CityId { get; set; }

        [Required]
        [Display(Name = "Address 1")]
        public string Address1 { get; set; }

        [Required]
        [Display(Name = "Address 2")]
        public string Address2 { get; set; }

        //public List<string> Claims { get; set; }
        public IList<string> Roles { get; set; }
    }
}
