﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecureGroup.ViewModel.Models
{
    public class Attendance
    {
        
        [Key]
        public int AttendanceId { get; set; }
        public int UserId { get; set; }
       
        public DateTime Date { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }
        public DateTime DurationTime { get; set; }
        public string Reason { get; set; }
        public int AttendanceStatusId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public Boolean IsActive { get; set; }

    }
}
