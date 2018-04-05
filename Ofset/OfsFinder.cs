using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Ofset
{
    public class OfsFinder
    {
        /// <summary>
        /// Метод поиска офсетов по регулярным выражениям
        /// </summary>
        /// <param name="puthToRegular">Путь к файлу с регулярными выражениями и соответствующим им ключам</param>
        /// <param name="puthToBinFile">Путь к бинарному файлу(дампу клиента)</param>
        /// <param name="puthToSave">Путь к файлу для сохранения найденных офсетов</param>
        public static void FindOfset(String pathToRegular, String pathToBinFile, String pathToSave)
        {
            //объект для хранения прочитанного дампа
            StringBuilder sb_dump = new StringBuilder();
            //объект для хранения файла офсетов
            StringBuilder sb_ofs = new StringBuilder();

            //считываем дамп в стрингбилдер (именно по 16 байт в строку - это важно для поиска)
            using (BinaryReader reader = new BinaryReader(File.Open(pathToBinFile, FileMode.Open, FileAccess.Read, FileShare.Read), Encoding.ASCII))
            {
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    var bytes = reader.ReadBytes(16);

                    foreach (Byte b in bytes)
                    {
                        sb_dump.AppendFormat("{0:X2}", b);
                    }
                    //sb_dump.AppendLine();
                }
            }
            //File.WriteAllText(Environment.CurrentDirectory + @"\dump.txt", sb_dump.ToString());
            //словарь регулярных выражений
            Dictionary<String, String> dictRegular = new Dictionary<string, string>();
            //считываем файл с регулярными выражениями
            var regularArray = File.ReadAllLines(pathToRegular, Encoding.Default);
            //добавляем считанные регулярки в словарь
            for(int i = 0; i < regularArray.Length; i++)
            {
                String key = Regex.Match(regularArray[i], @"(?<=^).*?(?=\=)", RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline).Value;
                String pattern = Regex.Match(regularArray[i], @"(?<=\[).*?(?=\]$)", RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline).Value;
                dictRegular.Add(key, pattern);
            }

            //словарь найденных значений
            Dictionary<String, String> dictFindOfs = new Dictionary<String, String>();

            foreach(String key in dictRegular.Keys)
            {
                var match = Regex.Match(sb_dump.ToString(), dictRegular[key], RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                String findValue = match.Value;
                
                var b = (match.Index/2 + 0x400000).ToString("X8");
                
                if (!String.IsNullOrEmpty(findValue))
                {
                    dictFindOfs.Add(key, b);
                }
                else
                {

                }
            }

            
            sb_dump.Clear();
            sb_ofs.Clear();
        }
    }
}
