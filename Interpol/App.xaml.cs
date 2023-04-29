using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SqlClient;

namespace Interpol
{
    /// <summary>
    /// Логика взаимодействия для сборки (App.xaml).
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Создание контекста.
        /// </summary>
        public static Entities.InterpolEntities Context
        { get; } = new Entities.InterpolEntities();

        /// <summary>
        /// Поле для хранения данных авторизированного пользователя (по умолчанию пользователь считается незарегистрированным).
        /// </summary>
        public static Entities.User CurrentUser = null;

        /// <summary>
        /// Отлавливание необработанных ошибок.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Возникла ошибка: " + e.Exception.Message, "Ошибка", MessageBoxButton.OK);
            e.Handled = true;
        }

        /// <summary>
        /// Проверка подключения к базе.
        /// </summary>
        /// <returns></returns>
        private bool IsDataBaseConnected()
        {
            return Context.Database.Exists();
        }

        /// <summary>
        /// Переопределение класса MainWindow.
        /// </summary>
        public static MainWindow MainWindow
        {
            get; set;
        }

        /// <summary>
        /// Запуск приложения.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (IsDataBaseConnected())
            {
                MainWindow = new MainWindow();
                MainWindow.Show();
            }
            else
            {
                if(MessageBox.Show("Не удалось подключиться к базе данных", "Ошибка", MessageBoxButton.OK) == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }
            }
            
        }
    }
}
