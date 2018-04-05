using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BotLibrary
{
    public class Bot : INotifyPropertyChanged
    {
        Boolean isStart;
        public Boolean IsStart
        {
            get { return isStart; }
            set
            {
                isStart = value;
                OnPropertyChanged("IsStart");
            }
        }

        String parameter;
        public String Parameter
        {
            get { return parameter; }
            set
            {
                parameter = value;
                OnPropertyChanged("Parameter");
            }
        }

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

        String repositoryName;
        public String RepositoryName
        {
            get { return repositoryName; }
            set
            {
                repositoryName = value;
                OnPropertyChanged("RepositoryName");
            }
        }

        List<String> filters { get; set; }
        public List<String> Filters
        {
            get { return filters; }
            set
            {
                filters = value;
                OnPropertyChanged("Filters");
            }
        }

        public override string ToString()
        {
            return Name ?? "No Name";
        }

        //public virtual Task Do(Object param) {
        //    return new Task(() => { return; });
        //}

        public virtual void Do(Object param)
        {
            return;
        }
        public virtual bool Check(Object param)
        {
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
