using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOCharp
{
    using Classes.Database;
    using System.Diagnostics;
    using System.Threading;
    using System.Windows.Forms;
    using static Define;
    public static class LoginHandler : base(User)
    {
        internal static void VersionCheck(User user)
        {
            Packet resut = new Packet(15);
            resut.SetByte(WIZ_VERSION_CHECK);
            resut.SetByte(0);
            resut.SetShort(__VERSION);
            resut.SetInt64(0);
            user.Send(resut);
        }

        internal static void AccountLogin(User user, Packet pkt)
        {
            KODatabase db = new KODatabase();
            // Login işlemleri.
            string AccountID = pkt.GetString();
            string strPassword = pkt.GetString();

            Packet result = new Packet(WIZ_LOGIN);

            result.SetByte(DBAgent.GameLogin(AccountID, strPassword,ref result));
            
            Thread thd = new Thread(() =>
            {
                user.sSid = user.g_pMain.GetNewSocketID();
            });

            thd.Start();

            user.Send(result);

            user.strAccountID = AccountID;
        }

        internal static void LoadCharInfoReq(User user, Packet pkt)
        {
            string strChar1 = String.Empty, strChar2 = String.Empty, strChar3 = String.Empty, strChar4 = String.Empty;

            Packet result = new Packet(WIZ_ALLCHAR_INFO_REQ);
            result.SetByte(1);

            if (__VERSION >= 1920)
                result.SetByte(1);

            KODatabase db = new KODatabase();
            Debug.WriteLine("Char bilgleri istendi {0}", user.strAccountID);
            var AllCharID = db.ACCOUNT_CHAR.Where(u => u.strAccountID == user.strAccountID).FirstOrDefault();

            if (AllCharID != null)
            {
                Debug.WriteLine("Char bilgileri alındı.");
                LoadCharInfo(AllCharID.strCharID1.TrimEnd(' ', '/'), ref result);
                LoadCharInfo(AllCharID.strCharID2.TrimEnd(' ', '/'), ref result);
                LoadCharInfo(AllCharID.strCharID3.TrimEnd(' ', '/'), ref result);
            }
            else
                Debug.WriteLine("Char bilgileri alınamadı.");

            user.Send(result);
        }

        private static void LoadCharInfo(string charID, ref Packet result)
        {
            KODatabase db = new KODatabase();

            var user = db.USERDATAs.Where(u => u.strUserID == charID).First();

            int nHair = 0;
            short sClass = 0;
            byte bRace = 0, bLevel = 0, bFace = 0, bZone = 0;
            string strItem = String.Empty;
            byte[] ItemArray = new byte[INVENTORY_TOTAL * 8];// = ToBytes(strItem);

            if (user != null)
            {
                result.SetString(user.strUserID);
                result.SetByte(user.Race);
                result.SetShort(user.Class);
                result.SetByte(user.Level);
                result.SetByte(user.Face);
                result.SetDword(user.HairRGB);
                result.SetByte(user.Zone);

                Packet item = new Packet();
                item.append(user.strItem, user.strItem.Length);

                for (int i = 0; i < SLOT_MAX; i++)
                {
                    int nItemID;
                    short sDurability, sCount;
                    nItemID = item.GetDWORD();
                    sDurability = item.GetShort();
                    sCount = item.GetShort();
                    if (i == HEAD || i == BREAST || i == SHOULDER || i == LEG || i == GLOVE || i == FOOT || i == RIGHTHAND || i == LEFTHAND)
                    {
                        result.SetDword(nItemID);
                        result.SetShort(sDurability);
                    }
                }
            }
            else
            {
                result.SetString(charID);
                result.SetByte(bRace);
                result.SetShort(sClass);
                result.SetByte(bLevel);
                result.SetByte(bFace);
                result.SetDword(nHair);
                result.SetByte(bZone);

                for (int i = 0; i < SLOT_MAX; i++)
                {
                    if (i == HEAD || i == BREAST || i == SHOULDER || i == LEG || i == GLOVE || i == FOOT || i == RIGHTHAND || i == LEFTHAND)
                    {
                        result.SetDword(0);
                        result.SetShort(0);
                    }
                }
            }
            
        }

        internal static void SelectNation(User user, Packet pkt)
        {
            Packet result = new Packet(WIZ_SEL_NATION);

            byte nation = pkt.GetByte();

            DBAgent.SelectNation(ref result,nation,user.strAccountID);

            user.Send(result);
        }

        internal static void SelCharToAgent(User user, Packet pkt)
        {
            Packet result = new Packet(WIZ_SEL_CHAR);

            byte bInit, bResult = 0;
            string strCharID = string.Empty, strAccountID = string.Empty;

            strAccountID = pkt.GetString();
            user.strCharID = strCharID = pkt.GetString();
            bInit = pkt.GetByte();


            if (user.GetAccountID() == String.Empty || user.GetName() == String.Empty ||
                !DBAgent.LoadUserData(user.GetAccountID(), strCharID, ref user) ||
                !DBAgent.LoadWarehouseData(user.GetAccountID(), user) ||
                !DBAgent.LoadPremiumServiceUser(user.GetAccountID(), user) ||
                !DBAgent.LoadSavedMagic(user))
            {
                Debug.WriteLine("Kullanıcı girişi sırasında hata oluştu");
                bResult = 0;
                goto fail_return;
            }

            bResult = 1;

            fail_return:
            user.SelectCharacter(bResult, bInit);            
        }

        internal static void CreateNewChar(User user, Packet pkt)
        {
            Packet result = new Packet(WIZ_NEW_CHAR);
            byte nHair;
            short sClass;
            byte bCharIndex, bRace, bFace, str, sta, dex, intel, cha, errorCode = 0;
            string strUserID;

            bCharIndex = pkt.GetByte();
            strUserID = pkt.GetString();
            bRace = pkt.GetByte();
            sClass = pkt.GetShort();
            bFace = pkt.GetByte();
            nHair = pkt.GetByte();
            str = pkt.GetByte();
            sta = pkt.GetByte();
            dex = pkt.GetByte();
            intel = pkt.GetByte();
            cha = pkt.GetByte();

            COEFFICIENT p_TableCoefficient = user.g_pMain.GetCoefficientData(sClass);
            
            if (bCharIndex > 2)
                errorCode = NEWCHAR_NO_MORE;
            else if (p_TableCoefficient == null
                || (str + sta + dex + intel + cha) > 300)
                errorCode = NEWCHAR_INVALID_DETAILS;
            else if (str < 50 || sta < 50 || dex < 50 || intel < 50 || cha < 50)
                errorCode = NEWCHAR_STAT_TOO_LOW;
            
            if (errorCode != 0)
            {
                result.SetByte(errorCode);
                user.Send(result);
                return;
            }

            int nRet = new KODatabase().CREATE_NEW_CHAR(user.GetAccountID(), bCharIndex, strUserID, bRace, sClass, nHair, bFace, str, sta, dex, intel, cha);
            result.SetByte((byte)nRet);

            user.Send(result);
        }
    }
}
