using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BotLibrary
{
    public class BotApp : INotifyPropertyChanged
    {
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

        ObservableCollection<Bot> botCollection;

        public ObservableCollection<Bot> BotCollection
        {
            get { return botCollection; }
            set
            {
                botCollection = value;
                OnPropertyChanged("BotCollection");
            }
        }

        ObservableCollection<BotButton> btnCollection;

        public ObservableCollection<BotButton> BtnCollection
        {
            get { return btnCollection; }
            set
            {
                btnCollection = value;
                OnPropertyChanged("BtnCollection");
            }
        }

        public override string ToString()
        {
            return Name ?? "No Name";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
