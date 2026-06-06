using System.ComponentModel.DataAnnotations;

public class LoginViewModel
{
    [Required(ErrorMessage = "لطفاً نام کاربری را وارد کنید.")]
    [Display(Name = "نام کاربری")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "لطفاً رمز عبور را وارد کنید.")]
    [DataType(DataType.Password)]
    [Display(Name = "رمز عبور")]
    public string Password { get; set; }

    [Display(Name = "مرا بخاطر بسپار")]
    public bool RememberMe { get; set; }
}