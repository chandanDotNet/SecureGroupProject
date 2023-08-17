using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureGroup.ViewModel
{
    public class ProjectViewModel
    {

        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string Dates { get; set; }
        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectDueDate { get; set; }
        public decimal ProjectBudget { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a Project Head")]
        public int ProjectHeadId { get; set; }
        public string ProjectHeadName { get; set; }
        public string? Address { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string? Block { get; set; }
        public string? ZipCode { get; set; }
        public int SchemeId { get; set; }
        public string SchemeName { get; set; }
        public int CountryId { get; set; }
        public int CreatedBy { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Must Choose a Status")]
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string SpentTime { get; set; }
        public string LastStatusUpdate { get; set; }
        public int NoOfTask { get; set; }
        public string TaskName { get; set; }
        public int TaskId { get; set; }

        public IList<SelectListItem> CityList { get; set; }
        public IList<SelectListItem> StateList { get; set; }
        public IList<SelectListItem> SchemeList { get; set; }
        public IList<SelectListItem> UserList { get; set; }
        public IList<SelectListItem> StatusList { get; set; }


    }
}
