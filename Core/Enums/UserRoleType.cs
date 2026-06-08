using System.ComponentModel.DataAnnotations;


namespace Core.Enums
{
    public enum UserRoleType
    {
        [Display(Name = "کاربر")]
        User = 1,

        [Display(Name = "کارشناس")]
        Support = 2
    }

}
