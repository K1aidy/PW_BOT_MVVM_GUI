using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BotLibrary
{
    public class BotButton : INotifyPropertyChanged
    {
        //имя кнопки
        String name;
        public String Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        //индекс
        Int32 index;
        public Int32 Index
        {
            get { return index; }
            set
            {
                index = value;
                OnPropertyChanged("Index");
            }
        }
        //выполняемый метод
        String methodName;
        public String MethodName
        {
            get { return methodName; }
            set
            {
                methodName = value;
                OnPropertyChanged("MethodName");
            }
        }

        //класс кнопки
        IButton classButton;
        public IButton ClassButton
        {
            get { return classButton; }
            set
            {
                classButton = value;
                OnPropertyChanged("ClassButton");
            }
        }

        RelayCommand clickButtonCommand;
        //команда нажатия на кнопку
        public RelayCommand СlickButtonCommand
        {
            get
            {
                return clickButtonCommand ??
                    (clickButtonCommand = new RelayCommand(
                        (o) => {
                            try
                            {
                                var bot = o as Bot;
                                if (bot == null) return;
                                this.ClassButton?.Do(bot).Wait();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }

                        }));
            }
        }

        public override string ToString()
        {
            return Name ?? "отсутствует имя";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
