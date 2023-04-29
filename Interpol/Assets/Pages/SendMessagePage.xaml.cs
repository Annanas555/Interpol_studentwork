using Microsoft.Win32;
using Org.BouncyCastle.Crypto;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ImageMagick;
using System.Security.Cryptography;

namespace Interpol.Assets.Pages
{
    /// <summary>
    /// Логика взаимодействия для страницы отправки сообщений (SendMessagePage.xaml).
    /// </summary>
    public partial class SendMessagePage : Page
    {
        private byte[] _mainImageData;
        public SendMessagePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Отправка обращения.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (TBoxMessage.Text.Length <= 1000)
            {
                var message = new Entities.Message
                {
                    Text = TBoxMessage.Text,
                    Image = _mainImageData,
                    Data = DateTime.Now
                };
                App.Context.Messages.Add(message);
                App.Context.SaveChanges();
                MessageBox.Show("Ваше обращение отправлено", "Успех", MessageBoxButton.OK);
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Текст должен быть не более 1000 символов", "Ошибка", MessageBoxButton.OK);
            }
            
        }

        /// <summary>
        /// Прикрепление фотографии к обращению.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAttach_Click(object sender, RoutedEventArgs e)
        {
            ImageCompressor();
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
                    MessageBox.Show("К обращению прикреплено фото", "Успех", MessageBoxButton.OK);
                }

                // Вывод имени прикрепленного файла в TextBlock.
                string fileName = ofd.FileName;
                TBlockFileName.Text = fileName;
            }
            else
                MessageBox.Show("Фото не было добавлено", "Внимание", MessageBoxButton.OK);
        }
    }
}
