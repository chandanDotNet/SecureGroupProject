﻿using System.ComponentModel.DataAnnotations;

namespace SecureGroup.ViewModel.Models
{
    public class StateMaster
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int CountryID { get; set; }
    }
}
