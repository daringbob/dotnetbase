using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace src.Models.@enum
{
    public enum GembaPlanStatusEnum
    {
        [Display(Name = "Lịch đột xuất")]
        New,
        [Display(Name = "Dời đi")]
        Change,
        [Display(Name = "Hủy lịch")]
        Cancel,
        [Display(Name = "Hoàn thành")]
        Complete
    }

}
