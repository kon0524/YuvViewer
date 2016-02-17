using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using YuvViewer.Model;

namespace YuvViewer.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties
        private uint inputWidth;
        public uint InputWidth
        {
            get { return inputWidth; }
            set
            {
                inputWidth = value;
                NotifyPropertyChanged("InputWidth");
            }
        }

        private uint inputHeight;
        public uint InputHeight
        {
            get { return inputHeight; }
            set
            {
                inputHeight = value;
                NotifyPropertyChanged("InputHeight");
            }
        }

        private WriteableBitmap inputImage;
        public WriteableBitmap InputImage
        {
            get { return inputImage; }
            set
            {
                inputImage = value;
                NotifyPropertyChanged("InputImage");
            }
        }
        #endregion
        
        public MainViewModel()
        {
            InputWidth = 5376;
            InputHeight = 2688;
        }

        public void DropYuvImage(string path)
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();

            WriteableBitmap wbmp = Yuv422.GetImage(path, (int)InputWidth, (int)InputHeight);

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds + "ms");

            InputImage = wbmp;
        }
    }
}
