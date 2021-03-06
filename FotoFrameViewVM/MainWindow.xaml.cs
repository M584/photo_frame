﻿using System;
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
        private const string _nameProgram = "Построитель фоторамки";

        public MainWindow()
        {
            InitializeComponent();
            this.Title = _nameProgram;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var photoFrame = this.Resources["PhotoFrame"]
                as PhotoFrameViewModel;
            var isValid = (bool)photoFrame?.BuildModel();

            var msg = isValid ? "Фоторамка успешно построена."
                : "Исправьте параметры фоторамки.";
            var typeMessage = isValid ? MessageBoxImage.Information
                : MessageBoxImage.Warning;

            MessageBox.Show(msg,
                _nameProgram,
                MessageBoxButton.OK,
                typeMessage);
        }
    }
}
