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

namespace FotoFrameViewVM
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var photoFrame = this.Resources["PhotoFrame"]
                as PhotoFrameViewModel;

            var isValid2 = (bool)photoFrame?.GetPhotoFrameTemplate.IsValid 
                ? "Верны" : "Ложны";
            MessageBox.Show($"Параметры фоторамки {isValid2}",
                "Построитель фоторамок",
                MessageBoxButton.OK);
        }
    }
}
