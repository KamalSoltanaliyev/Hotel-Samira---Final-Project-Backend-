﻿using System.ComponentModel.DataAnnotations;

namespace hotel_back.ViewModels;

public class RegisterViewModel
{
    [Required]
    public string UserName { get; set; }

    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required, DataType(DataType.Password)]
    public string Password { get; set; }

    [Required, DataType(DataType.Password), Compare(nameof(Password))]
    public string PasswordConfirm { get; set; }
}

