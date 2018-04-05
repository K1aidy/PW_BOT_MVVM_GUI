using BotConstructor.ViewModel;
using BotLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для EditBtnView.xaml
    /// </summary>
    public partial class EditBtnView : Window
    {
        EditBtnViewModel ebvm;
        public EditBtnView(ObservableCollection<BotButton> btnList)
        {
            InitializeComponent();
            ebvm = new EditBtnViewModel(btnList);
            this.DataContext = ebvm;
        }

        public ObservableCollection<BotButton> ReturnResult()
        {
            ebvm.RefreshIndexs();
            return ebvm._BtnList;
        }

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            if (this.ebvm._BtnList.Count() > 0)
            {
                this.DialogResult = true;
            }  
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
