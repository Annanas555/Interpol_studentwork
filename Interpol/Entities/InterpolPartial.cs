using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Interpol.Entities
{
    public partial class Lica_v_baze
    {
        /// <summary>
        /// Настройка видимости кнопок администратора.
        /// </summary>
        public string AdminControlsVisibility
        {
            get
            {
                // 1 - администратор, 2 - пользователь.
                if (App.CurrentUser != null && App.CurrentUser.Id_role == 1)
                {
                    return "Visible";
                }
                else
                {
                    return "Collapsed";
                }
            }
        }

        /// <summary>
        /// Настройка видимости кнопок пользователя.
        /// </summary>
        public string UserControlsVisibility
        {
            get
            {
                if (App.CurrentUser != null && App.CurrentUser.Id_role == 2)
                {
                    return "Visible";
                }
                else
                {
                    return "Collapsed";
                }
            }
        }

        /// <summary>
        /// Вставка картинки-заглушки, когда в карточке нет фотографии.
        /// </summary>
        public ImageSource GetImage
        {
            get
            {
                if (ImageData != null)
                {
                    return (ImageSource)new ImageSourceConverter().ConvertFrom(ImageData);
                }
                else
                {
                    return new BitmapImage(new Uri(@"pack://application:,,,/Assets/Avatar.png"));
                }
            }
        }

    }
}
