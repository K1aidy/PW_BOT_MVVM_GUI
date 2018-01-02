using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PWFramework_Mnogoletov
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
            Name = CalcMethods.ReadString(handle, Offsets.getInstance().BaseAdress, Offsets.getInstance().GetName);
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
            if (pw_1.ProcessID == pw_2.ProcessID && pw_1.Name == pw_2.Name)
                return true;
            else
                return false;
        }
        public static Boolean operator !=(PwClient pw_1, PwClient pw_2)
        {
            if (pw_1.ProcessID == pw_2.ProcessID && pw_1.Name == pw_2.Name)
                return false;
            else
                return true;
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
