using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Interpol
{
    /// <summary>
    /// Логика взаимодействия для главного окна (MainWindow.xaml).
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FrameMain.Navigate(new Assets.Pages.CardsPage());
        }

        /// <summary>
        /// Перемещение на страницу назад.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (FrameMain.CanGoBack)
                FrameMain.GoBack();
        }

        /// <summary>
        /// Вход в аккаунт.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLog_Click(object sender, RoutedEventArgs e)
        {
            BtnBack.Visibility = Visibility.Visible;
            while (FrameMain.CanGoBack)
                // Удаление истории перемещения.
                FrameMain.RemoveBackEntry();
            FrameMain.Navigate(new Assets.Pages.LoginPage());
        }

        /// <summary>
        /// Выход из аккаунта, сброс истории перемещения и возврат на главную страницу.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOut_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentUser = null;
            App.MainWindow.BtnOut.Visibility = Visibility.Collapsed;
            App.MainWindow.BtnBack.Visibility = Visibility.Collapsed;
            App.MainWindow.BtnLog.Visibility = Visibility.Visible;
            FrameMain.Navigate(new Assets.Pages.CardsPage());
            while (FrameMain.CanGoBack)
                FrameMain.RemoveBackEntry();
        }
    }
}
