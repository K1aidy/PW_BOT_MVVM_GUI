using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;

namespace PWFramework_Mnogoletov
{
    /// <summary>
    /// Класс, выполняющий сложные расчеты
    /// </summary>
    public static class CalcMethods
    {
        /// <summary>
        /// считываем Int32 по одному оффсету
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static Int32 CalcInt32Value(IntPtr handle, Int32 address)
        {
            IntPtr read;
            byte[] buffer = new byte[4];
            WinApi.ReadProcessMemory(handle, (IntPtr)address, buffer, buffer.Length, out read);
            Int32 x = BitConverter.ToInt32(buffer, 0);
            return x;
        }

        /// <summary>
        /// считываем значение Int32 по нашей цепочке оффсетов
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="address"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Int32 ReadInt(IntPtr handle, int address, int[] offset)
        {
            byte[] buffer = new byte[4];
            Int32 value = address;
            value = CalcInt32Value(handle, value);
            if (offset.Length > 0)
            {
                for (int i = 0; i < offset.Length - 1; i++)
                {
                    value = CalcInt32Value(handle, value + offset[i]);
                }
                value = CalcInt32Value(handle, value + offset[offset.Length - 1]);
            }
            return value;
        }

        /// <summary>
        /// считываем Float по одному оффсету
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static float CalcFloatValue(IntPtr handle, Int32 address)
        {
            IntPtr read;
            byte[] buffer = new byte[50];
            WinApi.ReadProcessMemory(handle, (IntPtr)address, buffer, buffer.Length, out read);
            float x = BitConverter.ToSingle(buffer, 0);
            return x;
        }

        /// <summary>
        /// считываем значение Float по нашей цепочке оффсетов
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="address"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static float ReadFloat(IntPtr handle, int address, int[] offset)
        {
            byte[] buffer = new byte[50];
            var value = address;
            value = CalcInt32Value(handle, value);
            if (offset.Length > 0)
            {
                for (int i = 0; i < offset.Length - 1; i++)
                {
                    value = CalcInt32Value(handle, value + offset[i]);
                }
            }
            return CalcFloatValue(handle, value + offset[offset.Length - 1]); ;
        }

        /// <summary>
        /// считываем значение String по нашей цепочке оффсетов
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="address"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static string ReadString(IntPtr handle, int address, int[] offset)
        {
            byte[] buffer = new byte[4];
            var value = address;
            value = CalcInt32Value(handle, value);
            if (offset.Length > 0)
            {
                for (int i = 0; i < offset.Length - 1; i++)
                {
                    value = CalcInt32Value(handle, value + offset[i]);
                }
            }
            return CalcStringValue(handle, value + offset[offset.Length - 1]); ;
        }

        /// <summary>
        /// считываем String по одному оффсету
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string CalcStringValue(IntPtr handle, Int32 address)
        {
            IntPtr read;
            byte[] buffer = new byte[300];
            WinApi.ReadProcessMemory(handle, (IntPtr)address, buffer, buffer.Length, out read);
            string x = Encoding.Unicode.GetString(buffer);
            return (x.IndexOf('\0') != -1) ? x.Substring(0, x.IndexOf('\0')) : x;
        }

        /// <summary>
        /// считываем Byte по одному оффсету
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static byte CalcByteValue(IntPtr handle, Int32 address)
        {
            IntPtr read;
            byte[] buffer = new byte[1];
            WinApi.ReadProcessMemory(handle, (IntPtr)address, buffer, buffer.Length, out read);
            return buffer[0];
        }

        /// <summary>
        /// считываем значение Int32 по нашей цепочке оффсетов
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="address"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static byte ReadByte(IntPtr handle, int address, int[] offset)
        {
            byte[] buffer = new byte[50];
            var value = address;
            value = CalcInt32Value(handle, value);
            if (offset.Length > 0)
            {
                for (int i = 0; i < offset.Length - 1; i++)
                {
                    value = CalcInt32Value(handle, value + offset[i]);
                }
            }
            return CalcByteValue(handle, value + offset[offset.Length - 1]); ;
        }

