using System;
using System.Collections.Generic;
using System.Text;

namespace PWFramework
{
    /// <summary>
    /// Класс, содержащий необходимые оффсеты
    /// </summary>
    [Serializable]
    public class Offsets
    {
        //ID иконки в трее
        const UInt32 trayID = 101;

        private static Offsets instance;

        private Offsets()
        { }

        public static Offsets getInstance()
        {
            if (instance == null)
                instance = new Offsets();
            return instance;
        }

        public static void setInstance(Offsets ofs)
        {
            if (ofs != null)
            {
                instance = ofs;
                instance.RefreshOffsets();
            }
                
        }

        #region Адреса и офсеты

        //базовый адрес
        //private String baseAdress;
        public Int32 BaseAdress { get; set; }
        //гейм адрес
        public Int32 GameAdress { get; set; }// = 0x00EFFDAC;
        //GUI адрес
        public Int32 GuiAdress { get; set; }// = 0x00AE71C0;
        //адрес для инжекта скиллов
        public Int32 UseSkill { get; set; }// = 0x4BC760;
        //адрес для отправки пакетов
        public Int32 SendPacket { get; set; }// = 0x0087A600;
        //адреса для инжекта движения
        public Int32 Action_1 { get; set; }// = 0x04CF3D0;
        public Int32 Action_2 { get; set; }// = 0x04D5950;
        public Int32 Action_3 { get; set; }// = 0x04CF9E0;
        // Счетчик уведомлений
        public Int32 InviteCount { get; set; }// = 0x00F0DF94;
        // Указатель на структуру уведомлений
        public Int32 InviteStruct { get; set; }// = 0x00F0DF88;
        //Указатель на начало чата
        public Int32 ChatStart { get; set; }// = 0x00F06768;
        //Указатель на число сообщений
        public Int32 ChatNumber { get; set; }// = 0x00F06774;
        //смещение к ID сообщения
        public Int32 MsgId { get; set; }// = 0x14;
        //смещение к типу сообщения
        public Int32 MsgType { get; set; }// = 0x4;
        //смещение к msg-form1
        public Int32 Msg_form1 { get; set; }// = 0xC;
        //смещение к msg-form2
        public Int32 Msg_form2 { get; set; }// = 0x8;
        //смещение к WID сообщения
        public Int32 MsgWid { get; set; }// = 0x20;
        //смещение к ID игрока, отправившего первое приглашение 
        public Int32 InviteWidPlayer { get; set; }// = 0x10;
        //смещение к ID пати, в которую приглашают
        public Int32 InviteWidParty { get; set; }// = 0x14;
        //смещение до gameadress
        public Int32 OffsetToGameAdress { get; set; }// = 0x1C;
        //смещение до структуры перса
        public Int32 OffsetToPersStruct { get; set; }// = 0x34;
        //смещение до счетчика бафов
        public Int32 OffsetToCountBufs { get; set; }// = 0x398;
        //смещение до структуры бафов
        public Int32 OffsetToBufsArray { get; set; }// = 0x390;
        //смещение до счетчика умений
        public Int32 OffsetToSkillsCount { get; set; }// = 0x157C;
        //смещение до массива умений
        public Int32 OffsetToSkillsArray { get; set; }// = 0x1578;
        //смещение до юзающегося скилла
        public Int32 OffsetToCurrentSkill { get; set; }// = 0x7ec;
        //смещение до ID скилла
        public Int32 OffsetToIdSkill { get; set; }// = 0x8;
        //смещение до кулдауна скилла
        public Int32 OffsetToCdSkill { get; set; }// = 0x10;
        //смещение до структуры пати
        public Int32 OffsetToParty { get; set; }// = 0x7D0;
        //смещение до начала структуры пати
        public Int32 OffsetToStructParty { get; set; }// = 0x14;
        //смещение до счетчика пати
        public Int32 OffsetToCountParty { get; set; }// = 0x18;
        //смещение до имени персонажа
        public Int32 offsetToName { get; set; }// = 0x700;
        //смщение до идентификатора класса
        public Int32 OffsetToClassID { get; set; }// = 0x0704;
        //смещение до состояния копания шахты
        public Int32 OffsetToMiningState { get; set; }// = 0x288;
        //cмещение до wid для работы окна Win_QuickAction
        public Int32 OffsetToWidWin_QuickAction { get; set; }// = 0xf4;
        //смещения к координатам
        public Int32 OffsetToX { get; set; }// = 0x3c;
        public Int32 OffsetToY { get; set; }// = 0x44;
        public Int32 OffsetToZ { get; set; }// = 0x40;
        //смещение к walk_mode
        public Int32 OffsetToWalkMode { get; set; }// = 0x710;
        //смещение к wid персонажа
        public Int32 OffsetToWid { get; set; }// = 0x4B8;
        //смещение к wid таргета
        public Int32 OffsetToTargetWid { get; set; }// = 0x5A4;
        //смещение до хэштаблиц
        public Int32 OffsetToHashTables { get; set; }// = 0x1c;
        //смещение до структуры игроков
        public Int32 OffsetToPlayersStruct { get; set; }// = 0x1c;
        //смещение до начала массива игроков
        public Int32 OffsetToBeginPlayersStruct { get; set; }// = 0x98;
        //смещение до счетчика игроков
        public Int32 OffsetToPlayersCount { get; set; }// = 0x18;
        //смещение до структуры мобов
        public Int32 OffsetToMobsStruct { get; set; }// = 0x20;
        //смещение до начала массива мобов
        public Int32 OffsetToBeginMobsStruct { get; set; }// = 0x5C;
        //смещение до счетчика мобов
        public Int32 OffsetToMobsCount { get; set; }// = 0x18;
        //смещение до имени моба 
        public Int32 OffsetToMobName { get; set; }// = 0x260;
        //смещение до wid моба
        public Int32 OffsetToMobWid { get; set; }// = 0x114;
        //смещение к структуре инвентаря
        public Int32 OffsetToInventStruct { get; set; }
        //смещение к началу списка ячеек
        public Int32 OffsetToInventStructSecond { get; set; }
        //смещение до счетчика количества ячеек в инвентаре
        public Int32 OffsetToCellsCount { get; set; }
        //смещение до типа предмета в ячейке
        public Int32 OffsetToItemInCellType { get; set; }
        //смещение до id предмета в ячейке
        public Int32 OffsetToItemInCellID { get; set; }
        //смещение до количества предметов в ячейке
        public Int32 OffsetToItemInCellCount { get; set; }
        //смещение до цены предмета в ячейке
        public Int32 OffsetToItemInCellPrice { get; set; }
        //смещение до имени предмета в ячейке
        public Int32 OffsetToItemInCellName { get; set; }
        //смещение до имени предмета в ячейке
        public Int32 OffsetItemInCellLevel { get; set; }

