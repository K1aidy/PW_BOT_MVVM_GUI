using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace PWFramework
{
    public class TxtReader : IOfsReader
    {
        public Dictionary<String, Int32> SetOfs(String path)
        {
            try
            {
                Dictionary<String, Int32> list = new Dictionary<String, Int32>();

                String[] rows = File.ReadAllLines(path);

                if (rows.Length == 0) throw new Exception("Файл пуст");

                foreach (String s in rows)
                {
                    String[] temp = s.Split('=');
                    if (!Int32.TryParse(temp[1].Trim().TrimStart(new char[] {'0', 'x' }), NumberStyles.HexNumber, null, out Int32 value))
                        throw new Exception($"Не удалось конвертировать значение {temp[0].Trim()}");
                    list.Add(temp[0].Trim(), value);
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
