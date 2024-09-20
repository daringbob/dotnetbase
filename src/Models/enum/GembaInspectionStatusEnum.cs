using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace src.Models.@enum
{
    public enum GembaInspectionStatusEnum
    {
        [Display(Name = "Complete")]
        Complete,
        [Display(Name = "Saved")]
        Saved,
        [Display(Name = "Deleted")]
        Deleted
    }

}
