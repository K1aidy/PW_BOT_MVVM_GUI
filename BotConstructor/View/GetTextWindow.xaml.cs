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
using System.Windows.Shapes;

namespace BotConstructor.View
{
    /// <summary>
    /// Логика взаимодействия для GetTextWindow.xaml
    /// </summary>
    public partial class GetTextWindow : Window
    {
        public GetTextWindow()
        {
            InitializeComponent();
        }

        public String ReturnValue()
        {
            return this.txtValue.Text;
        }

        private void Button_OkClick(object sender, RoutedEventArgs e)
        {
            if (this.txtValue.Text.Length > 0)
                this.DialogResult = true;
        }

        private void Button_CancelClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
