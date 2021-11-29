using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace SamplePeteService.Models
{
    public partial class TblEmployeeInfo
    {
        public TblEmployeeInfo()
        {
            JncProjectEmployees = new HashSet<JncProjectEmployee>();
        }

        [Key]
        public string EmployeeID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PositionTitle { get; set; }
        [Required]
        public DateTime DateHired { get; set; }

        public virtual ICollection<JncProjectEmployee> JncProjectEmployees { get; set; }
    }
}
