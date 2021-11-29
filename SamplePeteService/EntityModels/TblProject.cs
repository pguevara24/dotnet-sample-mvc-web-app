using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace SamplePeteService.Models
{
    public partial class TblProject
    {
        public TblProject()
        {
            JncProjectEmployees = new HashSet<JncProjectEmployee>();
        }

        [Key]
        public string ProjectID { get; set; }
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        public virtual ICollection<JncProjectEmployee> JncProjectEmployees { get; set; }
    }
}
