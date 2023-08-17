using System;

namespace SecureGroup.Models
{
    public class Project
    {

        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectDueDate { get; set; }
        public decimal ProjectBudget { get; set; }
        public int ProjectHeadId { get; set; }
        public string Address { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string Block { get; set; }
        public string ZipCode { get; set; }
        public int SchemeId { get; set; }
        public int CountryId { get; set; }
        public int StatusId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public bool IsActive { get; set; }

    }
}
