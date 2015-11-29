
using KOCharp.Classes.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KOCharp
{
    using System.Diagnostics;
    using static Define;
    public static class DBAgent
    {
        internal static void SelectNation(ref Packet result, byte nation, string strUserID)
        {
            KODatabase db = new KODatabase();

            ACCOUNT_CHAR aChar = db.ACCOUNT_CHAR.Where(acc => acc.strAccountID == strUserID).FirstOrDefault();
            WAREHOUSE wHouse = db.WAREHOUSEs.Where(wh => wh.strAccountID == strUserID).FirstOrDefault();

            if (wHouse == null)
            {
                wHouse = new WAREHOUSE();
                wHouse.strAccountID = strUserID;
                wHouse.nMoney = 0;
                wHouse.dwTime = 0;
                wHouse.WarehouseData = new byte[1536];
                wHouse.strSerial = new byte[1536];
                wHouse.WarehouseDataTime = new byte[1536];

                db.WAREHOUSEs.Add(wHouse);
                
                Debug.WriteLine("WAREHOUSE hesabı bulunamadığından yeni WAREHOUSE eklendi.");
            }

            if (aChar == null)
            {
                aChar = new ACCOUNT_CHAR();
                aChar.strAccountID = strUserID;
                aChar.bNation = nation;
                aChar.strCharID1 = null;
                aChar.strCharID2 = null;
                aChar.strCharID3 = null;
                aChar.strCharID4 = null;
                db.ACCOUNT_CHAR.Add(aChar);
                Debug.WriteLine("Hesap bulunamadığından yeni account eklendi.");
            }
            else
            {
                nation = aChar.bNation;
                Debug.WriteLine("Hesap bilgileri alındı. Seçilen ırk {0}", nation);
            }

            db.SaveChanges();

            if (nation != KARUS && nation != ELMORAD)
            {
                result.SetByte(0);
                return;
            }

            result.SetByte(nation);
            
        }

        internal static byte GameLogin(string accountID, string strPassword, ref Packet result)
        {
            KODatabase db = new KODatabase();

            int nRet = db.TB_USER.Where(usr => usr.strAccountID == accountID && usr.strPasswd == strPassword).Count();

            if (nRet < 1)
                return 0;

            ACCOUNT_CHAR Account = db.ACCOUNT_CHAR.Where(acc => acc.strAccountID == accountID).FirstOrDefault();

            if (Account == null)
                return 0;

            return Account.bNation;
        }

        internal static bool ReadLetter(string v, int nLetterID, string strMessage)
        {
            throw new NotImplementedException();
        }

        internal static bool GetLetterList(string strAccountID, ref Packet result, bool isHistory)
        {
            try {
                KODatabase db = new KODatabase();

                byte bType = isHistory ? (byte)2 : (byte)1;

                var letterlist = db.MAIL_BOX.Where(i => i.strRecipientID == strAccountID && i.bStatus == bType && !i.bDeleted).Take(10);
                byte Count = (byte)letterlist.Count();

                result.SetByte(1).SetByte(bType);

                foreach (MAIL_BOX letter in letterlist)
                {
                    result.SetDword(letter.nLetterID)
                        .SetByte(letter.bStatus)
                        .SetString(letter.strSubject)
                        .SetString(letter.strSenderID)
                        .SetByte(letter.bType);

                    if (letter.bType == 2)
                        result.SetDword(letter.nItemID).SetShort(letter.sCount).SetDword(letter.nCoins);

                    int DayDiff = (Int32)UNIXTIME - (Int32)(letter.dtSendDate.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                    byte RemainDays = (byte)(DayDiff / 60 / 60 / 24);
                    result.SetDword((int)UNIXTIME).SetByte(RemainDays);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        internal static byte GetUnreadLetter(string strAccountID)
        {
            KODatabase db = new KODatabase();

            return (byte)db.MAIL_BOX.Where(i => i.strRecipientID == strAccountID && i.bStatus == 1 && !i.bDeleted).Count();
        }


        public static bool LoadUserData(string AccountID, string strCharID, ref User pUser)
        {
            KODatabase db = new KODatabase();
            try
            {
                USERDATA pData = db.USERDATAs.Where(user => user.strUserID == strCharID).FirstOrDefault();

                if (pData == null)
                    return false;

                pUser.m_bNation = pData.Nation;
                pUser.m_bRace   = pData.Race   ;
                pUser.m_sClass  = pData.Class  ;
                pUser.m_nHair   = pData.HairRGB;
                pUser.m_bRank   = pData.Rank   ;
                pUser.m_bTitle  = pData.Title  ;
                pUser.m_bLevel  = pData.Level  ;
                pUser.m_iExp    = pData.Exp    ;
                pUser.m_iLoyalty= pData.Loyalty;
                pUser.m_bFace   = pData.Face   ;
                pUser.m_bCity   = pData.City   ;
                pUser.m_bKnights= pData.Knights;
                pUser.m_bFame   = pData.Fame   ;
                pUser.m_sHp     = pData.Hp     ;
                pUser.m_sMp     = pData.Mp     ;
                pUser.m_sSp     = pData.Sp     ;
                pUser.m_bStats[(int)StatType.STAT_STR] = pData.Strong;
                pUser.m_bStats[(int)StatType.STAT_STA] = pData.Sta;
                pUser.m_bStats[(int)StatType.STAT_DEX] = pData.Dex;
                pUser.m_bStats[(int)StatType.STAT_INT] = pData.Intel;
                pUser.m_bStats[(int)StatType.STAT_CHA] = pData.Cha;
                pUser.m_bAuthority= pData.Authority;
                pUser.m_sPoints   = pData.Points   ;
                pUser.m_iGold     = pData.Gold     ;
                pUser.m_bZone     = pData.Zone     ;
                pUser.m_sBind     = (long)pData.Bind;

                pUser.m_curx = (float)(pData.PX / 100.0f);
                pUser.m_curz = (float)(pData.PZ / 100.0f);
                pUser.m_cury = (float)(pData.PY / 100.0f);
                pUser.m_oldx = pUser.m_curx;
                pUser.m_oldy = pUser.m_cury;
                pUser.m_oldz = pUser.m_curz;

                pUser.m_dwTime = pData.dwTime;

                pUser.m_bstrSkill = pData.strSkill.ToCharArray();

                Packet itemBuffer = new Packet(pData.strItem);
                Packet serialBuffer = new Packet(pData.strSerial);
                Packet itemTimeBuffer = new Packet(pData.strItemTime);
                for (int i = 0; i < INVENTORY_TOTAL; i++)
                {
                    Int64 nSerialNum;
                    Int32 nItemID;
                    Int16 sDurability, sCount, nRentalTime;
                    Int32 nItemTime;

                    nItemID = itemBuffer.GetDWORD();
                    sDurability = itemBuffer.GetShort();
                    sCount = itemBuffer.GetShort();
                    nSerialNum = serialBuffer.GetInt64();
                    nItemTime = itemTimeBuffer.GetDWORD();
                    nRentalTime = itemTimeBuffer.GetShort();

                    _ITEM_DATA pItem = new _ITEM_DATA();

                    pItem.nNum = nItemID;
                    pItem.sCount = sCount;
                    pItem.sDuration = sDurability;
                    pItem.nSerialNum = nSerialNum;
                    pItem.nExpirationTime = nItemTime;
                    pItem.sRemainingRentalTime = nRentalTime;

                    pUser.m_sItemArray[i] = pItem;
                }

                return true;
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Karakter bilgileri alınırken özel durum oluştu : " + ex.Message);

                return false; 
            }
        }

        internal static bool LoadWarehouseData(string AccountID, User pUser)
        {
            return true;
        }

        internal static bool LoadPremiumServiceUser(string AccountID, User pUser)
        {
            return true;
        }

        internal static bool LoadSavedMagic(User pUser)
        {
            return true;
        }
    }
}
