using Interpol.Entities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ImageMagick;
using System.Text.RegularExpressions;

namespace Interpol.Assets.Pages
{
    /// <summary>
    /// Логика взаимодействия для страницы просмотра и редактирования записей (AddEditCardsPage.xaml).
    /// </summary>
    public partial class AddEditCardsPage : Page
    {
        private Entities.Lica_v_baze _currentPerson = null;
        private byte[] _mainImageData;
        List<string> errors = new List<string>();

        /// <summary>
        /// Словарь для хранения значений статуса разыскиваемых.
        /// </summary>
        Dictionary<string, int> statusDict = new Dictionary<string, int>
        {
            { "Преступник", 1 },
            { "Отбывший", 2 },
            { "Подозреваемый", 3 },
            { "Пропавший", 4 }
        };
        /// <summary>
        /// Словарь для хранения значений пола.
        /// </summary>
        Dictionary<string, string> genderDict = new Dictionary<string, string>
        {
            { "Женский","Ж" },
            { "Мужской", "М" }
        };

        public AddEditCardsPage()
        {
            InitializeComponent();
            NationalityComboBox.ItemsSource = App.Context.Nations.Select(c => c.Nazion).ToList();
        }
        /// <summary>
        /// Просмотр или изменение записи.
        /// </summary>
        /// <param name="person">Выбранная запись</param>
        public AddEditCardsPage(Entities.Lica_v_baze person)
        {
            _currentPerson = person;
            InitializeComponent();
            Title = $"{_currentPerson.F} {_currentPerson.I} {_currentPerson.O}";

            // Запрет на изменение данных для обычного пользователя.
            if (App.CurrentUser.Id_role == 2)
            {
                LastNameTextBox.IsReadOnly = true;
                FirstNameTextBox.IsReadOnly = true;
                PatronymicTextBox.IsReadOnly = true;
                BirthdateDatePicker.IsEnabled = false;
                EyeColorTextBox.IsReadOnly = true;
                HairColorTextBox.IsReadOnly = true;
                BirthplaceTextBox.IsReadOnly = true;
                HeightTextBox.IsReadOnly = true;
                WeightTextBox.IsReadOnly = true;
                GenderComboBox.IsEnabled = false;
                NationalityComboBox.IsEnabled = false;
                StatusComboBox.IsEnabled = false;
                BtnAddPhoto.Visibility = Visibility.Collapsed;
                BtnSave.Visibility = Visibility.Collapsed;
            }

            int statusNum = (int)_currentPerson.Status;
            string selectedStatus = statusDict.FirstOrDefault(k => k.Value == statusNum).Key;
            string genderCode = _currentPerson.Pol;
            string selectedGender = genderDict.FirstOrDefault(g => g.Value == genderCode).Key;
            NationalityComboBox.ItemsSource = App.Context.Nations.Select(c => c.Nazion).ToList();
            string selectedNation = _currentPerson.Nazion;
            var nation = App.Context.Nations.FirstOrDefault(c => c.Kod == selectedNation);

            LastNameTextBox.Text = _currentPerson.F;
            FirstNameTextBox.Text = _currentPerson.I;
            PatronymicTextBox.Text = _currentPerson.O;
            EyeColorTextBox.Text = _currentPerson.Glaza;
            HairColorTextBox.Text = _currentPerson.Volosy;
            BirthdateDatePicker.Text = _currentPerson.DR.ToString();
            HeightTextBox.Text = _currentPerson.Rost.ToString();
            WeightTextBox.Text = _currentPerson.Rost.ToString();
            BirthplaceTextBox.Text = _currentPerson.Mest_rozhd;
            GenderComboBox.Text = selectedGender;
            StatusComboBox.Text = selectedStatus;
            NationalityComboBox.SelectedItem = nation.Nazion;
            if (_currentPerson.ImageData != null)
                PhotoImage.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(_currentPerson.ImageData);
        }