        #endregion

        /// <summary>
        /// Метод обновления цепочек смещений
        /// </summary>
        public void RefreshOffsets()
        {
            GetCountParty = new Int32[] { OffsetToGameAdress,OffsetToPersStruct, OffsetToParty, OffsetToCountParty };
            GetStructParty = new Int32[] { OffsetToGameAdress, OffsetToPersStruct, OffsetToParty, OffsetToStructParty };
            GetClassId = new Int32[] { OffsetToGameAdress, OffsetToPersStruct, OffsetToClassID };
            GetMiningState = new Int32[] { OffsetToGameAdress, OffsetToPersStruct, OffsetToMiningState };
            GetX = new Int32[] { OffsetToGameAdress, OffsetToPersStruct, OffsetToX };
            GetY = new Int32[] { OffsetToGameAdress, OffsetToPersStruct, OffsetToY };
            GetZ = new Int32[] { OffsetToGameAdress, OffsetToPersStruct, OffsetToZ };
            GetWalkMode = new Int32[] { OffsetToGameAdress, OffsetToPersStruct, OffsetToWalkMode };
            GetWid = new Int32[] { OffsetToGameAdress, OffsetToPersStruct, OffsetToWid };
            GetName = new Int32[] { OffsetToGameAdress, OffsetToPersStruct, offsetToName, 0x0 };
            GetSkillsCount = new Int32[] { OffsetToGameAdress, OffsetToPersStruct, OffsetToSkillsCount };
            GetCurrentSkill = new Int32[] { OffsetToGameAdress, OffsetToPersStruct, OffsetToCurrentSkill };
            GetBufsCount = new Int32[] { OffsetToGameAdress, OffsetToPersStruct, OffsetToCountBufs };
            GetMobsCount = new Int32[] { OffsetToGameAdress, OffsetToHashTables, OffsetToMobsStruct, OffsetToMobsCount };
            GetPlayersCount = new Int32[] { OffsetToGameAdress, OffsetToHashTables, OffsetToPlayersStruct, OffsetToPlayersCount };
            GetInventCellsCount = new int[] { OffsetToGameAdress, OffsetToPersStruct, OffsetToInventStruct, OffsetToCellsCount };
        }

