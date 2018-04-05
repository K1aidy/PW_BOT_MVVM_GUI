using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Controls;
using BotLibrary;
using System.Reflection;
using BotConstructor.View;

namespace BotConstructor.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        RelayCommand loadedCommand;
        RelayCommand onClosingCommand;
        RelayCommand saveCommand;
        RelayCommand addCommand;
        RelayCommand addBotCommand;
        RelayCommand deleteBotCommand;
        RelayCommand appComboboxSelectionChangedCommand;
        RelayCommand botListSelectionChangedCommand;
        RelayCommand selectRepCommand;
        RelayCommand editBtnListCommand;

        //коллекция приложений
        ObservableCollection<BotApp> appCollection = new ObservableCollection<BotApp>();

        //коллекция доступных классов ботов
        ObservableCollection<String> assemblyBotCollection = new ObservableCollection<String>();

        //выбранное приложение
        BotApp currentApp;

        //выбранный бот
        Bot currentBot;

        public MainViewModel()
        {
            //OfsPresenter.setConfig(Environment.CurrentDirectory + @"\ofs.txt", typeof(TxtReader));
            //var t = OfsPresenter.getInstance("BA+GA");
        }

        public Bot CurrentBot
        {
            get { return currentBot; }
            set
            {
                currentBot = value;
                OnPropertyChanged("CurrentBot");
            }
        }
        public BotApp CurrentApp
        {
            get { return currentApp; }
            set
            {
                currentApp = value;
                OnPropertyChanged("CurrentApp");
            }
        }

        public ObservableCollection<BotApp> AppCollection
        {
            get { return appCollection; }
            set
            {
                appCollection = value;
                OnPropertyChanged("AppCollection");
            }
        }

        public ObservableCollection<String> AssemblyBotCollection
        {
            get { return assemblyBotCollection; }
            set
            {
                assemblyBotCollection = value;
                OnPropertyChanged("AssemblyBotCollection");
            }
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
                                foreach (var file in new DirectoryInfo(Environment.CurrentDirectory).GetFiles("*.json"))
                                {
                                    BotApp tempBotApp = JsonConvert.DeserializeObject<BotApp>(File.ReadAllText(file.FullName));
                                    if (tempBotApp?.BtnCollection == null)
                                        tempBotApp.BtnCollection = new ObservableCollection<BotButton>();
                                    AppCollection.Add(tempBotApp);
                                }

                                var dlls = Directory.GetFiles(Environment.CurrentDirectory, "*Addon.dll", SearchOption.TopDirectoryOnly);
                                foreach (var dll in dlls)
                                {
                                    foreach (Type type in Assembly.LoadFile(dll).GetTypes()
                                                    .Where(type => type.IsSubclassOf(typeof(Bot))))
                                    {
                                        AssemblyBotCollection.Add(type.ToString());
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

        //команда удаления бота
        public RelayCommand DeleteBotCommand
        {
            get
            {
                return deleteBotCommand ??
                    (deleteBotCommand = new RelayCommand(
                        (o) => {
                            try
                            {
                                var control = o as ListBox;
                                if (control == null) return;

                                var selBot = control.SelectedItem as Bot;
                                if (selBot == null) return;

                                CurrentApp.BotCollection.Remove(selBot);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }

                        }));
            }
        }

        //команда добавления бота
        public RelayCommand AddBotCommand
        {
            get
            {
                return addBotCommand ??
                    (addBotCommand = new RelayCommand(
                        (o) => {
                            try
                            {
                                var control = o as ComboBox;
                                if (control == null) return;
                                if (control.SelectedIndex < 0) return;

                                var getTxtWnd = new GetTextWindow();
                                if (getTxtWnd.ShowDialog() == true)
                                {
                                    CurrentApp.BotCollection.Add(new Bot { Name = getTxtWnd.ReturnValue(), RepositoryName = "" });
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }

                        }));
            }
        }

        //команда сохранения приложения
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                    (saveCommand = new RelayCommand(
                        (o) => {
                            try
                            {
                                var text = JsonConvert.SerializeObject(CurrentApp);
                                File.WriteAllText($"{CurrentApp.Name}.json", text);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }

                        }));
            }
        }

        //команда добавления приложения
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                    (addCommand = new RelayCommand(
                        (o) => {
                            try
                            {
                                var control = o as ComboBox;
                                if (control == null) return;

                                var getTxtWnd = new GetTextWindow();
                                if (getTxtWnd.ShowDialog() == true)
                                {
                                    AppCollection.Add(new BotApp { Name = $"{getTxtWnd.ReturnValue()}", BotCollection = new ObservableCollection<Bot>() });
                                    control.SelectedIndex = control.Items.Count - 1;
                                }
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

        //команда выбора приложения
        public RelayCommand AppComboboxSelectionChangedCommand
        {
            get
            {
                return appComboboxSelectionChangedCommand ??
                    (appComboboxSelectionChangedCommand = new RelayCommand(
                        (o) => {
                            try
                            {
                                var control = o as ComboBox;
                                if (control == null) return;

                                CurrentApp = control.SelectedItem as BotApp;
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }

                        }));
            }
        }

        //команда выбора бота
        public RelayCommand BotListSelectionChangedCommand
        {
            get
            {
                return botListSelectionChangedCommand ??
                    (botListSelectionChangedCommand = new RelayCommand(
                        (o) => {
                            try
                            {
                                var control = o as ListBox;
                                if (control == null) return;

                                CurrentBot = (control.SelectedItem as Bot);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }

                        }));
            }
        }

        //редактирование списка кнопок
        public RelayCommand EditBtnListCommand
        {
            get
            {
                return editBtnListCommand ??
                    (editBtnListCommand = new RelayCommand(
                        (o) => {
                            try
                            {
                                var control = o as ComboBox;
                                if (control == null) return;

                                var tempApp = control.SelectedItem as BotApp;
                                if (tempApp == null) return;

                                var dialogWnd = new EditBtnView(tempApp.BtnCollection);
                                if (dialogWnd.ShowDialog() == true)
                                {
                                    tempApp.BtnCollection = dialogWnd.ReturnResult();
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }

                        }));
            }
        }

        //выбор репозитория
        public RelayCommand SelectRepCommand
        {
            get
            {
                return selectRepCommand ??
                    (selectRepCommand = new RelayCommand(
                        (o) => {
                            try
                            {
                                var control = o as ListBox;
                                if (control == null) return;

                                var tempBot = control.SelectedItem as Bot;
                                if (tempBot == null) return;

                                var dialogWnd = new SelectValue(assemblyBotCollection.Cast<Object>());
                                if (dialogWnd.ShowDialog() == true && CurrentBot != null)
                                {
                                    CurrentBot.RepositoryName = dialogWnd.ReturnResult();
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
