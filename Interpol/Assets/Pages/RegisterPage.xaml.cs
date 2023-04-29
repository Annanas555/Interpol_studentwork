using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Interpol.Entities;
using InterpolLibrary;

namespace Interpol.Assets.Pages
{
    /// <summary>
    /// Логика взаимодействия для страницы регистрации (RegisterPage.xaml).
    /// </summary>
    public partial class RegisterPage : Page
    {
        /// <summary>
        /// Поле для хранения введенного пароля.
        /// </summary>
        private PasswordChecker checker;
        public RegisterPage()
        {
            InitializeComponent();
            checker = new PasswordChecker();
        }

        /// <summary>
        /// Регистрация нового пользователя.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            // Хотя бы 1: заглавная, строчная буква, знак, цифра, длина не менее 8 символов.
            Regex reg = new Regex(@"^(?=.*[\p{Ll}\p{Lu}])(?=.*\d)(?=.*[^\p{L}\d])[\p{L}\d\S]{1,10}$");
            // Создание списка ошибок, которые может допустить пользователь.
            List<string> errors = new List<string>();
            // Переменная для выборки существующего логина в базе данных, при попытке создать аккаунт с таким же именем.
            var existingUser = App.Context.Users.FirstOrDefault(u => u.Login == TBoxLogin.Text);

            if (TBoxLogin.Text == "")
            {
                errors.Add("Введите логин.");
            }

            if (TBoxLogin.Text.Length < 4)
            {
                errors.Add("Логин должен быть не менее 4 символов в длину.");
            }

            if (existingUser != null)
            {
                errors.Add("Пользователь с таким именем уже существует.");
            }

            if (PBoxPassword.Password == "")
            {
                errors.Add("Введите пароль.");
            }

            if (PBoxConfirmPassword.Password == "")
            {
                errors.Add("Подтвердите пароль.");
            }

            if (PBoxPassword.Password.Length < 8 || PBoxConfirmPassword.Password.Length < 8)
            {
                errors.Add("Пароль должен быть не менее 8 символов в длину.");
            }

            if (PBoxPassword.Password.Length > 20 || PBoxConfirmPassword.Password.Length > 20)
            {
                errors.Add("Пароль должен быть не более 20 символов в длину.");
            }

            if (!reg.IsMatch(PBoxPassword.Password) || !reg.IsMatch(PBoxConfirmPassword.Password))
            {
                errors.Add("Пароль должен содержать хотя бы одну заглавную букву, одну строчную букву, одну цифру и один символ, не являющийся буквой или цифрой.");
            }

            if (PBoxPassword.Password != PBoxConfirmPassword.Password)
            {
                errors.Add("Пароли не совпадают.");
            }

            // Проверка на наличие ошибок во время попытки регистрации.
            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errors), "Ошибка");
            }
            else
            {
                // Шифрование введенного пароля.
                MD5 md5 = new MD5CryptoServiceProvider();
                string pass = PBoxPassword.Password;
                byte[] checkSum = md5.ComputeHash(Encoding.UTF8.GetBytes(pass));
                // Создание записи пользователя в базе данных.
                var newUser = new Entities.User
                {
                    Login = TBoxLogin.Text,
                    Password = checkSum,
                    Id_role = 2
                };
                App.Context.Users.Add(newUser);
                App.Context.SaveChanges();
                MessageBox.Show("Ваш аккаунт создан", "Успешно");
                NavigationService.GoBack();
            }
        }

        /// <summary>
        /// Вывод уровня сложности пароля при изменении текста внутри PasswordBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PBoxPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Проверка сложности пароля.
            SecureString securePassword = ((PasswordBox)sender).SecurePassword;
            string password = new System.Net.NetworkCredential(string.Empty, securePassword).Password;
            PasswordStrength strength = checker.CheckPassword(password);

            switch (strength)
            {
                case PasswordStrength.NotSecure:
                    TBlockPasswordStrength.Text = "Пароль не надежный";
                    break;

                case PasswordStrength.Weak:
                    TBlockPasswordStrength.Text = "Пароль слабый";
                    break;

                case PasswordStrength.Good:
                    TBlockPasswordStrength.Text = "Пароль хороший";
                    break;

                case PasswordStrength.Strong:
                    TBlockPasswordStrength.Text = "Пароль сильный";
                    break;
            }
        }

        /// <summary>
        /// Метод для показа паролей.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBoxSeePassword_Click(object sender, RoutedEventArgs e)
        {
            if (CBoxSeePassword.IsChecked == true)
            {
                // Показать TextBlock, содержащие пароли.
                TBPassword.Text = PBoxPassword.Password;
                TBConfirmPassword.Text = PBoxConfirmPassword.Password;
                TBPassword.Visibility = Visibility.Visible;
                TBConfirmPassword.Visibility = Visibility.Visible;
            }
            else
            {
                // Скрыть TextBlock.
                TBPassword.Visibility = Visibility.Hidden;
                TBConfirmPassword.Visibility = Visibility.Hidden;
            }
        }
    }
}
