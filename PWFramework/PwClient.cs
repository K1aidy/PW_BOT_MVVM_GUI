using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PWFramework
{
    public class PwClient : INotifyPropertyChanged
    {
        protected IntPtr descrypt;
        public IntPtr Descrypt
        {
            get { return descrypt; }
            private set { descrypt = value; OnPropertyChanged("Descrypt"); }
        }

        protected IntPtr handle;
        public IntPtr Handle
        {
            get { return handle;}
            private set { handle = value; OnPropertyChanged("handle"); }
        }
        protected String name;
        public String Name
        {
            get { return name; }
            private set { name = value; OnPropertyChanged("name"); }
        }

        protected Int32 money;
        public Int32 Money
        {
            get { return money; }
            set { money = value; OnPropertyChanged("Money"); }
        }

        protected Int32 processID;
        public Int32 ProcessID
        {
            get { return processID; }
            private set { processID = value; OnPropertyChanged("processID"); }
        }

        public PwClient(IntPtr descript)
        {
            this.Descrypt = descript;
            Int32 processID;
            WinApi.GetWindowThreadProcessId(descript, out processID);
            ProcessID = processID;
            handle = WinApi.OpenProcess(WinApi.ProcessAccessFlags.All, false, ProcessID);
            Name = CalcMethods.ReadString(handle, OfsPresenter.getInstance("BA")[0], OfsPresenter.getInstance("GA+Player+Name+0x0"));
            Money = CalcMethods.ReadInt(handle, OfsPresenter.getInstance("BA")[0], OfsPresenter.getInstance("GA+Player+Money"));
        }
        ~PwClient()
        {
            WinApi.CloseHandle(Handle);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public override String ToString()
        {
            return this.Name + " : " + this.Handle.ToString() + " : " + this.ProcessID.ToString();
        }

        public static Boolean operator == (PwClient pw_1, PwClient pw_2)
        {
            return pw_1?.Equals(pw_2) ?? false;
        }
        public static Boolean operator !=(PwClient pw_1, PwClient pw_2)
        {
            return !(pw_1 == pw_2);
        }
        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;
            PwClient p = obj as PwClient;
            if ((Object)p == null)
                return false;

            return (ProcessID == p.ProcessID) && (Name == p.Name);
        }

        public override Int32 GetHashCode()
        {
            return ProcessID ^ (Int32)Name[0];
        }

    }
}
