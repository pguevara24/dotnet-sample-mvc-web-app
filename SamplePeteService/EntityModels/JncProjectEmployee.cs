using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace SamplePeteService.Models
{
    public partial class JncProjectEmployee
    {
        [Key]
        public string FkProjectID { get; set; }
        [Key]
        public string FkEmployeeID { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime ModifiedDate { get; set; }

        public virtual TblEmployeeInfo FkEmployee { get; set; }
        public virtual TblProject FkProject { get; set; }
    }
}
