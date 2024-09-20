using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace src.Models.@enum
{
    public enum StatusEnum
    {
        [Display(Name = "Activated")]
        Activated,
        [Display(Name = "Deactivated")]
        Deactivated,
        [Display(Name = "Deleted")]
        Deleted
    }

}