        #region Цепочки смещений
        //цепочка оффсетов до названия локации
        public Int32[] GetLocationName { get; set; }
        //цепочка оффсетов до счетчика пати
        public Int32[] GetCountParty { get; set; }
        //цепочка оффсетов до структуры пати
        public Int32[] GetStructParty { get; set; }
        //цепочка оффсетов до ID класс
        public Int32[] GetClassId { get; set; }
        //цепочка оффсетов до состояния копки
        public Int32[] GetMiningState { get; set; }
        //цепочка оффсетов до координаты X
        public Int32[] GetX { get; set; }
        //цепочка оффсетов до координаты Y
        public Int32[] GetY { get; set; }
        //цепочка оффсетов до координаты Z
        public Int32[] GetZ { get; set; }
        //цепочка оффсетов до walk_mode
        public Int32[] GetWalkMode { get; set; }
        //цепочка оффсетов до wid персонажа
        public Int32[] GetWid { get; set; }
        //цепочка оффсетов до имени персонажа
        public Int32[] GetName { get; set; }
        //цепочка оффсетов до счетчика скиллов
        public Int32[] GetSkillsCount { get; set; }
        //цепочка оффсетов до счетчика бафов
        public Int32[] GetBufsCount { get; set; }
        //цепочка оффсетов до структуры юзающегося скилла
        public Int32[] GetCurrentSkill { get; set; }
        //цепочка смещение до счетчика мобов
        public Int32[] GetMobsCount { get; set; }
        //цепочка смещение до счетчика игроков
        public Int32[] GetPlayersCount { get; set; }
        //цепочка смещение до счетчика ячеек
        public Int32[] GetInventCellsCount { get; set; }
        //цепочка оффсетов до ID скилла
        public Int32[] GetToIdSkill(Int32 iter)
        {
            return new Int32[] {OffsetToGameAdress, OffsetToPersStruct, OffsetToSkillsArray, 0x4*iter, OffsetToIdSkill };
        }
        //цепочка оффсетов до кулдауна скилла
        public Int32[] GetToCdSkill(Int32 iter)
        {
            return new Int32[] { OffsetToGameAdress, OffsetToPersStruct, OffsetToSkillsArray, 0x4 * iter, OffsetToCdSkill };
        }
        //цепочка оффсетов до ID бафа
        public Int32[] GetIdBuf(Int32 iter)
        {
            return new Int32[] { OffsetToGameAdress, OffsetToPersStruct, OffsetToBufsArray, 0x12 * iter};
        }
        //цепочка оффсетов до имени моба
        public Int32[] GetNameMob(Int32 iter)
        {
            return new Int32[] { OffsetToGameAdress, OffsetToHashTables, OffsetToMobsStruct,
                                OffsetToBeginMobsStruct, 0x4 * iter, OffsetToMobName, 0x0};
        }
        //цепочка оффсетов до WID моба
        public Int32[] GetWidMob(Int32 iter)
        {
            return new Int32[] { OffsetToGameAdress, OffsetToHashTables, OffsetToMobsStruct,
                                OffsetToBeginMobsStruct, 0x4 * iter, OffsetToMobWid };
        }
        //цепочка оффсетов до WID игрока
        public Int32[] GetWidPlayer(Int32 iter)
        {
            return new Int32[] { OffsetToGameAdress, OffsetToHashTables, OffsetToPlayersStruct,
                                OffsetToBeginPlayersStruct, 0x4 * iter, OffsetToWid };
        }
        //цепочка оффсетов до WID таргета игрока
        public Int32[] GetTargetWidPlayer(Int32 iter)
        {
            return new Int32[] { OffsetToGameAdress, OffsetToHashTables, OffsetToPlayersStruct,
                                OffsetToBeginPlayersStruct, 0x4 * iter, OffsetToTargetWid};
        }
        //цепочка оффсетов до ID сообщения
        public Int32 GetMsgId(Int32 chatStartAddress , Int32 iter)
        {
            return (chatStartAddress + (iter * 0x24)) + MsgId;
        }
        //цепочка оффсетов до типа сообщения
        public Int32 GetMsgType(Int32 chatStartAddress, Int32 iter)
        {
            return (chatStartAddress + (iter * 0x24)) + MsgType;
        }
        //цепочка оффсетов до WID сообщения
        public Int32 GetMsgWid(Int32 chatStartAddress, Int32 iter)
        {
            return (chatStartAddress + (iter * 0x24)) + MsgWid;
        }
        //цепочка оффсетов до 1й формы сообщения
        public Int32 GetMsgForm1(Int32 chatStartAddress, Int32 iter)
        {
            return (chatStartAddress + (iter * 0x24)) + Msg_form1;
        }
        //цепочка оффсетов до 2й формы сообщения
        public Int32 GetMsgForm2(Int32 chatStartAddress, Int32 iter)
        {
            return (chatStartAddress + (iter * 0x24)) + Msg_form2;
        }
        //цепочка оффсетов до типа предмета в ячейке
        public Int32[] OffsetsItemInCellType(Int32 iter)
        {
            return new Int32[] { OffsetToGameAdress, OffsetToPersStruct, OffsetToInventStruct, OffsetToInventStructSecond, 0x4 * iter, OffsetToItemInCellType };
        }
        //цепочка оффсетов до ID предмета в ячейке
        public Int32[] OffsetsItemInCellID(Int32 iter)
        {
            return new Int32[] { OffsetToGameAdress, OffsetToPersStruct, OffsetToInventStruct, OffsetToInventStructSecond, 0x4 * iter, OffsetToItemInCellID };
        }
        //цепочка оффсетов до количества предметов в ячейке
        public Int32[] OffsetsItemInCellCount(Int32 iter)
        {
            return new Int32[] { OffsetToGameAdress, OffsetToPersStruct, OffsetToInventStruct, OffsetToInventStructSecond, 0x4 * iter, OffsetToCellsCount };
        }
        //цепочка оффсетов до цены предмета в ячейке
        public Int32[] OffsetsItemInCellPrice(Int32 iter)
        {
            return new Int32[] { OffsetToGameAdress, OffsetToPersStruct, OffsetToInventStruct, OffsetToInventStructSecond, 0x4 * iter, OffsetToItemInCellPrice };
        }
        //цепочка оффсетов до имени предмета в ячейке
        public Int32[] OffsetsItemInCellName(Int32 iter)
        {
            return new Int32[] { OffsetToGameAdress, OffsetToPersStruct, OffsetToInventStruct, OffsetToInventStructSecond, 0x4 * iter, OffsetToItemInCellName, 0x0 };
        }
        //цепочка оффсетов до уровня заточки предмета в ячейке
        public Int32[] OffsetsItemInCellLevel(Int32 iter)
        {
            return new Int32[] { OffsetToGameAdress, OffsetToPersStruct, OffsetToInventStruct, OffsetToInventStructSecond, 0x4 * iter, OffsetItemInCellLevel};
        }

        #endregion


    }
}