        /// <summary>
        /// Поиск wid по названию NPC/моба/пета,
        /// возвращает первый встретившийся,
        /// если ничего не найдено, то возвращает 0.
        /// </summary>
        /// <param name="processID"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int MobSearch(IntPtr oph, string name)
        {
            try
            {
                int mobs_count = ReadInt(oph, Offsets.getInstance().BaseAdress, Offsets.getInstance().GetMobsCount);
                for (int i = 0; i < mobs_count; i++)
                {
                    string mob_name = ReadString(oph, Offsets.getInstance().BaseAdress, Offsets.getInstance().GetNameMob(i));
                    if (mob_name.Length > 0)
                    {
                        //если имя моба/NPC/пета совпадает с заданным, возвращает его wid
                        if (mob_name.IndexOf(name) != -1)
                            return ReadInt(oph, Offsets.getInstance().BaseAdress, Offsets.getInstance().GetWidMob(i));
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// считываем значение String по нашей цепочке оффсетов с кодировкой ASCII
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="address"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static string ReadString_ASCII(IntPtr handle, int address, int[] offset)
        {
            byte[] buffer = new byte[4];
            var value = address;
            value = CalcInt32Value(handle, value);
            if (offset.Length > 0)
            {
                for (int i = 0; i < offset.Length - 1; i++)
                {
                    value = CalcInt32Value(handle, value + offset[i]);
                }
            }
            return CalcStringValue_ASCII(handle, value + offset[offset.Length - 1]); ;
        }

        /// <summary>
        /// считываем String по одному оффсету с кодировкой ASCII
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string CalcStringValue_ASCII(IntPtr handle, Int32 address)
        {
            IntPtr read;
            byte[] buffer = new byte[300];
            WinApi.ReadProcessMemory(handle, (IntPtr)address, buffer, buffer.Length, out read);
            string x = Encoding.ASCII.GetString(buffer);
            return (x.IndexOf('\0') != -1) ? x.Substring(0, x.IndexOf('\0')) : x;
        }

        /// <summary>
        /// Узнаем адрес контрола активного окна указанного по ID процесса клиента
        /// </summary>
        /// <param name="processID"></param>
        /// <returns></returns>
        public static int[] CalcControlAddress(IntPtr oph)
        {
            try
            {
                int[] result = { 0, 0 };
                string name_control = "";
                int[] offset_win_struct = { 0x1c, 0x18, 0x8, 0x74 };
                int address_win_struct = ReadInt(oph, Offsets.getInstance().BaseAdress, offset_win_struct);

                int temp_address = CalcInt32Value(oph, address_win_struct + 0x1cc);
                for (int k = 0; k < 50; k++)
                {
                    int window_address = CalcInt32Value(oph, temp_address + 0x4);
                    for (int j = 0; j < k; j++)
                    {
                        window_address = CalcInt32Value(oph, window_address + 0x4);
                    }
                    int controlstruct_address = CalcInt32Value(oph, window_address + 0x8);
                    window_address = CalcInt32Value(oph, controlstruct_address + 0x18);
                    name_control = CalcStringValue_ASCII(oph, window_address + 0x0);
                    if (name_control.IndexOf("Btn_Back") != -1)
                    {
                        int address_to_command_control = CalcInt32Value(oph, controlstruct_address + 0x1c);
                        result[0] = address_win_struct;
                        result[1] = address_to_command_control;
                        return result;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Узнаем адрес активного окна
        /// </summary>
        /// <param name="oph"></param>
        /// <returns></returns>
        public static int CalcAddressActiveWindow(IntPtr oph)
        {
            try
            {
                int[] offset_win_struct = { 0x1c, 0x18, 0x8, 0x74 };
                return ReadInt(oph, Offsets.getInstance().BaseAdress, offset_win_struct);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Узнаем адрес контрола активного окна указанного по ID процесса клиента.
        /// Возвращает {указатель на окно, указатель на команду контрола, указатель на контрол}
        /// </summary>
        /// <param name="processID"></param>
        /// <returns></returns>
        public static Int32[] CalcControlAddress(IntPtr oph, String window_name, String control_name)
        {
            try
            {
                //вводим временный массив дял хранения результатов
                Int32[] result = { 0, 0, 0 };
                //вводим пустые переменные для хранения имени окна и контрола
                String win_name_8C = String.Empty;
                String win_name_AC = String.Empty;
                String name_control = String.Empty;
                //считываем начало массива структур окон нижнего или верхнего уровня уровня во временную переменную
                Int32 temp_address_8C = ReadInt(oph, Offsets.getInstance().BaseAdress, new Int32[] { 0x1c, 0x18, 0x8, 0x8C });
                Int32 temp_address_AC = ReadInt(oph, Offsets.getInstance().BaseAdress, new Int32[] { 0x1c, 0x18, 0x8, 0xAC });
                //в цикле проверяем имя каждого окна, пока не найдем нужное
                for (Int32 iter = 0; iter < 1500; iter++)
                {
                    if (iter > 0)
                    {
                        temp_address_8C = CalcInt32Value(oph, temp_address_8C + 0x0);
                        temp_address_AC = CalcInt32Value(oph, temp_address_AC + 0x0);
                    }
                    Int32 temp_address_8C_2 = CalcInt32Value(oph, temp_address_8C + 0x8);
                    Int32 temp_address_AC_2 = CalcInt32Value(oph, temp_address_AC + 0x8);
                    Int32 temp_address_8C_3 = CalcInt32Value(oph, temp_address_8C_2 + 0x4c);
                    Int32 temp_address_AC_3 = CalcInt32Value(oph, temp_address_AC_2 + 0x4c);
                    win_name_8C = CalcStringValue_ASCII(oph, temp_address_8C_3 + 0x0);
                    win_name_AC = CalcStringValue_ASCII(oph, temp_address_AC_3 + 0x0);
                    if (win_name_8C == "" && win_name_AC == "")
                        break;
                    if (CalcStringValue_ASCII(oph, temp_address_8C_3 + 0x0) == window_name)
                    {
                        //сохраняем значение временной переменной и выходим из цикла
                        result[0] = temp_address_8C_2;
                        break;
                    }
                    if (CalcStringValue_ASCII(oph, temp_address_AC_3 + 0x0) == window_name)
                    {
                        //сохраняем значение временной переменной и выходим из цикла
                        result[0] = temp_address_AC_2;
                        break;
                    }
                }
                //если не нашли нужное окно, то выходим и возвращаем 0
                if (result[0] == 0)
                    return result;
                //считываем начало массива контролов найденного окна во временную переменную
                Int32 temp_address = CalcInt32Value(oph, result[0] + 0x1cc);
                //в цикле проверяем имя каждого контрола, пока не найдем нужное
                for (Int32 iter = 0; iter < 100; iter++)
                {
                    if (iter > 0)
                        temp_address = CalcInt32Value(oph, temp_address + 0x4);
                    //считываем адреса структуры контрола
                    Int32 controlstruct_address = CalcInt32Value(oph, temp_address + 0x8);
                    Int32 temp_address_2 = CalcInt32Value(oph, controlstruct_address + 0x18);
                    name_control = CalcStringValue_ASCII(oph, temp_address_2 + 0x0);
                    if (CalcStringValue_ASCII(oph, temp_address_2 + 0x0) == control_name)
                    {
                        //сохраняем значение адреса структуры контрола
                        result[1] = CalcInt32Value(oph, controlstruct_address + 0x1c);
                        result[2] = controlstruct_address;
                        return result;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Запись значения по указанной области памяти указанного процесса
        /// </summary>
        /// <param name="processID"></param>
        /// <param name="value"></param>
        /// <param name="adress"></param>
        public static void WriteProcessBytes(IntPtr oph, int value, int adress)
        {
            byte[] new_value = BitConverter.GetBytes(value);
            int number_of_bytes_written;
            WinApi.WriteProcessMemory(oph, adress, new_value, new_value.Length, out number_of_bytes_written);
        }

        /// <summary>
        /// Поиск среди ближайших игроков по wid
        /// </summary>
        /// <param name="processID"></param>
        /// <param name="wid"></param>
        /// <returns></returns>
        public static bool SearchPlayerNearby(IntPtr oph, int wid)
        {
            try
            {
                //считаем количество народа вокруг
                int player_count = ReadInt(oph, Offsets.getInstance().BaseAdress, Offsets.getInstance().GetPlayersCount);
                for (int i = 0; i < player_count; i++)
                {
                    //считываем wid каждого
                    int player_wid = ReadInt(oph, Offsets.getInstance().BaseAdress, Offsets.getInstance().GetWidPlayer(i));
                    //если найден нужный wid, выходим и возвращаем true
                    if (player_wid == wid)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Ищем wid таргета по wid персонажа, с которого берем ассист
        /// </summary>
        /// <param name="processID"></param>
        /// <param name="wid"></param>
        /// <returns></returns>
        public static int TargetPlayerWid(IntPtr oph, uint wid)
        {
            try
            {
                //считаем количество народа вокруг
                int player_count = ReadInt(oph, Offsets.getInstance().BaseAdress, Offsets.getInstance().GetPlayersCount);
                for (int i = 0; i < player_count; i++)
                {
                    //считываем wid каждого
                    int player_wid = ReadInt(oph, Offsets.getInstance().BaseAdress, Offsets.getInstance().GetWidPlayer(i));
                    //если найден нужный wid, выходим и возвращаем true
                    if (player_wid == wid)
                    {
                        return ReadInt(oph, Offsets.getInstance().BaseAdress, Offsets.getInstance().GetTargetWidPlayer(i));
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }
    }
}
