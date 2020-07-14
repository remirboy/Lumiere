using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lumiere.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Не указано имя")]
        [StringLength(50, ErrorMessage = "Длина имени должна быть меньше 50 символов")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Не указана фамлия")]
        [StringLength(50, ErrorMessage = "Длина фамилии должна быть меньше 50 символов")]
        public string SecondName { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Не указан Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Длина пароля должна быть больше 5 и меньше 100 символов")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