        /// <summary>
        /// Подсчет ошибок во время работы с записью.
        /// </summary>
        public void AddEditErrors()
        {
            if (!string.IsNullOrEmpty(LastNameTextBox.Text) && !Regex.IsMatch(LastNameTextBox.Text, @"^[\p{L}]+$"))
            {
                errors.Add("Неверно заполнено поле 'Фамилия'.");
            }
            if (!string.IsNullOrEmpty(FirstNameTextBox.Text) && !Regex.IsMatch(FirstNameTextBox.Text, @"^[\p{L}]+$"))
            {
                errors.Add("Неверно заполнено поле 'Имя'.");
            }
            if (!string.IsNullOrEmpty(PatronymicTextBox.Text) && !Regex.IsMatch(PatronymicTextBox.Text, @"^[\p{L}]+$"))
            {
                errors.Add("Неверно заполнено поле 'Отчество'.");
            }
            if (!string.IsNullOrEmpty(EyeColorTextBox.Text) && !Regex.IsMatch(EyeColorTextBox.Text, @"^[\p{L}]+$"))
            {
                errors.Add("Неверно заполнено поле 'Цвет глаз'.");
            }
            if (!string.IsNullOrEmpty(HairColorTextBox.Text) && !Regex.IsMatch(HairColorTextBox.Text, @"^[\p{L}]+$"))
            {
                errors.Add("Неверно заполнено поле 'Цвет волос'.");
            }
            int height;
            if (!string.IsNullOrEmpty(HeightTextBox.Text) && !int.TryParse(HeightTextBox.Text, out height))
            {
                errors.Add("Неверно заполнено поле 'Рост'.");
            }
            int weight;
            if (!string.IsNullOrEmpty(WeightTextBox.Text) && !int.TryParse(WeightTextBox.Text, out weight))
            {
                errors.Add("Неверно заполнено поле 'Вес'.");
            }
            if (GenderComboBox.SelectedItem == null)
            {
                errors.Add("Укажите пол.");
            }
            if (NationalityComboBox.SelectedItem == null)
            {
                errors.Add("Укажите национальность или выберите 'Неизвестно'.");
            }
            if (StatusComboBox.SelectedItem == null)
            {
                errors.Add("Укажите статус.");
            }
        }

        /// <summary>
        /// Добавление фотографии.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddPhoto_Click(object sender, RoutedEventArgs e)
        {
            ImageCompressor();
        }

