using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace birthreg.Models
{
    public class Parent
    {
        public int Id { get; set; }
        [Display(Name = "Family Name")]
        public string FamilyName { get; set; }
        [Display(Name = "Father's Name")]
        public string FatherName { get; set; }
        [Display(Name = "Mother's Name")]
        public string MotherName { get; set; }
        [Display(Name = "Father's Phone Number")]
        public string FatherPhoneNumber { get; set; }
        [Display(Name = "Mother's Phone Number")]
        public string MotherPhoneNumber { get; set; }
    }
}
