﻿using System.ComponentModel.DataAnnotations;

namespace SecureGroup.ViewModel.Models
{
    public class CityMaster
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int StateID { get; set; }
    }
}