        /// <summary>
        /// Сохранение записи.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            AddEditErrors();
            if (errors.Count() > 0)
            {
                MessageBox.Show(string.Join("\n", errors), "Ошибка");
                errors.Clear();
            }
            else
            {
                // Создание новой записи.
                if (_currentPerson == null)
                {
                    int statusNumber;
                    string selectedStatus = StatusComboBox.Text;
                    string genderCode;
                    string selectedGender = GenderComboBox.Text;
                    // Взятие нужных данных из словарей.
                    if (statusDict.TryGetValue(selectedStatus, out statusNumber) && genderDict.TryGetValue(selectedGender, out genderCode))
                    {
                        // Значение национальности из базы.
                        var nation = App.Context.Nations.Where(c => c.Nazion == NationalityComboBox.Text).Select(c => c.Kod).FirstOrDefault();
                        var person = new Entities.Lica_v_baze
                        {
                            // Возможность сохранить запись, где практически каждое поле может остаться пустым.
                            F = string.IsNullOrEmpty(LastNameTextBox.Text) ? null : LastNameTextBox.Text,
                            I = string.IsNullOrEmpty(FirstNameTextBox.Text) ? null : FirstNameTextBox.Text,
                            O = string.IsNullOrEmpty(PatronymicTextBox.Text) ? null : PatronymicTextBox.Text,
                            Glaza = string.IsNullOrEmpty(EyeColorTextBox.Text) ? null : EyeColorTextBox.Text,
                            Volosy = string.IsNullOrEmpty(HairColorTextBox.Text) ? null : HairColorTextBox.Text,
                            Rost = string.IsNullOrEmpty(HeightTextBox.Text) ? null : (int?)int.Parse(HeightTextBox.Text),
                            Ves = string.IsNullOrEmpty(WeightTextBox.Text) ? null : (int?)int.Parse(WeightTextBox.Text),
                            Mest_rozhd = string.IsNullOrEmpty(BirthplaceTextBox.Text) ? null : BirthplaceTextBox.Text,
                            DR = BirthdateDatePicker.SelectedDate.HasValue ? BirthdateDatePicker.SelectedDate : (DateTime?)null,
                            Pol = genderCode,
                            Status = statusNumber,
                            Nazion = nation,
                            ImageData = _mainImageData
                        };

                        App.Context.Lica_v_baze.Add(person);
                        App.Context.SaveChanges();
                        MessageBox.Show("Запись добавлена", "Успех");
                        NavigationService.GoBack();
                    }
                }
                // Редактирование существующей записи.
                else
                {
                    int statusNum = (int)_currentPerson.Status;
                    string genderCode = _currentPerson.Pol;
                    var nation = App.Context.Nations.Where(c => c.Nazion == NationalityComboBox.Text).Select(c => c.Kod).FirstOrDefault();

                    _currentPerson.F = string.IsNullOrEmpty(LastNameTextBox.Text) ? null : LastNameTextBox.Text;
                    _currentPerson.I = string.IsNullOrEmpty(FirstNameTextBox.Text) ? null : FirstNameTextBox.Text;
                    _currentPerson.O = string.IsNullOrEmpty(PatronymicTextBox.Text) ? null : PatronymicTextBox.Text;
                    _currentPerson.Glaza = string.IsNullOrEmpty(EyeColorTextBox.Text) ? null : EyeColorTextBox.Text;
                    _currentPerson.Volosy = string.IsNullOrEmpty(HairColorTextBox.Text) ? null : HairColorTextBox.Text;
                    _currentPerson.Rost = string.IsNullOrEmpty(HeightTextBox.Text) ? null : (int?)int.Parse(HeightTextBox.Text);
                    _currentPerson.Ves = string.IsNullOrEmpty(WeightTextBox.Text) ? null : (int?)int.Parse(WeightTextBox.Text);
                    _currentPerson.Mest_rozhd = string.IsNullOrEmpty(BirthplaceTextBox.Text) ? null : BirthplaceTextBox.Text;
                    _currentPerson.DR = BirthdateDatePicker.SelectedDate.HasValue ? BirthdateDatePicker.SelectedDate : (DateTime?)null;
                    _currentPerson.Pol = genderCode;
                    _currentPerson.Status = statusNum;
                    _currentPerson.Nazion = nation;
                    if (_mainImageData != null)
                        _currentPerson.ImageData = _mainImageData;

                    App.Context.SaveChanges();
                    MessageBox.Show("Запись отредактирована", "Успех");
                    NavigationService.GoBack();
                }
            }
        }

        /// <summary>
        /// Прикрепление файла или сжатие, если он имеет слишком большой размер (более 10Мб).
        /// </summary>
        public void ImageCompressor()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image |*.png; *.jpg; *.jpeg";
            if (ofd.ShowDialog() == true)
            {
                _mainImageData = File.ReadAllBytes(ofd.FileName);
                FileInfo fileInfo = new FileInfo(ofd.FileName);
                long fileSize = fileInfo.Length;
                if (fileSize > 10 * 1024 * 1024)
                {
                    MessageBox.Show("Размер файла превышает 10 Мб. Файл будет сжат", "Внимание", MessageBoxButton.OK);
                    using (var image = new MagickImage(_mainImageData))
                    {
                        // Установка нового качества для сжатия любого размера до 10Мб или менее.
                        image.Quality = (int)(10 * 1024 * 1024 / fileSize * 100);
                        image.Write(ofd.FileName);
                    }

                }
                else
                {
                    MessageBox.Show("К записи прикреплено фото", "Успех", MessageBoxButton.OK);
                }
                PhotoImage.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(_mainImageData);
            }
            else
            {
                MessageBox.Show("Фото не было добавлено", "Ошибка", MessageBoxButton.OK);
            }
        }
    }
}
