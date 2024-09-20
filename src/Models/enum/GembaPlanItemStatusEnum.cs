using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace src.Models.@enum
{
    public enum GembaPlanItemStatusEnum
    {
        [Display(Name = "New")]
        New,
        [Display(Name = "Change")]
        Change,
        [Display(Name = "Cancel")]
        Cancel,
        [Display(Name = "Complete")]
        Complete,
    }

}
