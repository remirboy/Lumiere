using System.Threading.Tasks;
using Lumiere.Models;
using Lumiere.Repositories;
using Lumiere.Services;
using Lumiere.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Lumiere.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<User> _signInManager;
        private readonly EmailService _emailService;

        public AccountController(IUserRepository userRepository, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
            _emailService = new EmailService(configuration["CompanyEmailAuth:Login"], configuration["CompanyEmailAuth:Password"]);
        }

        /// <summary>
        /// Метод для GET запроса авторизации пользователя.
        /// </summary>
        /// <returns>Частичное представление окна авторизации пользователя.</returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Метод для POST запроса авторизации пользователя, авторизует пользователя на сайте, используя логин и пароль.
        /// </summary>
        /// <param name="model">Модель для авторизации пользователя.</param>
        /// <returns>При удачной авторизации происходит редирект на главную страницу, при неудачной - возвращает 
        /// частичное представление окна авторизации с ошибками авторизации.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(nameof(model.Password), "Неправильный логин и (или) пароль");
                }
            }

            return View(model);
        }

        /// <summary>
        /// Метод для GET запроса регистрации пользователя.
        /// </summary>
        /// <returns>Частичное представление окна регистрации пользователя.</returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Метод для POST запроса регистрации пользователя, регистрирует пользователя на сайте, используя 
        /// имя, логин и пароль.
        /// </summary>
        /// <param name="model">Модель для регистрации пользователя.</param>
        /// <returns>
        /// При удачной регистрации происходит редирект на главную страницу, при неудачной - возвращает 
        /// частичное представление окна авторизации с ошибками регистрации.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = model.Email,
                    UserName = model.Email,
                    FirstName = model.Name
                };

                var result = await _userRepository.CreateWithPasswordAsync(user, model.NewPassword);
                if (result.Succeeded)
                {
                    // Генерация токена для пользователя.
                    string token = await _userRepository.GenerateEmailConfirmationTokenAsync(user);

                    // Формирование ссылки для подтверждения регистрации.
                    string callbackUrl = Url.Action
                        (
                            "ConfirmEmail",
                            "Account",
                            new { userId = user.Id, token = token },
                            protocol: HttpContext.Request.Scheme
                        );

                    // Отправка сообщения пользователю на Email для его подтверждения.
                    EmailMessage emailMessage = _emailService.GetEmailConfirmMessage(user.FirstName, user.Email, callbackUrl);
                    await _emailService.SendEmailAsync(emailMessage);

                    // установка куки.
                    await _signInManager.SignInAsync(user, false);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        /// <summary>
        /// Метод для выхода пользователя из аккаунта.
        /// </summary>
        /// <returns>Редирект на главную страницу.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки.
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Метод для подтверждения регистрации новым пользователем. Вызывается, когда пользователь переходит по
        /// сгенерированной ссылке, отправленной на электронную почту.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="token">Сгенерированный токен для данного пользователя.</param>
        /// <returns>При удачном подтверждении редирект на главную страницу, при неудачном - представление с ошибкой.</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return View("Error");

            User user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return View("Error");

            var result = await _userRepository.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
                return View("Error");
        }

        /// <summary>
        /// Метод для GET запроса страницы "Забыли пароль".
        /// </summary>
        /// <returns>Представление "Забыли пароль" для ввода email адреса.</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// Метод для POST запроса "Забыли пароль", отправляет ссылку для сброса пароля на указанный email адрес.
        /// </summary>
        /// <param name="model">Модель представления "Забыли пароль".</param>
        /// <returns>
        /// При удачной валидации модели и пользователя возвращает представление с сообщением о удачной отправке 
        /// письма, при неудачной - возвращает частичное представление "Забыли пароль" с ошибками.
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Пользователь с такой электронной почтой не найден.");
                    return View(model);
                }

                bool isEmailConfirmed = await _userRepository.IsEmailConfirmedAsync(user);
                if (isEmailConfirmed == false)
                {
                    ModelState.AddModelError(string.Empty, "Данная электронная почта не подтверждена пользователем.");
                    return View(model);
                }

                var token = await _userRepository.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, token = token }, protocol: HttpContext.Request.Scheme);

                EmailMessage emailMessage = _emailService.GetResetPasswordMessage(user.FirstName, model.Email, callbackUrl);
                await _emailService.SendEmailAsync(emailMessage);

                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        /// <summary>
        /// Метод для GET запроса сброса пароля.
        /// </summary>
        /// <param name="token">Токен, сгенерированный и отправленный пользователю на указанную электронную почту.</param>
        /// <returns>Если токен корректен, то возвращает представление сброса пароля, иначе представление с ошибками.</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token)
        {
            return string.IsNullOrEmpty(token) ? View("Error") : View();
        }

        /// <summary>
        /// Метод для POST запроса сброса пароля пользователя, устанавливает новый пароль для данного пользователя.
        /// </summary>
        /// <param name="model">Модель представления сброса пароля.</param>
        /// <returns>
        /// При удачном сбросе пароля возвращает представление с сообщением об удачном сбросе пароля, при неудачном -
        /// представление сброса пароля с выявленными ошибками.
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userRepository.GetByNameAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Пользователь с такой электронной почтой не найден.");
                return View(model);
            }

            var result = await _userRepository.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
                return View("ResetPasswordConfirmation");

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }
    }
}