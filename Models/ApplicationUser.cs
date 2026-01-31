using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;


namespace dotnetapp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set;}
    }
}