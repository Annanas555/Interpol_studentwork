using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

namespace Interpol.Assets.Pages
{
    /// <summary>
    /// Логика взаимодействия для страницы авторизации (LoginPage.xaml).
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Регистрация нового пользователя.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterPage());
        }

        /// <summary>
        /// Вход в аккаунт.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            string pass = PBoxPassword.Password;
            byte[] checkSum = md5.ComputeHash(Encoding.UTF8.GetBytes(pass));

            var currentUser = App.Context.Users.FirstOrDefault(p => p.Login == TBoxLogin.Text && p.Password == checkSum);

            if (currentUser != null)
            {
                App.CurrentUser = currentUser;
                App.MainWindow.BtnLog.Visibility = Visibility.Collapsed;
                App.MainWindow.BtnOut.Visibility = Visibility.Visible;
                NavigationService.Navigate(new CardsPage());
            }
            else
            {
                MessageBox.Show("Пользователь с такими данными не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
