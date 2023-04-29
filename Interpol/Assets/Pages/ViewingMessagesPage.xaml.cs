using System;
using System.Collections.Generic;
using System.Data;
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
using Interpol.Entities;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;

namespace Interpol.Assets.Pages
{
    /// <summary>
    /// Логика взаимодействия для страницы просмотра сообщений (ViewingMessagesPage.xaml).
    /// </summary>
    public partial class ViewingMessagesPage : Page
    {
        public ViewingMessagesPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Удаление сообщения из базы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelMes_Click(object sender, RoutedEventArgs e)
        {
            var currentMessage = (sender as Button).DataContext as Entities.Message;

            if (MessageBox.Show($"Вы уверены, что хотите удалить сообщение под номером {currentMessage.Id_Message}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                App.Context.Messages.Remove(currentMessage);
                App.Context.SaveChanges();
                MessageBox.Show("Запись удалена", "Успех", MessageBoxButton.OK);
                UpdateMessages();
            }
            else
            {
                MessageBox.Show($"Сообщение под номером {currentMessage.Id_Message} не было удалено", "Ошибка", MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Создание PDF-файла из выбранного сообщения.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPDF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var message = (sender as Button).DataContext as Entities.Message;

                // Установка параметров сохранения.
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveFileDialog.Title = "Save PDF File";
                saveFileDialog.FileName = $"Message {message.Id_Message}.pdf";

                if (saveFileDialog.ShowDialog() == true)
                {
                    // Создание нового документа, установка шрифта.
                    Document document = new Document(PageSize.A4, 50, 50, 50, 50);
                    string fontPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
                    // Инициализация средства записи PDF документа.
                    PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));
                    document.Open();

                    // Параметры шрифтов.
                    BaseFont headFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    Font headerFont = new Font(headFont, 16f, Font.BOLD);

                    BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    Font font = new Font(baseFont, 14f, Font.NORMAL);

                    // Создание текста и фото.
                    iTextSharp.text.Paragraph header = new iTextSharp.text.Paragraph($"Сообщение {message.Id_Message}", headerFont);
                    document.Add(header);

                    iTextSharp.text.Paragraph date = new iTextSharp.text.Paragraph(message.Data.ToString(), font);
                    date.PaddingTop = 30f;
                    document.Add(date);

                    iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph(message.Text, font);
                    paragraph.PaddingTop = 20f;
                    document.Add(paragraph);

                    if (message.Image != null)
                    {
                        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(message.Image);
                        // Подгонка изображения под размеры документа.
                        image.ScaleToFit(document.PageSize.Width - document.LeftMargin - document.RightMargin, document.PageSize.Height - document.TopMargin - document.BottomMargin);
                        image.PaddingTop = 40f;
                        document.Add(image);
                    }

                    document.Close();

                    MessageBox.Show("PDF-документ создан успешно", "Успех", MessageBoxButton.OK);
                }
            }
            catch
            {
                MessageBox.Show("Документ не был создан", "Ошибка", MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Выборка всех сообщений из базы данных.
        /// </summary>
        private void UpdateMessages()
        {
            LViewMessages.ItemsSource = App.Context.Messages.ToList();
        }

        /// <summary>
        /// Загрузка страницы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateMessages();
        }
    }
}
