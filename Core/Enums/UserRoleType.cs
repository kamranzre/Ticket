using System.ComponentModel.DataAnnotations;


namespace Core.Enums
{
    public enum UserRoleType
    {
        [Display(Name = "کاربر")]
        User = 1,

        [Display(Name = "کارشناس")]
        Expert = 2
    }

}
