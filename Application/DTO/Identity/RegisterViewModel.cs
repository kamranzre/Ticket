using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required(ErrorMessage = "وارد کردن نام کاربری الزامی است.")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "نام کاربری باید فقط شامل حروف انگلیسی، عدد یا _ باشد.")]
    [Display(Name = "نام کاربری")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "وارد کردن ایمیل الزامی است.")]
    [EmailAddress(ErrorMessage = "فرمت ایمیل صحیح نیست.")]
    [RegularExpression(@"^[a-zA-Z0-9@._-]+$", ErrorMessage = "ایمیل باید فقط با حروف انگلیسی نوشته شود.")]
    [Display(Name = "ایمیل")]
    public string Email { get; set; }

    [Required(ErrorMessage = "وارد کردن رمز عبور الزامی است.")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",ErrorMessage = "رمز عبور باید حداقل ۸ کاراکتر بوده و شامل حداقل یک حرف بزرگ، یک حرف کوچک و یک عدد باشد.")]
    [MinLength(6, ErrorMessage = "رمز عبور باید حداقل 6 کاراکتر باشد.")]
    [Display(Name = "رمز عبور")]
    public string Password { get; set; }

    [Required(ErrorMessage = "تکرار رمز عبور الزامی است.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "رمز عبور و تکرار آن یکسان نیستند.")]
    [Display(Name = "تکرار رمز عبور")]
    public string ConfirmPassword { get; set; }
}
