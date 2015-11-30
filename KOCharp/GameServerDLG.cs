using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace KOCharp
{
    using System.Net.Sockets;
    using static Define;
    using static DBAgent;
    using static Classes.Database.LoadDatabase;
    using System.Windows.Forms;
    using Classes.Database;

    public class Pair<T1, T2, T3>
    {
        public T1 First { get; set; }
        public T2 Second { get; set; }
        public T3 Other { get; set; }

        public Pair(T1 t1,T2 t2,T3 t3)
        {
            First = t1;
            Second = t2;
            Other = t3;
        }
    }

    public class GameServerDLG
    {
        private main main;
        public List<Pair<short, User, int>> m_arUserList = new List<Pair<short, User,int>>();
        //public List<User> m_arUserList = new List<User>();
        public List<COEFFICIENT> m_CoefficientArray = new List<COEFFICIENT>();
        public List<LEVEL_UP> m_LevelUpArray = new List<LEVEL_UP>();
        public List<_ITEM_TABLE> m_ItemArray = new List<_ITEM_TABLE>();
        public List<Tuple<User, short, short>> m_Regions = new List<Tuple<User, short, short>>();
        public List<_ZONE_INFO> m_ZoneArray = new List<_ZONE_INFO>();
        public List<_START_POSITION> m_StartPosition = new List<_START_POSITION>();
        //public List<Tuple<short, Knights>> m_KnightsArray = new List<Tuple<short, Knights>>();
        public List<_MAGIC_TABLE> m_MagicTable = new List<_MAGIC_TABLE>();
        public List<_MAGIC_TYPE1> m_MagicType1 = new List<_MAGIC_TYPE1>();
        public List<_MAGIC_TYPE2> m_MagicType2 = new List<_MAGIC_TYPE2>();
        public List<_MAGIC_TYPE3> m_MagicType3 = new List<_MAGIC_TYPE3>();
        public List<_MAGIC_TYPE4> m_MagicType4 = new List<_MAGIC_TYPE4>();
        public List<_MAGIC_TYPE5> m_MagicType5 = new List<_MAGIC_TYPE5>();
        public List<_MAGIC_TYPE6> m_MagicType6 = new List<_MAGIC_TYPE6>();
        public List<_MAGIC_TYPE7> m_MagicType7 = new List<_MAGIC_TYPE7>();
        public List<_MAGIC_TYPE8> m_MagicType8 = new List<_MAGIC_TYPE8>();
        public List<_MAGIC_TYPE9> m_MagicType9 = new List<_MAGIC_TYPE9>();
        public List<CNpc> m_arNpcList = new List<CNpc>();
        public List<K_NPC> m_NpcList = new List<K_NPC>();
        public List<K_MONSTER> m_MonsterList = new List<K_MONSTER>();
        public List<Knights> m_arKnights = new List<Knights>();
        //public List<Npc> m_arNpc = new List<Npc>();
        //public List<Unit> m_unitList = new List<Unit>();
        public int m_nServerNum;
        internal bool m_bPointCheckFlag;

        public GameServerDLG(main main)
        {
            this.main = main;

            Console.WriteLine("LOADING COEFFICIENT");
            if (!LoadCoefficient(ref m_CoefficientArray))
            {
                MessageBox.Show("Database okunamadı. Server kapatılıyor."); Environment.Exit(0);
            }
            Console.WriteLine("\t\t[\tOK\t]");

            Console.WriteLine("LOADING LEVELUP");
            if (!LoadLevelUp(ref m_LevelUpArray))
            {
                MessageBox.Show("Database okunamadı. Server kapatılıyor."); Environment.Exit(0);
            }
            Console.WriteLine("\t\t[\tOK\t]");

            Console.WriteLine("LOADING ITEM");
            if (!LoadItemTable(ref m_ItemArray))
            {
                MessageBox.Show("Database okunamadı. Server kapatılıyor."); Environment.Exit(0);
            }
            Console.WriteLine("\t\t[\tOK\t]");

            Console.WriteLine("LOADING NPCLIST");
            if (!LoadNpc(ref m_NpcList))
            {
                MessageBox.Show("Database okunamadı. Server kapatılıyor."); Environment.Exit(0);
            }
            Console.WriteLine("\t\t[\tOK\t]");
            

            // Maksimum kullanıcı sayısında oyuncu portu aç
            for (int i = 0; i < USER_MAX; i++)
                m_arUserList.Add(new Pair<short, User, int>(Convert.ToInt16(i), null, 0));
            
        }


        internal short GetNewSocketID()
        {
            foreach (Pair<short, User, int> pUser in m_arUserList)
            {
                /* Rezerve edilmiş bir soket ise kontrol et, 
                 * eğer oyuncu oyunda değil ise bağlantıyı kapat soketi kullanılır hale getir. */
                if (UNIXTIME - pUser.Other > TIME_PORT_RESERVE && pUser.Second != null && !pUser.Second.isInGame())
                {
                    pUser.Second.OnDisconnect();
                    pUser.Second = null;

                    return pUser.First;
                }
                /* Rerve edilemiş ve kullanılmadıysa */
                else if (UNIXTIME - pUser.Other > TIME_PORT_RESERVE && pUser.Second == null)
                {
                    pUser.Second = null;

                    return pUser.First;
                }
                /* Kullanılabilir durumda bir port ise */
                else
                {
                    continue;
                }
            }           
            
            return -1;
        }

        public COEFFICIENT GetCoefficientData(short sClass)
        {
            foreach(COEFFICIENT coef in m_CoefficientArray)
                if (coef.sClass == sClass)
                    return coef;
            
            return null;         
        }

        internal void UserInOutForMe(User user)
        {
        }

        internal void MerchantUserInOutForMe(User user)
        {
        }

        internal void NpcInOutForMe(User user)
        {
            
        }

        internal Int64 GetExpByLevel(int level)
        {
            foreach (LEVEL_UP itr in m_LevelUpArray)
                if (itr.level == level)
                    return (Int64)itr.Exp;

            return 0;
        }

        internal void TempleEventGetActiveEventTime(User user)
        {
        }

        internal _ITEM_TABLE GetItemPtr(int nNum)
        {
            foreach(_ITEM_TABLE item in m_ItemArray)
            {
                if (item.m_iNum == nNum)
                    return item;
            }

            return null;
        }

        internal void Send_Region(Packet pkt, short rx, short rz, User pExceptUser, short nEventRoom)
        {
            for (int x = -1; x <= 1; x++)
                for (int z = -1; z <= 1; z++)
                    Send_UnitRegion(pkt, (short)(rx + x), (short)(rz + z), pExceptUser, nEventRoom);
        }

        public void Send_UnitRegion(Packet pkt, int rx, int rz, User pExceptUser, short nEventRoom = 0)
        {
            foreach (Pair<short, User, int> itr in m_arUserList)
            {
                User pUser = itr.Second;

                if (pUser == null || pUser.isDisconnect())
                    continue;
                // if (!pUser.isInGame())
                //     continue;
                //
                // if (nEventRoom > 0 && nEventRoom != pUser.GetEventRoom())
                //     continue;

                if (pUser.GetRegionZ() != rz ||
                    pUser.GetRegionX() != rx)
                    continue;

                pUser.Send(pkt);
            }

        }

        internal CNpc GetNpcPtr(int tid)
        {
            return null;
        }

        internal User GetUserPtr(int tid)
        {
            lock(m_arUserList)
            {
                foreach (Pair<short, User, int> pUser in m_arUserList)
                {
                    if (pUser.First == tid)
                        return pUser.Second;
                }
            }
            return null;
        }

        internal void Send_All(Packet result, byte bNation = 0)
        {
            foreach (Pair<short, User, int> pUser in m_arUserList)
            {
                if (pUser.Second != null && pUser.Second.isInGame())
                {
                    if (bNation > 0 && pUser.Second.GetNation() != bNation)
                        continue;

                    pUser.Second.Send(result);
                }
            }
        }

        internal void Send_Zone(Packet result, byte bZone, byte bNation =0 )
        {
            foreach (Pair<short, User, int> pUser in m_arUserList)
            {
                if (pUser.Second != null && pUser.Second.isInGame()
                    && pUser.Second.GetZoneID() == bZone)
                {
                    if (bNation > 0 && pUser.Second.GetNation() != bNation)
                        continue;

                    pUser.Second.Send(result);
                }
            }
        }

        internal Knights GetClanPtr(short ClanID)
        {
            lock(m_arKnights)
            {
                foreach (Knights knights in m_arKnights)
                {
                    if (knights.GetID() == ClanID)
                        return knights;
                }
            }

            return null;
        }

        internal bool AddUserInGame(short sSid, User user)
        {
            foreach (Pair<short,User,int> pUser in m_arUserList)
            {
                if (pUser.First != sSid)
                    continue;

                pUser.Second = user;
                return true;
            }

            return false;
        }
    }
}