using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOCharp
{
    using static Define;
    public class _KNIGHTS_USER
    {
        public byte byUsed;
        public string strUserName;
        public int nDonatedNP;
        public short sSid = -1;
        public byte Fame;
        public byte Level;
        public short m_sClass;
        public string strMemo;
        public User pSession;
        public DateTime LastLogin;

        public _KNIGHTS_USER() { Initialise(); }
        public void Initialise()
        {
            byUsed = Fame = Level = 0;
            strUserName = strMemo = string.Empty;
            nDonatedNP = 0;
            m_sClass = 0;
            LastLogin = DateTime.Now;
            sSid = -1;
            pSession = null;
        }
    };

    public class Knights
    {
        public short m_sIndex;
        ClanTypeFlag m_ByFlag;			// 1 : Clan, 2 : Knights
        public byte m_byFlag;
        public byte m_byNation;			// nation
        public byte m_byGrade;
        public byte m_byRanking;
        public short m_sMembers;

        public string m_strName;
        public string m_strChief, m_strViceChief_1, m_strViceChief_2, m_strViceChief_3;

        public Int64 m_nMoney;
        public Int16 m_sDomination;
        public Int32 m_nPoints;
        public Int32 m_nClanPointFund; // stored in national point form
        public Int16 m_sMarkVersion, m_sMarkLen;
        public byte[] m_Image = new byte[MAX_KNIGHTS_MARK];
        public Int16 m_sCape;
        public byte m_bCapeR, m_bCapeG, m_bCapeB;
        public Int16 m_sAlliance;
        public byte m_sClanPointMethod;
        public string m_strClanNotice;
        public byte bySiegeFlag;
        public Int16 nLose, nVictory;

        public List<_KNIGHTS_USER> m_arKnightsUser = new List<_KNIGHTS_USER>();
        // public _KNIGHTS_USER[] m_arKnightsUser = new _KNIGHTS_USER[MAX_CLAN_USERS];

        public short GetID() { return m_sIndex; }
        public short GetAllianceID() { return m_sAlliance; }
        public short GetCapeID(Knights pKnights)
        {
            if (pKnights != null)
            {
                if (isInAlliance())
                    return m_sCape = pKnights.m_sCape;
                else
                    return m_sCape;
            }
            return 0;
        }
        public short CapGetCapeID() { return m_sCape; }
        public string GetName() { return m_strName; }
        public byte GetClanPointMethod() { return m_sClanPointMethod; }

        public bool isPromoted() { return m_byFlag >= (byte)ClanTypeFlag.ClanTypePromoted; }
        public bool isInAlliance() { return m_sAlliance > 0; }
        public bool isAllianceLeader() { return GetAllianceID() == GetID(); }

        public Knights()
        {
            m_sIndex = 0;
            m_byFlag = (byte)ClanTypeFlag.ClanTypeNone;
            m_byNation = 0;
            m_byGrade = 5;
            m_byRanking = 0;
            m_sMembers = 1;
            m_nMoney = 0;
            m_sDomination = 0;
            m_nPoints = 0;
            m_nClanPointFund = 0;
            m_sCape = -1;
            m_sAlliance = 0;
            m_sMarkLen = 0;
            m_sMarkVersion = 0;
            m_bCapeR = m_bCapeG = m_bCapeB = 0;
            m_sClanPointMethod = 0;
        }



        internal void OnLoginAlliance(User user)
        {
            throw new NotImplementedException();
        }

        public void OnLogin(User pUser)
        {
            //Packet result = new Packet();
            //
            //bool byUsed = false;
            //int i = 0;
            //// Set the active session for this user
            //foreach (_KNIGHTS_USER p in m_arKnightsUser)
            //{
            //    i++;
            //    if (STRCMP(p.strUserName, pUser.GetName()) == false)
            //        continue;
            //    byUsed = true;
            //
            //    m_arKnightsUser[i - 1].pSession = pUser;
            //    // p.pSession = pUser;
            //
            //    pUser.m_pKnightsUser = p;
            //
            //    break;
            //}
            ///*
            //if (!byUsed && m_arKnightsUser.Count < MAX_CLAN_USERS)
            //{
            //    _KNIGHTS_USER pKnightUser = new _KNIGHTS_USER();
            //
            //    pKnightUser.byUsed = 1;
            //    pKnightUser.nDonatedNP = 0;
            //    pKnightUser.pSession = pUser;
            //    pKnightUser.strUserName = pUser.GetName();
            //
            //    pUser.m_pKnightsUser = pKnightUser;
            //
            //    m_arKnightsUser.Add(pKnightUser);
            //}*/
            //
            //// Send login notice
            //// TODO: Shift this to SERVER_RESOURCE
            //string buffer = string.Format("{0} is online.", pUser.GetName());
            ////pUser.ChatHandler.Construct(ref result, (byte)ChatType.KNIGHTS_CHAT, buffer);
            ////pUser.SendClan(result);
            //
            //// Construct the clan notice packet to send to the logged in player
            //if (m_strClanNotice != String.Empty)
            //{
            //    ConstructClanNoticePacket(ref result);
            //    pUser.Send(result);
            //}
        }

        public void ConstructClanNoticePacket(ref Packet result)
        {
            result = new Packet();
            result.SetByte(WIZ_NOTICE);
            result.DByte();
            result.SetByte(4);		// type
            result.SetByte(1);			// total blocks
            result.SetString("Clan Notice");	// header
            result.SetString(m_strClanNotice);
        }

        internal bool AddUser(string strUserID, GameServerDLG g_pMain, _KNIGHTS_USER pUser)
        {
            if (m_arKnightsUser.Count >= MAX_CLAN_USERS)
                return false;

            _KNIGHTS_USER pKnightUser = new _KNIGHTS_USER();

            pKnightUser.byUsed = 1;
            pKnightUser.nDonatedNP = 0;
            pKnightUser.pSession = null;
            pKnightUser.strUserName = strUserID;
            pKnightUser.Level = pUser.Level;
            pKnightUser.Fame = pUser.Fame;
            pKnightUser.LastLogin = pUser.LastLogin;
            pKnightUser.strMemo = pUser.strMemo;

            m_arKnightsUser.Add(pKnightUser);
            return true;
        }
    }
}
