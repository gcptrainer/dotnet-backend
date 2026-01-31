using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace dotnetapp.Models
{
    public class UserRoles
    {
        public string Baker {get; set;}

        public string Customer {get; set;}
        
    }
}