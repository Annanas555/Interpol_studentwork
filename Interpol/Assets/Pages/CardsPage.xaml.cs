using Interpol.Entities;
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

namespace Interpol.Assets.Pages
{
    /// <summary>
    /// Логика взаимодействия для CardsPage.xaml
    /// </summary>
    public partial class CardsPage : Page
    {
        public CardsPage()
        {
            InitializeComponent();
            ComboGender.SelectedIndex = 0;
            ComboStatus.SelectedIndex = 0;
            TBoxSearch.TextChanged += OnSearchTextChanged;

            // 1 - администратор, 2 - пользователь.
            if (App.CurrentUser == null)
            {
                BtnAddCard.Visibility = Visibility.Collapsed;
                BtnViewingMessages.Visibility = Visibility.Collapsed;
                BtnMessage.Visibility = Visibility.Collapsed;
            }
            else if (App.CurrentUser.Id_role != 1)
            {
                BtnMessage.Visibility = Visibility.Visible;
                BtnAddCard.Visibility = Visibility.Collapsed;
                BtnViewingMessages.Visibility = Visibility.Collapsed;
            }
            else
            {
                BtnMessage.Visibility = Visibility.Collapsed;
                BtnAddCard.Visibility = Visibility.Visible;
                BtnViewingMessages.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Создание записи разыскиваемого.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddCard_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.AddEditCardsPage());
        }

        /// <summary>
        /// Переход к странице отправки обращений.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMessage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.SendMessagePage());
        }

        /// <summary>
        /// Переход к просмотру сообщений.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnViewingMessages_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.ViewingMessagesPage());
        }

        /// <summary>
        /// Редактирование выбранной записи.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var currentPerson = (sender as Button).DataContext as Entities.Lica_v_baze;
            NavigationService.Navigate(new Pages.AddEditCardsPage(currentPerson));
        }

        /// <summary>
        /// Удаление выбранной записи.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var currentCard = (sender as Button).DataContext as Entities.Lica_v_baze;

            if (MessageBox.Show($"Вы уверены, что хотите удалить запись разыскиваемого {currentCard.F}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                App.Context.Lica_v_baze.Remove(currentCard);
                App.Context.SaveChanges();
                MessageBox.Show("Запись удалена", "Успех", MessageBoxButton.OK);
                UpdateCards();
            }
        }

        /// <summary>
        /// Загрузка страницы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateCards();
        }

        /// <summary>
        /// Фильтрация записей. Обновляется каждый раз, когда меняется TBoxSearch.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string query = TBoxSearch.Text.Trim();
            List<Lica_v_baze> filteredCards = new List<Lica_v_baze>(App.Context.Lica_v_baze);

            // Фильтрация записей по запросу.
            if (!string.IsNullOrEmpty(query))
            {
                filteredCards = filteredCards.Where(c => (c.F?.Contains(query) ?? false) || (c.I?.Contains(query) ?? false) || (c.O?.Contains(query) ?? false)).ToList();
            }

            // Применение фильтрации и сортировки.
            string selectedGender = ((ComboBoxItem)ComboGender.SelectedItem).Content.ToString();
            int status = ComboStatus.SelectedIndex;

            if (selectedGender != "Все")
            {
                filteredCards = filteredCards.Where(c => c.Pol == selectedGender).ToList();
            }

            if (status > 0)
            {
                filteredCards = filteredCards.Where(c => c.Status == status).ToList();
            }

            LViewCards.ItemsSource = filteredCards;
        }

        /// <summary>
        /// Выборка записей из базы данных, согласно фильтрации (по умолчанию - все записи).
        /// </summary>
        private void UpdateCards()
        {
            // Применение фильтрации и сортировки.
            string selectedGender = ((ComboBoxItem)ComboGender.SelectedItem).Content.ToString();
            int status = ComboStatus.SelectedIndex;

            List<Lica_v_baze> filteredCards = new List<Lica_v_baze>(App.Context.Lica_v_baze);

            if (selectedGender != "Все")
            {
                filteredCards = filteredCards.Where(c => c.Pol == selectedGender).ToList();
            }

            if (status > 0)
            {
                filteredCards = filteredCards.Where(c => c.Status == status).ToList();
            }

            // Применение фильтрации по запросу.
            string query = TBoxSearch.Text.Trim();
            if (!string.IsNullOrEmpty(query))
            {
                filteredCards = filteredCards.Where(c => (c.F?.Contains(query) ?? false) || (c.I?.Contains(query) ?? false) || (c.O?.Contains(query) ?? false)).ToList();
            }

            LViewCards.ItemsSource = filteredCards;
        }

        /// <summary>
        /// Фильтрация записей по полу.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCards();
        }

        /// <summary>
        /// Фильтрация записей по статусу.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCards();
        }

        /// <summary>
        /// Просмотр подробной информации из выбранной записи.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMore_Click(object sender, RoutedEventArgs e)
        {
            var currentPerson = (sender as Button).DataContext as Entities.Lica_v_baze;
            NavigationService.Navigate(new Pages.AddEditCardsPage(currentPerson));
        }

    }
}
