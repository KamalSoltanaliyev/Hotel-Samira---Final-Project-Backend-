using System.ComponentModel.DataAnnotations;

namespace hotel_back.ViewModels;

public class ForgotPasswordViewModel
{
    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}
