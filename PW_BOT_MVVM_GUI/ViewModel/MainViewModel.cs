using Ofset;
using PWFramework_Mnogoletov;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PW_BOT_MVVM_GUI.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        RelayCommand loadedCommand;
        RelayCommand onClosingCommand;

        public MainViewModel()
        {
            OfsPresenter.setConfig(Environment.CurrentDirectory + @"\ofs.txt", typeof(TxtReader));
            var t = OfsPresenter.getInstance("BA+GA");
        }

        //команда выполняемая при загрузке главного окна
        public RelayCommand LoadedCommand
        {
            get
            {
                return loadedCommand ??
                    (loadedCommand = new RelayCommand(
                        (o) => {
                            try
                            {
                                Debug.WriteLine("OK");
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }

                        }));
            }
        }
        //команда выполняемая при выходе
        public RelayCommand OnClosingCommand
        {
            get
            {
                return onClosingCommand ??
                    (onClosingCommand = new RelayCommand(
                        (o) => {
                            try
                            {
                                Properties.Settings.Default.Save();
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
