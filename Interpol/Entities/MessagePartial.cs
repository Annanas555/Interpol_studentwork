using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Interpol.Entities
{
    public partial class Message
    {
        /// <summary>
        /// Установка картинки-заглушки, если в сообщении нет фотографии.
        /// </summary>
        public ImageSource GetImage
        {
            get
            {
                if (Image != null)
                {
                    return (ImageSource)new ImageSourceConverter().ConvertFrom(Image);
                }
                else
                {
                    return new BitmapImage(new Uri(@"pack://application:,,,/Assets/empty_photo.png"));
                }
            }
        }
    }
}
