﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureGroup.ViewModel
{
    public class UserViewModel
    {
        [Key]
        public int UserId { get; set; } 
        public string Name { get; set; }    
        public string Email { get; set; }
        public string Password { get; set; }
        public Boolean IsActive { get; set; }
        
         
    }
}
