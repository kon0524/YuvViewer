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

namespace YuvViewer.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
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

        public MainViewModel()
        {
            InputWidth = 5376;
            InputHeight = 2688;
        }

        public void DropYuvImage(string path)
        {
            Stopwatch sw = new Stopwatch();

            // Y,Cb,Crのbyte列を取得
            byte[] lum, cb, cr;
            int ret;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                // byte列の確保
                lum = new byte[fs.Length / 2];
                cb = new byte[lum.Length / 2];
                cr = new byte[lum.Length / 2];

                // 読込み
                ret = fs.Read(lum, 0, lum.Length);
                ret = fs.Read(cr, 0, cr.Length);
                ret = fs.Read(cb, 0, cb.Length);
            }

            double temp;
            WriteableBitmap wbmp = new WriteableBitmap(
                (int)InputWidth, (int)InputHeight, 96, 96, PixelFormats.Bgr32, null);

            sw.Start();
            wbmp.Lock();

            byte[] buf = new byte[wbmp.PixelWidth * wbmp.PixelHeight * 4];
            for (int i = 0; i < buf.Length; i += 4)
            {
                int index = i / 4;
                // B
                temp = 1.164 * (lum[index] - 16) + 2.018 * (cr[index / 2] - 128);
                if (temp > 255) temp = 255;
                if (temp < 0) temp = 0;
                buf[i] = (byte)temp;

                // G
                temp = 1.164 * (lum[index] - 16) - 0.391 * (cr[index / 2] - 128) - 0.813 * (cb[index / 2] - 128);
                if (temp > 255) temp = 255;
                if (temp < 0) temp = 0;
                buf[i+1] = (byte)temp;
                
                // R
                temp = 1.164 * (lum[index] - 16) + 1.596 * (cb[index / 2] - 128);
                if (temp > 255) temp = 255;
                if (temp < 0) temp = 0;
                buf[i+2] = (byte)temp;
            }

            Marshal.Copy(buf, 0, wbmp.BackBuffer, buf.Length);

            wbmp.AddDirtyRect(new Int32Rect(0, 0, wbmp.PixelWidth, wbmp.PixelHeight));
            wbmp.Unlock();

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds + "ms");

            InputImage = wbmp;
        }
    }
}
