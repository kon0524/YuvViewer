using System.Windows;
using YuvViewer.ViewModel;

namespace YuvViewer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel mainVM;

        public MainWindow()
        {
            InitializeComponent();
            mainVM = new MainViewModel();
            this.DataContext = mainVM;
        }

        private void Window_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            string[] dropFiles = e.Data.GetData(System.Windows.DataFormats.FileDrop) as string[];
            if (dropFiles == null) return;

            mainVM.DropYuvImage(dropFiles[0]);
        }
    }
}
