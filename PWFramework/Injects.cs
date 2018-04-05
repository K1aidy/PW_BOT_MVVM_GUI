using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWFramework
{
    public static class Injects
    {
        /// <summary>
        /// Инжект для GUI элементов
        /// </summary>
        /// <param name="win_struct"></param>
        /// <param name="command_text"></param>
        /// <param name="processID"></param>
        public static void GUI_Inject(int win_struct, int command_text, IntPtr oph)
        {
            try
            {
                // ---- Создаем скелет пакета для инжектирования
                byte[] gui_packet =
                {
                0x60,                           //Pushad
                0xB9, 0x0, 0x0, 0x0, 0x0,       //Mov_ECX + win_struct_address
                0x68, 0x0, 0x0, 0x0, 0x0,       //Push68 + command_text_address
                0xB8, 0x0, 0x0, 0x0, 0x0,       //Mov_EAX + call_address
                0xFF, 0xD0,                     //Call_EAX
                0x61,                           //Popad
                0xC3                            //Ret
                };

                // ---- заменяем указанные эелементы пакета адресом для GUI инжектирования
                var x = OfsPresenter.getInstance("GUI")[0];
                Buffer.BlockCopy(BitConverter.GetBytes(OfsPresenter.getInstance("GUI")[0]), 0, gui_packet, 12, 4);
                // ---- заменяем указанные эелементы пакета адресом структуры необходимого окна
                Buffer.BlockCopy(BitConverter.GetBytes(win_struct), 0, gui_packet, 2, 4);
                // ---- заменяем указанные эелементы пакета адресом функции необходимого контрола
                Buffer.BlockCopy(BitConverter.GetBytes(command_text), 0, gui_packet, 7, 4);
                // ---- временные переменные
                int lpNumberOfBytesWritten = 0;
                IntPtr lpThreadId;
                // ---- выделяем место в памяти
                IntPtr gui_address = WinApi.VirtualAllocEx(oph, IntPtr.Zero, 20, WinApi.AllocationType.Commit, WinApi.MemoryProtection.ReadWrite);
                // ---- записываем в выделенную память наш пакет
                WinApi.WriteProcessMemory(oph, (int)gui_address, gui_packet, 20, out lpNumberOfBytesWritten);
                // ---- запускаем записанную в память функцию
                IntPtr hProcThread = WinApi.CreateRemoteThread(oph, IntPtr.Zero, 0, gui_address, IntPtr.Zero, 0, out lpThreadId);
                // ---- Ожидаем завершения функции
                WinApi.WaitForSingleObject(hProcThread, WinApi.INFINITE);
                // ---- подчищаем за собой
                WinApi.VirtualFreeEx(oph, gui_address, 20, WinApi.FreeType.Release);
                WinApi.VirtualFreeEx(oph, hProcThread, 20, WinApi.FreeType.Release);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
