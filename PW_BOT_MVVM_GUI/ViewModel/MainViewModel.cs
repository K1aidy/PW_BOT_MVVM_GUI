using BotLibrary;
using Microsoft.Win32;
using Newtonsoft.Json;
using PWFramework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PW_BOT_MVVM_GUI.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        RelayCommand loadedCommand;
        RelayCommand onClosingCommand;
        RelayCommand openExeFile;
        RelayCommand startBotCommand;
        RelayCommand stopBotCommand;

        List<Assembly> assemblies = new List<Assembly>();
        List<Type> types = new List<Type>();
        ObservableCollection<Object> mass = new ObservableCollection<object> {new object(), new object() };
        List<Thread> threadPool = new List<Thread>();

        BotApp currentApp;
        public BotApp CurrentApp
        {
            get { return currentApp; }
            set
            {
                currentApp = value;
                OnPropertyChanged("CurrentApp");
            }
        }

        ObservableCollection<Bot> bots = new ObservableCollection<Bot>();
        public ObservableCollection<Bot> Bots
        {
            get { return bots; }
            set
            {
                bots = value;
                OnPropertyChanged("Bots");
            }
        }


        public MainViewModel()
        {

        }

        //команда для выбора бинарного файла
        public RelayCommand OpenExeFile
        {
            get
            {
                return openExeFile ??
                    (openExeFile = new RelayCommand(
                        (o) => {
                            try
                            {
                                OfsFinder.FindOfset(Environment.CurrentDirectory + @"\regular.txt", Environment.CurrentDirectory + @"\PW.exe", "");
                                //OpenFileDialog ofd = new OpenFileDialog();
                                //ofd.Filter = "elementclient.exe|*.exe|Все файлы|*.*";
                                //if (ofd.ShowDialog() == true)
                                //{

                                //}
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }

                        }));
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
                                var window = o as Window;
                                if (window == null) return;
                                //окно во весь экран
                                window.Width = SystemParameters.WorkArea.Width;
                                window.Height = SystemParameters.WorkArea.Height;

                                //подгружаем офсеты
                                OfsPresenter.setConfig(Environment.CurrentDirectory + @"\ofs.txt", typeof(TxtReader));
                                //подгружаем приложение, укзанное в app.config
                                String appName = ConfigurationManager.AppSettings["AppName"]?.ToString();
                                if (String.IsNullOrEmpty(appName)) throw new Exception("Не задано имя приложения в app.config");
                                if (File.Exists($"{appName}.json"))
                                    CurrentApp = JsonConvert.DeserializeObject<BotApp>(File.ReadAllText($"{appName}.json"));
                                //подгружаем сборки *Addon.dll
                                var dlls = Directory.GetFiles(Environment.CurrentDirectory, "*Addon.dll", SearchOption.TopDirectoryOnly);
                                foreach (var dll in dlls)
                                {
                                    var assembly = Assembly.LoadFile(dll);
                                    assemblies.Add(assembly);
                                    types.AddRange(assembly.GetTypes());

                                }
                                foreach(var btn in CurrentApp.BtnCollection)
                                {
                                    var type = types.FirstOrDefault(t => t.FullName == btn.MethodName);
                                    if (type != null)
                                    {
                                        btn.ClassButton = (IButton)Activator.CreateInstance(type);
                                        //btn.ClassButton.Do("Загружено").Start();
                                    }
                                }
                                for (int i = 0; i < CurrentApp.BotCollection.Count; i++)
                                {
                                    threadPool.Add(null);
                                    var type = types.FirstOrDefault(t => t.FullName == CurrentApp.BotCollection[i].RepositoryName);
                                    if (type != null)
                                    {
                                        var tempBot = (Bot)Activator.CreateInstance(type);
                                        tempBot.Name = CurrentApp.BotCollection[i].Name;
                                        tempBot.Parameter = CurrentApp.BotCollection[i].Parameter;
                                        tempBot.Filters = CurrentApp.BotCollection[i].Filters;
                                        tempBot.RepositoryName = CurrentApp.BotCollection[i].RepositoryName;
                                        CurrentApp.BotCollection[i] = tempBot;
                                    }
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

        //команда для запуска ботов
        public RelayCommand StartBotCommand
        {
            get
            {
                return startBotCommand ??
                    (startBotCommand = new RelayCommand(
                        (o) => {
                            try
                            {
                                //TODO
                                var list = o as ListBox;
                                if (list == null) return;
                                foreach (var item in list.SelectedItems)
                                {
                                    //находим индекс элемента
                                    var index = list.Items.IndexOf(item);
                                    //если поток работает, то пропускаем итерацию
                                    if (threadPool[index]?.ThreadState == System.Threading.ThreadState.Running) continue;
                                    //получаем тип репозитория
                                    var botType = types.FirstOrDefault(t => t.FullName == (item as Bot)?.RepositoryName);
                                    //запускаем метод 'Do' найденного репозитория в новом потоке
                                    threadPool[index] = new Thread(() => botType?.GetMethod("Do").Invoke(item, new Object[] { (item as Bot)?.Parameter })) { IsBackground = true };
                                    threadPool[index].Start();
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }

                        }));
            }
        }

        //команда для остановки ботов
        public RelayCommand StopBotCommand
        {
            get
            {
                return stopBotCommand ??
                    (stopBotCommand = new RelayCommand(
                        (o) => {
                            try
                            {
                                //TODO
                                var list = o as ListBox;
                                if (list == null) return;
                                foreach (var item in list.SelectedItems)
                                {
                                    //находим индекс элемента
                                    var index = list.Items.IndexOf(item);
                                    //вызываем исключение в потокес
                                    threadPool[index]?.Abort();
                                    threadPool[index] = null;

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
