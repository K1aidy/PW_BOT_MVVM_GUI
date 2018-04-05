using BotConstructor.View;
using BotLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BotConstructor.ViewModel
{
    public class EditBtnViewModel : INotifyPropertyChanged
    {
        RelayCommand selectBtnCommand;
        RelayCommand addButtonCommand;
        RelayCommand deleteButtonCommand;
        RelayCommand loadedCommand;
        RelayCommand onClosingCommand;
        RelayCommand upIndexCommand;
        RelayCommand downIndexCommand;
        RelayCommand selectMethodCommand;

        //коллекция доступных классов ботов
        ObservableCollection<String> assemblyMethodCollection = new ObservableCollection<String>();

        BotButton _currentBtn;
        public BotButton CurrentBtn
        {
            get { return _currentBtn; }
            set
            {
                _currentBtn = value;
                OnPropertyChanged("CurrentBtn");
            }
        }


        ObservableCollection<BotButton> _btnList = new ObservableCollection<BotButton>();
        public ObservableCollection<BotButton> _BtnList
        {
            get { return _btnList; }
            set
            {
                _btnList = value;
                OnPropertyChanged("_BtnList");
            }
        } 

        public EditBtnViewModel(ObservableCollection<BotButton> btnList)
        {
            foreach (var btn in btnList)
                _BtnList.Add(new BotButton {Name = btn.Name, Index = btn.Index, MethodName = btn.MethodName });
            _BtnList.OrderBy(x => x.Index);
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

                                var dlls = Directory.GetFiles(Environment.CurrentDirectory, "*Addon.dll", SearchOption.TopDirectoryOnly);
                                foreach (var dll in dlls)
                                {
                                    foreach (Type type in Assembly.LoadFile(dll).GetTypes()
                                                    .Where(type => type.GetInterfaces().Contains(typeof(IButton))))
                                    {
                                        assemblyMethodCollection.Add(type.ToString());
                                    }
                                }
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
                                //Properties.Settings.Default.Save();
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }

                        }));
            }
        }

        //выбор кнопки
        public RelayCommand SelectBtnCommand
        {
            get
            {
                return selectBtnCommand ??
                    (selectBtnCommand = new RelayCommand(
                        (o) => {
                            try
                            {
                                var control = o as ListBox;
                                if (control == null) return;

                                var tempBtn = control.SelectedItem as BotButton;
                                if (tempBtn == null)
                                {
                                    this.CurrentBtn = null;
                                    return;
                                }
                                    
                                this.CurrentBtn = tempBtn;

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }

                        }));
            }
        }

        //добавить кнопку
        public RelayCommand AddButtonCommand
        {
            get
            {
                return addButtonCommand ??
                    (addButtonCommand = new RelayCommand(
                        (o) => {
                            try
                            {
                                var control = o as ListBox;
                                if (control == null) return;

                                BotButton tempBtn = new BotButton { Name = "Кнопка"};
                                this._BtnList.Add(tempBtn);

                                control.SelectedItem = tempBtn;
                                //control.SelectedIndex = control.Items.Count - 1;

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }

                        }));
            }
        }

        //удалить кнопку
        public RelayCommand DeleteButtonCommand
        {
            get
            {
                return deleteButtonCommand ??
                    (deleteButtonCommand = new RelayCommand(
                        (o) => {
                            try
                            {
                                var control = o as ListBox;
                                if (control == null) return;

                                var tempBtn = control.SelectedItem as BotButton;
                                if (tempBtn == null) return;

                                this._BtnList.Remove(tempBtn);
                                control.SelectedIndex = control.Items.Count - 1;

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }

                        }));
            }
        }

        //поменять с вышестоящим элементом
        public RelayCommand UpIndexCommand
        {
            get
            {
                return upIndexCommand ??
                    (upIndexCommand = new RelayCommand(
                        (o) => {
                            try
                            {
                                var control = o as ListBox;
                                if (control == null) return;

                                Int32 tempIndex = control.SelectedIndex;

                                if (tempIndex < 1) return;

                                var tempBtn = new BotButton
                                {
                                    Name = _BtnList[tempIndex].Name,
                                    Index = _BtnList[tempIndex].Index,
                                    MethodName = _BtnList[tempIndex].MethodName
                                };
                                _BtnList[tempIndex] = _BtnList[tempIndex - 1];
                                _BtnList[tempIndex - 1] = tempBtn;

                                control.SelectedIndex = tempIndex - 1;

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }

                        }));
            }
        }

        //поменять с нижестоящим элементом
        public RelayCommand DownIndexCommand
        {
            get
            {
                return downIndexCommand ??
                    (downIndexCommand = new RelayCommand(
                        (o) => {
                            try
                            {
                                var control = o as ListBox;
                                if (control == null) return;

                                Int32 tempIndex = control.SelectedIndex;
                                if (tempIndex > control.Items.Count - 2) return;

                                var tempBtn = new BotButton
                                {
                                    Name = _BtnList[tempIndex + 1].Name,
                                    Index = _BtnList[tempIndex + 1].Index,
                                    MethodName = _BtnList[tempIndex + 1].MethodName
                                };
                                _BtnList[tempIndex + 1] = _BtnList[tempIndex];
                                _BtnList[tempIndex] = tempBtn;

                                control.SelectedIndex = tempIndex + 1;


                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }

                        }));
            }
        }

        public void RefreshIndexs()
        {
            for (int index = 0; index < _BtnList.Count - 1; index++)
            {
                _BtnList[index].Index = index;
            }
        }

        //выбор репозитория
        public RelayCommand SelectMethodCommand
        {
            get
            {
                return selectMethodCommand ??
                    (selectMethodCommand = new RelayCommand(
                        (o) => {
                            try
                            {
                                var control = o as ListBox;
                                if (control == null) return;

                                var tempBot = control.SelectedItem as BotButton;
                                if (tempBot == null) return;

                                var dialogWnd = new SelectValue(assemblyMethodCollection.Cast<Object>());
                                if (dialogWnd.ShowDialog() == true && CurrentBtn != null)
                                {
                                    CurrentBtn.MethodName = dialogWnd.ReturnResult();
                                }
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
