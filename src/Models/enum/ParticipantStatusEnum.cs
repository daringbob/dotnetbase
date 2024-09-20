using System.ComponentModel.DataAnnotations;

namespace src.Models.@enum
{
    public enum ParticipantStatusEnum
    {
        [Display(Name = "Chờ xác nhận")]
        Confirming,
        [Display(Name = "Đã xác nhận")]
        Confirmed,
        [Display(Name = "Từ chối")]
        Rejected

    }
}
