using BotLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BotConstructor.ViewModel
{
    public class ListViewModel: INotifyPropertyChanged
    {

        RelayCommand resultSelectionChangedCommand;

        String selectItem = String.Empty;
        public String SelectItem
        {
            get { return selectItem; }
            set
            {
                selectItem = value;
                OnPropertyChanged("SelectItem");
            }
        }

        ObservableCollection<Object> _list = new ObservableCollection<object>();
        public ObservableCollection<Object> List
        {
            get { return _list; }
            set
            {
                _list = value;
                OnPropertyChanged("List");
            }
        }

        public ListViewModel(IEnumerable<Object> list)
        {
            foreach (var i in list)
                List.Add(i);
        }

        //выбор из списка
        public RelayCommand ResultSelectionChangedCommand
        {
            get
            {
                return resultSelectionChangedCommand ??
                    (resultSelectionChangedCommand = new RelayCommand(
                        (o) => {
                            try
                            {
                                var control = o as ListBox;
                                if (control == null) return;

                                SelectItem = control.SelectedItem.ToString();
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }

                        }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
