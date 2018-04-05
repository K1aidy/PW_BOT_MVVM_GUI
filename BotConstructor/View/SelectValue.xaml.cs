using BotConstructor.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Логика взаимодействия для SelectValue.xaml
    /// </summary>
    public partial class SelectValue : Window
    {

        ListViewModel lvm;
        public SelectValue(IEnumerable<Object> list)
        {
            InitializeComponent();
            lvm = new ListViewModel(list);
            this.DataContext = lvm;
        }

        //возвращение выбранного значения
        public String ReturnResult() { return lvm.SelectItem; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedIndex >= 0)
                this.DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
