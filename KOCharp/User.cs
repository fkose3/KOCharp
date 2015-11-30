using System;
using System.Net.Sockets;
using System.Threading;

namespace KOCharp
{
    using static Define;
    using static UserHeader;
    using static LoginHandler;
    using static LetterHandler;
    using static CharacterMoveHandle;
    using Classes.Database;

    public class User
    {
        public GameServerDLG g_pMain;
        public Socket socket;
        public short sSid = -1;

        public User(Socket soc, GameServerDLG g_pMain)
        {
            this.socket = soc;
            this.g_pMain = g_pMain;

            Initialize();
            ReceiveUser();
        }

        #region USER DATA
        #region Getter
        public bool isMastered() {
            short sClass = GetClassType();
            return (sClass == (short)ClassType.ClassWarriorMaster || sClass == (short)ClassType.ClassRogueMaster
                || sClass == (short)ClassType.ClassMageMaster || sClass == (short)ClassType.ClassPriestMaster);
        }
        public bool isBlockingpublicChat() { return m_bBlockpublicChat; }
        public bool isGenieActive() { return m_bGenieStatus; } // Genie iþlemleri yapýlýnca kaldýrýlacak.
        public short GetAchieveTitle() { return m_sAchieveCoverTitle; } // Achieve Title gönderilecek.
        public short GetAchieveSkillTitle() { return m_sAchieveSkillTitle; } // Achieve Title gönderilecek.

        public short GetID() { return sSid; }

        public byte isRebirth()
        {
            return (byte)((m_brStats[0] + m_brStats[1] + m_brStats[2] + m_brStats[3] + m_brStats[4]) / 2);
        }
        public byte GetZoneID() { return m_bZone; }
        public byte GetFame() { return m_bFame; }
        public bool isClanLeader() { return GetFame() == 1; }
        public bool isInClan() { return m_bKnights > 0; }

        public bool isMining() { return m_bMining; }

        public bool isTrading() { return m_sExchangeUser != -1; }

        public sbyte GetMerchantState() { return m_bMerchantState; }
        public bool isMerchanting() { return GetMerchantState() != Convert.ToSByte(MerchantState.MERCHANT_STATE_NONE); }
        public bool isSellingMerchant() { return GetMerchantState() == Convert.ToByte(MerchantState.MERCHANT_STATE_SELLING); }
        public bool isBuyingMerchant() { return GetMerchantState() == Convert.ToByte(MerchantState.MERCHANT_STATE_BUYING); }

        public short GetClanID() { return m_bKnights; }
        public short GetMaxHealth() { return m_iMaxHp; }
        public short GetHealth() { return m_sHp; }

        public virtual bool isDead() { return m_bResHpType == USER_DEAD || m_sHp <= 0; }
        public virtual bool isBlinking() { return m_bAbnormalType == (int)AbnormalType.ABNORMAL_BLINKING; }

        public bool isInGame() { return bGameStart; }
        public bool isDisconnect() { return m_State == GameState.GAME_STATE_DISCONNECT ? true : false; }
        public short GetEventRoom() { return m_sEventRoom; }

        public float GetX() { return m_curx; }
        public float GetY() { return m_cury; }
        public float GetZ() { return m_curz; }

        public short GetSPosX() { return Convert.ToInt16(GetX() * 10); }
        public short GetSPosY() { return Convert.ToInt16(GetY() * 10); }
        public short GetSPosZ() { return Convert.ToInt16(GetZ() * 10); }

        public short GetRegionX() { return m_sRegionX; }
        public short GetRegionZ() { return m_sRegionZ; }

        public short GetNewRegionX() { return m_sNewRegionX; }
        public short GetNewRegionZ() { return m_sNewRegionZ; }

        public short GetSocketID() { return sSid; }

        public short GetPartyID() { return m_sPartyIndex; }
        public bool isInParty() { return m_bInParty; }

        public bool isGM() { return m_bAuthority == 0; }
        public bool isKing() { return m_bRank == 1; }

        public short GetClass() { return m_sClass; }

        public bool isWarrior() { return JobGroupCheck((short)ClassType.ClassWarrior); }
        public bool isRogue() { return JobGroupCheck((short)ClassType.ClassRogue); }
        public bool isMage() { return JobGroupCheck((short)ClassType.ClassMage); }
        public bool isPriest() { return JobGroupCheck((short)ClassType.ClassPriest); }

        public bool JobGroupCheck(short jobgroupid)
        {
            if ((short)jobgroupid > 100)
                return GetClass() == (short)jobgroupid;

            return true;
            ClassType subClass = GetBaseClassType();
            switch ((byte)jobgroupid)
            {
                case GROUP_WARRIOR:
                    return (subClass == ClassType.ClassWarrior || subClass == ClassType.ClassWarriorNovice || subClass == ClassType.ClassWarriorMaster);

                case GROUP_ROGUE:
                    return (subClass == ClassType.ClassRogue || subClass == ClassType.ClassRogueNovice || subClass == ClassType.ClassRogueMaster);

                case GROUP_MAGE:
                    return (subClass == ClassType.ClassMage || subClass == ClassType.ClassMageNovice || subClass == ClassType.ClassMageMaster);

                case GROUP_CLERIC:
                    return (subClass == ClassType.ClassPriest || subClass == ClassType.ClassPriestNovice || subClass == ClassType.ClassPriestMaster);
            }

            return (Convert.ToInt16(subClass) == jobgroupid);
        }

        public ClassType GetBaseClassType()
        {
            ClassType[] classTypes =
            {
                ClassType.ClassWarrior, ClassType.ClassRogue, ClassType.ClassMage, ClassType.ClassPriest,
                ClassType.ClassWarrior, ClassType.ClassWarrior,	// job changed / mastered
		    	ClassType.ClassRogue, ClassType.ClassRogue,		// job changed / mastered
		    	ClassType.ClassMage, ClassType.ClassMage,		// job changed / mastered
		    	ClassType.ClassPriest, ClassType.ClassPriest	// job changed / mastered
		    };

            byte classType = GetClassType();
            return classTypes[classType - 1];
        }

        public byte GetClassType()
        {
            return (byte)(GetClass() - (GetNation() * 100));
        }

        public short GetStatWithItemBonus(StatType type)
        {
            return (short)(GetStat(type) + GetStatItemBonus(type));
        }

        public byte GetLevel()
        {
            return m_bLevel;
        }

        public short GetStatBonusTotal(StatType type)
        {
            return (short)(GetStatBuff(type) + GetStatItemBonus(type));
        }

        public _ITEM_TABLE GetItemPrototype(byte pos)
        {
            if (pos >= INVENTORY_TOTAL)
                return null;

            _ITEM_DATA pItem = GetItem(pos);

            if (pItem == null)
                return null;

            return pItem.nNum == 0 ? null : g_pMain.GetItemPtr(pItem.nNum);
        }

        public _ITEM_DATA GetItem(int i)
        {
            return m_sItemArray[i];
        }

        public byte GetStat(StatType statType)
        {
            if (statType >= StatType.STAT_COUNT)
                return 0;

            return m_bStats[(byte)statType];
        }

        public int getStatTotal(StatType type)
        {
            return GetStat(type) + GetStatItemBonus(type) + GetStatBuff(type);
        }

        public short GetStatBuff(StatType type)
        {
            return m_bStatBuffs[(byte)type];
        }

        public short GetStatItemBonus(StatType type)
        {
            return m_sStatItemBonuses[(byte)type];
        }

        #endregion
        #region Enums
        public enum UserStatusBehaviour
        {
            USER_STATUS_CURE = 0,
            USER_STATUS_INFLICT = 1
        };

        public enum GameState
        {
            GAME_STATE_CONNECTED,
            GAME_STATE_INGAME,
            GAME_STATE_DISCONNECT
        };

        #endregion
        #region Data
        public byte m_TimeQuest;
        public short m_iMaxSp;
        public GameState m_State;
        public int m_iTotalTrainingExp, m_lastTrainingTime, m_tLastMiningAttempt;
        public string m_spublicChatUser = string.Empty;
        public bool m_bBlockpublicChat = true, m_bStoreOpen, m_bIsHidingCospre, m_bPremiumMerchant;

        public byte m_teamColour;

        public bool m_bGenieStatus;
        public short m_sGenieTime;
        public byte[] GenieData = new byte[100];

        bool bExpsealStatus;
        long m_iSealedExp;

        // public _ACHIEVE_KILL_DATA[] pKillData = new _ACHIEVE_KILL_DATA[ACHIEVE_MAX];
        public short[] m_sAchieveEndIndex = new short[3];

        public int m_sAchieveMonsterKillCount;
        public int m_sAchieveUserDeadCount;
        public int m_sAchieveUserDefeatedCount;
        public short m_sAchieveNormalComlateCount;
        public short m_sAchieveQuestComplateCount;
        public short m_sAchieveWarComplateCount;
        public short m_sAchieveAdventureComplateCount;
        public short m_sAchieveChallangeComplateCount;
        public int m_iAchieveRankPoint;
        public int m_tAchieveGameStartTime;

        public int m_tGameStartTimeSavedMagic;

        public short m_sAchieveCoverTitle, m_sAchieveSkillTitle;

        public string m_strMemo;

        public float m_tBlinkExpiryTime;

        public _KNIGHTS_USER m_pKnightsUser;

        public sbyte m_bMerchantState;

        public float m_tLastSaveTime;

        public short m_sEventRoom;

        public short m_sRegionX;
        public short m_sRegionZ;
        public short m_sNewRegionX;
        public short m_sNewRegionZ;

        public short m_sACAmount;   // additional absolute AC
        public short m_sACPercent;  // percentage of total AC to modify by
        public byte m_bAttackAmount;
        public short m_sMagicAttackAmount;
        public short m_sMaxHPAmount, m_sMaxMPAmount;
        public byte m_bHitRateAmount;
        public short m_sAvoidRateAmount;

        public byte m_bResistanceBonus;
        public bool isWeaponsDisabled() { return m_bWeaponsDisabled; }
        public short m_sTotalHit;
        public short m_sTotalAc;
        public float m_fTotalHitrate;
        public float m_fTotalEvasionrate;
        // Weapon resistances
        public short m_sDaggerR;
        public byte m_byDaggerRAmount;
        public short m_sSwordR;
        public short m_sAxeR;
        public short m_sMaceR;
        public short m_sSpearR;
        public short m_sBowR;
        public byte m_byBowRAmount;
        // Item calculated elemental resistances.
        public short m_sFireR, m_sColdR, m_sLightningR,
           m_sMagicR, m_sDiseaseR, m_sPoisonR;

        // Additional elemental resistance amounts from skills (note: NOT percentages)
        public byte m_bAddFireR, m_bAddColdR, m_bAddLightningR,
           m_bAddMagicR, m_bAddDiseaseR, m_bAddPoisonR;

        // Elemental resistance percentages (adjusted by debuffs)
        public byte m_bPctFireR, m_bPctColdR, m_bPctLightningR,
           m_bPctMagicR, m_bPctDiseaseR, m_bPctPoisonR;

        // public _ITEM_DATA GetItem(int i)
        //{
        //    return m_sItemArray[i];
        //}
        public static bool bSelectChar = false;
        public static bool bGameStart = false;
        public static bool IsInGame()
        {
            return bGameStart;
        }

        public short nQuestCount = 0;

        public float m_curx, m_curz, m_cury;
        public byte m_bZone;
        public byte m_bLevel;
        public byte m_bNation;
        public byte m_bRace;
        public short m_sClass;

        // public short m_sTotalHit, m_sTotalAc;

        public int m_nHair;

        public byte m_bRank;
        public byte m_bTitle;
        public Int64 m_iExp;
        public Int32 m_iLoyalty, m_iLoyaltyMonthly;
        public Int32 m_iMannerPoint;
        public byte m_bFace;
        public sbyte m_bCity;
        public Int16 m_bKnights;
        public byte m_bFame;
        public Int16 m_sHp, m_sMp, m_sSp;
        public byte[] m_bStats = new byte[(int)StatType.STAT_COUNT];
        public byte[] m_brStats = new byte[(int)StatType.STAT_COUNT];
        public byte m_bAuthority;
        public Int16 m_sPoints; // this is just to shut the compiler up
        public Int32 m_iGold, m_iBank;
        public Int64 m_sBind;
        public bool m_bIsChicken; // Is the character taking the beginner/chicken quest?
        public bool m_bIsHidingHelmet;

        public bool m_bMining = false;
        //time_t	m_tLastMiningAttempt;

        public byte m_bPersonalRank;
        public byte m_bKnightsRank;

        public float m_oldx, m_oldy, m_oldz;
        public Int16 m_sDirection;

        public Int64 m_iMaxExp;

        public short m_sMaxWeight;
        public short m_sMaxWeightBonus;
        public Int16 m_sSpeed;

        public byte m_bPlayerAttackAmount;
        public byte m_bAddWeaponDamage;
        public short m_sAddArmourAc;
        public byte m_bPctArmourAc;

        public Int16 m_sItemMaxHp;
        public Int16 m_sItemMaxMp;
        public short m_sItemWeight;
        public short m_sItemAc;
        public short m_sItemHitrate;
        public short m_sItemEvasionrate;

        public byte m_byAPBonusAmount;
        public byte[] m_byAPClassBonusAmount = new byte[4]; // one for each of the 4 class types
        public byte[] m_byAcClassBonusAmount = new byte[4]; // one for each of the 4 class types

        public Int16[] m_sStatItemBonuses = new Int16[(int)StatType.STAT_COUNT];
        public byte[] m_bStatBuffs = new byte[(int)StatType.STAT_COUNT];

        public short m_sExpGainAmount;
        public byte m_bItemExpGainAmount;
        public byte m_bNPGainAmount, m_bItemNPBonus, m_bSkillNPBonus;
        public byte m_bNoahGainAmount, m_bItemNoahGainAmount;
        public byte m_bMaxWeightAmount;

        public short m_iMaxHp, m_iMaxMp;

        public byte m_bResHpType;
        public bool m_bWarp;
        public byte m_bNeedParty;

        public short m_sPartyIndex;
        public bool m_bInParty;
        public bool m_bPartyLeader;

        public bool m_bCanSeeStealth;
        public byte m_bInvisibilityType;

        public short m_sExchangeUser;
        public byte m_bExchangeOK;

        //ItemList	m_ExchangeItemList;
        

        int m_tHPLastTimeNormal;					// For Automatic HP recovery. 
        int m_tHPStartTimeNormal;
        public short m_bHPAmountNormal;
        public byte m_bHPDurationNormal;
        public byte m_bHPIntervalNormal;

        //time_t m_tGameStartTimeSavedMagic;

        public Int32 m_fSpeedHackClientTime, m_fSpeedHackServerTime;
        public byte m_bSpeedHackCheck;

        //int	m_tBlinkExpiryTime;			// When you should stop blinking.

        public Int32 m_bAbnormalType;           // Is the player normal, a giant, or a dwarf?
        public Int32 m_nOldAbnormalType;

        public Int16 m_sWhoKilledMe;                // ID of the unit that killed you.
        public Int64 m_iLostExp;                    // Experience points that were lost when you died.

        int m_tLastTrapAreaTime;		// The last moment you were in the trap area.

        public bool m_bZoneChangeFlag;

        public byte m_bRegeneType;              // Did you die and go home or did you type '/town'?

        int m_tLastRegeneTime;			// The last moment you got resurrected.

        public bool m_bZoneChangeSameZone;      // Did the server change when you warped?

        //int		m_iSelMsgEvent[MAX_MESSAGE_EVENT];
        public short m_sEventNid, m_sEventSid;
        public Int32 m_nQuestHelperID;

        public bool m_bWeaponsDisabled;

        //TeamColour	m_teamColour;
        public Int32 m_iLoyaltyDaily;
        public short m_iLoyaltyPremiumBonus;
        public short m_KillCount;
        public short m_DeathCount;

        public float m_LastX;
        public float m_LastZ;

        public char[] m_bstrSkill = new char[10];
        public _ITEM_DATA[] m_sItemArray = new _ITEM_DATA[INVENTORY_TOTAL];
        public _ITEM_DATA[] m_sWarehouseArray = new _ITEM_DATA[WAREHOUSE_MAX];

        public byte m_bLogout;
        public Int32 m_dwTime;
        public float m_lastSaveTime;

        public byte m_bAccountStatus;
        public byte m_bPremiumType;
        public short m_sPremiumTime;
        public Int32 m_nKnightCash;

        public byte GetNation() { return m_bNation; }
        public string strAccountID;
        internal string GetAccountID()
        {
            return strAccountID;
        }
        public string strCharID;
        internal string GetName()
        {
            return strCharID;
        }
        #endregion
        #endregion

        #region NetworkListen
        public void ReceiveUser()
        {
            Thread ThreadListener = new Thread(new ThreadStart(delegate { OnClientDataReceived(); }));
            ThreadListener.Start();

            while (socket.Connected)
            {
                Thread.Sleep(45);
            }

           // OnDisconnect();
        }

        public void OnDisconnect()
        {
            if (socket.Connected)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                Thread.CurrentThread.Abort(0);
            }

            m_State = GameState.GAME_STATE_DISCONNECT;
        }

        public void OnClientDataReceived()
        {
            try {
                int bytes = 0;

                do
                {
                    byte[] read_byte = new byte[1024 * 8];

                    bytes = socket.Receive(read_byte, 1024 * 8, 0);

                    Packet pkt = new Packet(read_byte, socket);

                    Parsing(pkt);

                }
                while (true);
            }
            catch
            {

            }
        }


        public void Send(Packet resut)
        {
            resut.Send(socket);
        }

        public void SendToRegion(Packet result, User pExceptUser = null, short m_sEventRoom = -1)
        {
            g_pMain.Send_Region(result, GetRegionX(), GetRegionZ(), pExceptUser, m_sEventRoom);
        }

        #endregion


        public void Initialize()
        {
            //Unit::Initialize();

            strCharID = string.Empty;
            m_strMemo = string.Empty;
            strAccountID = string.Empty;
            m_bLogout = 0;

            m_iTotalTrainingExp = 0;
            m_lastTrainingTime = 0;


            m_iMaxSp = 100;
            m_sSp = 100;

            m_bAuthority = 1;
            m_sBind = -1;
            m_TimeQuest = 1;
            m_State = GameState.GAME_STATE_CONNECTED;

            bSelectChar = false;
            m_bStoreOpen = false;
            m_bPartyLeader = false;
            m_bIsChicken = false;
            m_bIsHidingHelmet = false;
            m_bIsHidingCospre = false;
            m_bGenieStatus = false;

            m_bMining = false;
            m_bPremiumMerchant = false;
            m_bInParty = false;

            //m_JrEventJoin = false;

            //for (int i = 0; i < ACHIEVE_MAX; i++)
            //{
            //    pKillData[i] = new _ACHIEVE_KILL_DATA();
            //}

            for (int i = 0; i < 3; i++)
                m_sAchieveEndIndex[0] = 0;

            m_tLastMiningAttempt = 0;

            m_bMerchantState = MERCHANT_STATE_NONE;
            m_bInvisibilityType = (byte)InvisibilityType.INVIS_NONE;

            m_sDirection = 0;

            m_sItemMaxHp = m_sItemMaxMp = 0;
            m_sItemWeight = 0;
            m_sItemAc = 0;

            m_sExpGainAmount = m_bNPGainAmount = m_bNoahGainAmount = 100;
            m_bItemExpGainAmount = m_bItemNoahGainAmount = 0;
            m_bItemNPBonus = m_bSkillNPBonus = 0;

            m_byAPBonusAmount = 0;

            Array.Clear(m_byAPClassBonusAmount, 0, m_byAPClassBonusAmount.Length);
            Array.Clear(m_byAcClassBonusAmount, 0, m_byAcClassBonusAmount.Length);

            Array.Clear(m_bStats, 0, m_bStats.Length);
            // memset(m_bAchieveStats, 0, m_bStats.Length);
            Array.Clear(m_brStats, 0, m_brStats.Length);
            Array.Clear(m_sStatItemBonuses, 0, m_sStatItemBonuses.Length);
            Array.Clear(m_bStatBuffs, 0, m_bStatBuffs.Length);
            Array.Clear(m_bstrSkill, 0, m_bstrSkill.Length);

            m_bPlayerAttackAmount = 100;

            m_bAddWeaponDamage = 0;
            m_bPctArmourAc = 100;
            m_sAddArmourAc = 0;

            m_sItemHitrate = 100;
            m_sItemEvasionrate = 100;

            m_sSpeed = 0;

            m_bAuthority = 1;
            m_bLevel = 1;
            m_iExp = 0;
            m_iBank = m_iGold = 0;
            m_iLoyalty = m_iLoyaltyMonthly = 0;
            m_iMannerPoint = 0;
            m_sHp = m_sMp = m_sSp = 0;

            m_iMaxHp = 0;
            m_iMaxMp = 1;
            m_iMaxExp = 0;
            m_sMaxWeight = 0;
            m_sMaxWeightBonus = 0;

            m_bResHpType = USER_STANDING;
            m_bWarp = false;

            //m_sMerchantsSocketID = -1;
            //m_sChallengeUser = -1;
            m_sPartyIndex = -1;
            m_sExchangeUser = -1;
            //m_bRequestingChallenge = 0;
            //m_bChallengeRequested = 0;
            m_bExchangeOK = 0x00;
            m_bBlockpublicChat = false;
            m_spublicChatUser = string.Empty;
            m_bNeedParty = 0x01;

            m_tHPLastTimeNormal = 0;		// For Automatic HP recovery. 
            m_tHPStartTimeNormal = 0;
            m_bHPAmountNormal = 0;
            m_bHPDurationNormal = 0;
            m_bHPIntervalNormal = 5;

            //m_tGameStartTimeSavedMagic = 0;

            m_tAchieveGameStartTime = 0; // achieve
            m_sAchieveNormalComlateCount = 0;
            m_sAchieveQuestComplateCount = 0;
            m_sAchieveWarComplateCount = 0;
            m_sAchieveAdventureComplateCount = 0;
            m_sAchieveChallangeComplateCount = 0;
            m_iAchieveRankPoint = 0;

            m_fSpeedHackClientTime = 0;
            m_fSpeedHackServerTime = 0;
            m_bSpeedHackCheck = 0;

            m_tBlinkExpiryTime = 0;

            m_bAbnormalType = ABNORMAL_NORMAL;	// User starts out in normal size.
            m_nOldAbnormalType = m_bAbnormalType;

            m_sWhoKilledMe = -1;
            m_iLostExp = 0;

            m_tLastTrapAreaTime = 0;

            // Array.Clear(m_iSelMsgEvent, 0, 12);

            m_sEventNid = m_sEventSid = -1;
            m_nQuestHelperID = 0;
            m_bZoneChangeFlag = false;
            m_bRegeneType = 0;
            m_tLastRegeneTime = 0;
            m_bZoneChangeSameZone = false;

            //m_transformationType = TransformationNone;
            //m_sTransformID = 0;
            //m_tTransformationStartTime = 0;
            //m_sTransformationDuration = 0;

            //Array.Clear(m_bKillCounts, 0, sizeof(m_bKillCounts));
            //m_sEventDataIndex = 0;

            m_pKnightsUser = null;

            //m_sRivalID = -1;
            //m_tRivalExpiryTime = 0;
            //
            //m_byAngerGauge = 0;

            m_bWeaponsDisabled = false;

            m_teamColour = 0;
            m_iLoyaltyDaily = 0;
            m_iLoyaltyPremiumBonus = 0;
            m_KillCount = 0;
            m_DeathCount = 0;

            m_sAchieveMonsterKillCount = 0;
            m_sAchieveUserDeadCount = 0;
            m_sAchieveUserDefeatedCount = 0;

            m_LastX = 0;
            m_LastZ = 0;
        }

        public void Parsing(Packet pkt)
        {
            byte command = pkt.GetByte();
            Console.WriteLine("Client Received : 0x" + command.ToString("X2"));
            if (!bSelectChar)
            {
                switch (command)
                {
                    case WIZ_VERSION_CHECK:
                        VersionCheck(this);
                        break;
                    case WIZ_LOGIN:
                        AccountLogin(this, pkt);
                        break;
                    case WIZ_ALLCHAR_INFO_REQ:
                        LoadCharInfoReq(this, pkt);
                        break;
                    case WIZ_SEL_NATION:
                        SelectNation(this, pkt);
                        break;
                    case WIZ_SEL_CHAR:
                        SelCharToAgent(this, pkt);
                        break;
                    case WIZ_NEW_CHAR:
                        CreateNewChar(this, pkt);
                        break;
                    case WIZ_LOGINCHECKFORV2:
                        {
                            Packet result = new Packet(WIZ_LOGINCHECKFORV2);
                            result.SetByte(1);
                            Send(result);
                        }
                        break;
                }
            }
            else
            {
                switch (command)
                {
                    case WIZ_MOVE: break;
                    case WIZ_USER_INOUT: break;
                    case WIZ_ATTACK: break;
                    case WIZ_ROTATE: break;
                    case WIZ_NPC_INOUT: break;
                    case WIZ_NPC_MOVE: break;
                    case WIZ_ALLCHAR_INFO_REQ: break;
                    case WIZ_GAMESTART:
                        GameStart(pkt);
                        break;
                    case WIZ_MYINFO: break;
                    case WIZ_LOGOUT: break;
                    case WIZ_CHAT: break;
                    case WIZ_DEAD: break;
                    case WIZ_REGENE: break;
                    case WIZ_TIME: break;
                    case WIZ_WEATHER: break;
                    case WIZ_REGIONCHANGE: break;
                    case WIZ_REQ_USERIN: break;
                    case WIZ_HP_CHANGE: break;
                    case WIZ_MSP_CHANGE: break;
                    case WIZ_ITEM_LOG: break;
                    case WIZ_EXP_CHANGE: break;
                    case WIZ_LEVEL_CHANGE: break;
                    case WIZ_NPC_REGION: break;
                    case WIZ_REQ_NPCIN: break;
                    case WIZ_WARP: break;
                    case WIZ_ITEM_MOVE: break;
                    case WIZ_NPC_EVENT: break;
                    case WIZ_ITEM_TRADE: break;
                    case WIZ_TARGET_HP: break;
                    case WIZ_ITEM_DROP: break;
                    case WIZ_BUNDLE_OPEN_REQ: break;
                    case WIZ_TRADE_NPC: break;
                    case WIZ_ITEM_GET: break;
                    case WIZ_ZONE_CHANGE: break;
                    case WIZ_POINT_CHANGE: break;
                    case WIZ_STATE_CHANGE: break;
                    case WIZ_LOYALTY_CHANGE: break;
                    case WIZ_CRYPTION: break;
                    case WIZ_USERLOOK_CHANGE: break;
                    case WIZ_NOTICE: break;
                    case WIZ_PARTY: break;
                    case WIZ_EXCHANGE: break;
                    case WIZ_MAGIC_PROCESS: break;
                    case WIZ_SKILLPT_CHANGE: break;
                    case WIZ_OBJECT_EVENT: break;
                    case WIZ_CLASS_CHANGE: break;
                    case WIZ_CHAT_TARGET: break;
                    case WIZ_CONCURRENTUSER: break;
                    case WIZ_DATASAVE: break;
                    case WIZ_DURATION: break;
                    case WIZ_TIMENOTIFY: break;
                    case WIZ_REPAIR_NPC: break;
                    case WIZ_ITEM_REPAIR: break;
                    case WIZ_KNIGHTS_PROCESS: break;
                    case WIZ_ITEM_COUNT_CHANGE: break;
                    case WIZ_KNIGHTS_LIST: break;
                    case WIZ_ITEM_REMOVE: break;
                    case WIZ_OPERATOR: break;
                    case WIZ_SPEEDHACK_CHECK: break;
                    case WIZ_COMPRESS_PACKET: break;
                    case WIZ_SERVER_CHECK: break;
                    case WIZ_CONTINOUS_PACKET: break;
                    case WIZ_WAREHOUSE: break;
                    case WIZ_SERVER_CHANGE: break;
                    case WIZ_REPORT_BUG: break;
                    case WIZ_HOME: break;
                    case WIZ_FRIEND_PROCESS: break;
                    case WIZ_GOLD_CHANGE: break;
                    case WIZ_WARP_LIST: break;
                    case WIZ_VIRTUAL_SERVER: break;
                    case WIZ_ZONE_CONCURRENT: break;
                    case WIZ_CORPSE: break;
                    case WIZ_PARTY_BBS: break;
                    case WIZ_MARKET_BBS: break;
                    case WIZ_KICKOUT: break;
                    case WIZ_CLIENT_EVENT: break;
                    case WIZ_MAP_EVENT: break;
                    case WIZ_WEIGHT_CHANGE: break;
                    case WIZ_SELECT_MSG: break;
                    case WIZ_NPC_SAY: break;
                    case WIZ_BATTLE_EVENT: break;
                    case WIZ_AUTHORITY_CHANGE: break;
                    case WIZ_EDIT_BOX: break;
                    case WIZ_SANTA: break;
                    case WIZ_ITEM_UPGRADE: break;
                    case WIZ_PACKET1: break;
                    case WIZ_PACKET2: break;
                    case WIZ_ZONEABILITY: break;
                    case WIZ_EVENT: break;
                    case WIZ_STEALTH: break;
                    case WIZ_ROOM_PACKETPROCESS: break;
                    case WIZ_ROOM: break;
                    case WIZ_PACKET3: break;
                    case WIZ_QUEST: break;
                    case WIZ_PACKET4: break;
                    case WIZ_KISS: break;
                    case WIZ_RECOMMEND_USER: break;
                    case WIZ_MERCHANT: break;
                    case WIZ_MERCHANT_INOUT: break;
                    case WIZ_SHOPPING_MALL:
                        HandleShoppingMall(pkt, this);
                        break;
                    case WIZ_SERVER_INDEX:
                        ServerIndex(pkt);
                        break;
                    case WIZ_EFFECT: break;
                    case WIZ_SIEGE: break;
                    case WIZ_NAME_CHANGE: break;
                    case WIZ_WEBPAGE: break;
                    case WIZ_CAPE: break;
                    case WIZ_PREMIUM: break;
                    case WIZ_HACKTOOL: break;
                    case WIZ_RENTAL:
                        Packet result = new Packet(20);
                        result.SetByte(WIZ_RENTAL);
                        result.SetByte(1);
                        Send(result);
                        break;
                    case WIZ_PACKET5: break;
                    case WIZ_CHALLENGE: break;
                    case WIZ_PET: break;
                    case WIZ_CHINA: break;
                    case WIZ_KING: break;
                    case WIZ_SKILLDATA: break;
                    case WIZ_PROGRAMCHECK: break;
                    case WIZ_BIFROST: break;
                    case WIZ_REPORT: break;
                    case WIZ_LOGOSSHOUT: break;
                    case WIZ_PACKET6: break;
                    case WIZ_PACKET7: break;
                    case WIZ_RANK: break;
                    case WIZ_STORY: break;
                    case WIZ_PACKET8: break;
                    case WIZ_PACKET9: break;
                    case WIZ_PACKET10: break;
                    case WIZ_PACKET11: break;
                    case WIZ_MINING: break;
                    case WIZ_HELMET: break;
                    case WIZ_PVP: break;
                    case WIZ_CHANGE_HAIR: break;
                    case WIZ_PACKET12: break;
                    case WIZ_PACKET13: break;
                    case WIZ_PACKET14: break;
                    case WIZ_PACKET15: break;
                    case WIZ_PACKET16: break;
                    case WIZ_PACKET17: break;
                    case WIZ_DEATH_LIST: break;
                    case WIZ_CLANPOINTS_BATTLE: break;
                    case WIZ_TEST_PACKET: break;
                    case WIZ_GENIE: break;
                    case WIZ_USER_INFORMATIN: break;
                    case WIZ_ACHIEVE: break;
                    case WIZ_EXP_SEAL: break;
                    case WIZ_SP_CHANGE: break;
                }
            }
        }

        public void GameStart(Packet pkt)
        {
            if (IsInGame())
                return;

            byte opcode = pkt.GetByte();
            Console.WriteLine("Game Start Opcode : " + opcode);
            if (opcode == 1)
            {
                SendMyInfo();

                g_pMain.UserInOutForMe(this);
                g_pMain.MerchantUserInOutForMe(this);
                g_pMain.NpcInOutForMe(this);

                SendNotice();
                TopSendNotice();
                SendTime();
                SendWeather();

                Packet result = new Packet(2);
                result.SetByte(WIZ_GAMESTART);
                Send(result);

                m_State = GameState.GAME_STATE_INGAME;
                UserInOut(InOutType.INOUT_RESPAWN, this);
                bGameStart = true;

                SetUserAbility();

                if (isDead())
                    SendDeathAnimation();

                // m_tGameStartTimeSavedMagic = UNIXTIME;

                m_LastX = GetX();
                m_LastZ = GetZ();
            }

            BlinkStart();

            m_tHPLastTimeNormal = UNIXTIME;
        }

        public void SendDeathAnimation()
        {
            Packet result = new Packet(WIZ_DEAD);
            result.SetShort(GetID());
            SendToRegion(result);
        }

        public void SetUserAbility(bool bSendPacket = true)
        {
            bool bHaveBow = false;
            COEFFICIENT p_TableCoefficient = g_pMain.GetCoefficientData(m_sClass);

            short sItemDamage = 0;

            float hitcoefficient = 0.0f;

            if (!isWeaponsDisabled())
            {
                _ITEM_TABLE pRightHand = GetItemPrototype(RIGHTHAND);
                _ITEM_DATA pItem = GetItem(RIGHTHAND);

                if (pRightHand != null && pRightHand.m_iNum != 0)
                {
                    switch (pRightHand.m_bKind / 10)
                    {
                        case WEAPON_DAGGER:
                            hitcoefficient = (float)p_TableCoefficient.ShortSword;
                            break;
                        case WEAPON_SWORD:
                            hitcoefficient = (float)p_TableCoefficient.Sword;
                            break;
                        case WEAPON_AXE:
                            hitcoefficient = (float)p_TableCoefficient.Axe;
                            break;
                        case WEAPON_MACE:
                        case WEAPON_MACE2:
                            hitcoefficient = (float)p_TableCoefficient.Club;
                            break;
                        case WEAPON_SPEAR:
                            hitcoefficient = (float)p_TableCoefficient.Spear;
                            break;
                        case WEAPON_BOW:
                        case WEAPON_LONGBOW:
                        case WEAPON_LAUNCHER:
                            hitcoefficient = (float)p_TableCoefficient.Bow;
                            bHaveBow = true;
                            break;
                        case WEAPON_STAFF:
                            hitcoefficient = (float)p_TableCoefficient.Staff;
                            break;
                    }

                    if (pRightHand.m_sDuration == 0)
                        sItemDamage += (short)((pRightHand.m_sDamage + m_bAddWeaponDamage) / 2);
                    else
                        sItemDamage += (short)(pRightHand.m_sDamage + m_bAddWeaponDamage);
                }

                _ITEM_TABLE pLeftHand = GetItemPrototype(LEFTHAND);
                _ITEM_DATA pLeftData = GetItem(LEFTHAND);
                if (pLeftHand != null && pRightHand.m_iNum != 0)
                {
                    if (pLeftHand.isBow())
                    {
                        hitcoefficient = (float)p_TableCoefficient.Bow;
                        bHaveBow = true;
                        if (pLeftData.sDuration == 0)
                            sItemDamage = (short)((pLeftHand.m_sDamage + m_bAddWeaponDamage) / 2);
                        else
                            sItemDamage = (short)(pLeftHand.m_sDamage + m_bAddWeaponDamage);
                    }
                    else
                    {
                        if (pLeftData.sDuration == 0)
                            sItemDamage += (short)(((pLeftHand.m_sDamage + m_bAddWeaponDamage) / 2) / 2);
                        else
                            sItemDamage += (short)((pLeftHand.m_sDamage + m_bAddWeaponDamage) / 2);
                    }
                }
            }

            if (m_sACAmount < 0)
                m_sACAmount = 0;

            m_sTotalHit = 0;

            if (sItemDamage < 3)
                sItemDamage = 3;

            // Update stats based on item data
            SetSlotItemValue();

            int temp_str = GetStat(StatType.STAT_STR), temp_dex = getStatTotal(StatType.STAT_DEX);

            int baseAP = 0, ap_stat = 0, additionalAP = 3;
            if (temp_str > 150)
                baseAP = temp_str - 150;

            if (temp_str == 160)
                baseAP--;

            temp_str += GetStatBonusTotal(StatType.STAT_STR);

            m_sMaxWeight = (short)((((GetStatWithItemBonus(StatType.STAT_STR) + GetLevel()) * 50) + m_sMaxWeightBonus) * (m_bMaxWeightAmount <= 0 ? 1 : m_bMaxWeightAmount / 100));

            if (isRogue())
            {
                ap_stat = temp_dex;
            }
            else
            {
                ap_stat = temp_str;
                additionalAP += baseAP;
            }

            if (isWarrior() || isPriest())
            {
                m_sTotalHit = (short)((0.010f * sItemDamage * (ap_stat + 40)) + (hitcoefficient * sItemDamage * GetLevel() * ap_stat));
                m_sTotalHit = (short)((m_sTotalHit + additionalAP) * (100 + m_byAPBonusAmount) / 100);
            }
            if (isRogue())
            {
                m_sTotalHit = (short)((0.007f * sItemDamage * (ap_stat + 40)) + (hitcoefficient * sItemDamage * GetLevel() * ap_stat));
                m_sTotalHit = (short)((m_sTotalHit + additionalAP) * (100 + m_byAPBonusAmount) / 100);
            }
            else if (isMage())
            {
                m_sTotalHit = (short)((0.005f * sItemDamage * (ap_stat + 40)) + (hitcoefficient * sItemDamage * GetLevel()));
                m_sTotalHit = (short)((m_sTotalHit + additionalAP) * (100 + m_byAPBonusAmount) / 100);
            }

            m_sTotalAc = (short)(p_TableCoefficient.Ac * (GetLevel() + m_sItemAc));
            if (m_sACPercent <= 0)
                m_sACPercent = 100;

            m_sTotalAc = (short)(m_sTotalAc * m_sACPercent / 100);

            m_fTotalHitrate = (float)((1 + p_TableCoefficient.Hitrate * GetLevel() * temp_dex) * m_sItemHitrate / 100) * (m_bHitRateAmount / 100);

            m_fTotalEvasionrate = (float)((1 + p_TableCoefficient.Evasionrate * GetLevel() * temp_dex) * m_sItemEvasionrate / 100) * (m_sAvoidRateAmount / 100);

            SetMaxHp();
            SetMaxMp();

            if (m_bAddWeaponDamage > 0)
                ++m_sTotalHit;

            if (m_sAddArmourAc > 0 || m_bPctArmourAc > 100)
                ++m_sTotalAc;

            byte bSta = GetStat(StatType.STAT_STA);
            if (bSta > 100)
            {
                m_sTotalAc += (short)(bSta - 100);
                // m_sTotalAcUnk += (bSta - 100) / 3;
            }

            byte bInt = GetStat(StatType.STAT_INT);
            if (bInt > 100)
                m_bResistanceBonus += (byte)((bInt - 100) / 2);

            // TODO: Transformation stats need to be applied here

            if (bSendPacket)
                SendItemMove(2);
        }

        public void SendItemMove(byte subcommand)
        {
            Packet result = new Packet(WIZ_ITEM_MOVE, 0x01);
            result.SetByte(subcommand);
            if (m_bAttackAmount == 0)
                m_bAttackAmount = 100;
            // If the subcommand is not error, send the stats.
            if (subcommand != 0)
            {
                result.SetShort((short)(m_sTotalHit * m_bAttackAmount / 100));
                result.SetShort((short)(m_sTotalAc + m_sACAmount));
                result.SetDword(m_sMaxWeight);
                result.SetShort(m_iMaxHp); result.SetShort(m_iMaxMp);
                result.SetShort(GetStatBonusTotal(StatType.STAT_STR)); result.SetShort(GetStatBonusTotal(StatType.STAT_STA));
                result.SetShort(GetStatBonusTotal(StatType.STAT_DEX)); result.SetShort(GetStatBonusTotal(StatType.STAT_INT));
                result.SetShort(GetStatBonusTotal(StatType.STAT_CHA));
                result.SetShort((short)(((m_sFireR + m_bAddFireR) * m_bPctFireR / 100) + m_bResistanceBonus));
                result.SetShort((short)(((m_sColdR + m_bAddColdR) * m_bPctColdR / 100) + m_bResistanceBonus));
                result.SetShort((short)(((m_sLightningR + m_bAddLightningR) * m_bPctLightningR / 100) + m_bResistanceBonus));
                result.SetShort((short)(((m_sMagicR + m_bAddMagicR) * m_bPctMagicR / 100) + m_bResistanceBonus));
                result.SetShort((short)(((m_sDiseaseR + m_bAddDiseaseR) * m_bPctDiseaseR / 100) + m_bResistanceBonus));
                result.SetShort((short)(((m_sPoisonR + m_bAddPoisonR) * m_bPctPoisonR / 100) + m_bResistanceBonus));
            }
            Send(result);
        }

        public void HpChange(int amount, User pAttacker = null)
        {
            Packet result = new Packet(WIZ_HP_CHANGE);
            short tid = (pAttacker != null ? pAttacker.GetID() : (short)-1);
            short oldHP = m_sHp;
            int originalAmount = amount;
            int mirrorDamage = 0;

            // No cheats allowed
            if (pAttacker != null && pAttacker.GetZoneID() != GetZoneID())
                return;

            // Implement damage/HP cap.
            if (amount < -MAX_DAMAGE)
                amount = -MAX_DAMAGE;
            else if (amount > MAX_DAMAGE)
                amount = MAX_DAMAGE;

            // If we're taking damage...
            if (amount < 0)
            {
                if (isGM())
                    return;

                RemoveStealth();

                // Handle the mirroring of damage.
                //if (m_bMirrorDamage > 0 && isInParty() && GetZoneID() != ZONE_CHAOS_DUNGEON)
                //{
                //    _PARTY_GROUP* pParty = nullptr;
                //    CUser* pUser = nullptr;
                //    mirrorDamage = (m_byMirrorAmount * amount) / 100;
                //    amount -= mirrorDamage;
                //    pParty = g_pMain->GetPartyPtr(GetPartyID());
                //    if (pParty != nullptr)
                //    {
                //        mirrorDamage = mirrorDamage / (GetPartyMemberAmount(pParty) - 1);
                //        for (int i = 0; i < MAX_PARTY_USERS; i++)
                //        {
                //            pUser = g_pMain->GetUserPtr(pParty->uid[i]);
                //            if (pUser == nullptr || pUser == this)
                //                continue;
                //
                //            pUser->HpChange(mirrorDamage);
                //        }
                //    }
                //}

                // Handle mana absorb skills
                //if (m_bManaAbsorb > 0 && GetZoneID() != ZONE_CHAOS_DUNGEON)
                //{
                //    int toBeAbsorbed = 0;
                //    toBeAbsorbed = (originalAmount * m_bManaAbsorb) / 100;
                //    amount -= toBeAbsorbed;
                //
                //    if (m_bManaAbsorb == 15)
                //        toBeAbsorbed *= 4;
                //
                //    MSpChange(toBeAbsorbed);
                //}

                // Handle mastery passives
                //if (isMastered() && GetZoneID() != ZONE_CHAOS_DUNGEON)
                //{
                //    // Matchless: [Passive]Decreases all damages received by 15%
                //    if (CheckSkillPoint(SkillPointMaster, 10, MAX_LEVEL))
                //        amount = (85 * amount) / 100;
                //    // Absoluteness: [Passive]Decrease 10 % demage of all attacks
                //    else if (CheckSkillPoint(SkillPointMaster, 5, 9))
                //        amount = (90 * amount) / 100;
                //}
            }
            // If we're receiving HP and we're undead, all healing must become damage.
            //else if (m_bIsUndead)
            //{
            //    amount = -amount;
            //    originalAmount = amount;
            //}

            if (amount < 0 && -amount >= m_sHp)
                m_sHp = 0;
            else if (amount >= 0 && m_sHp + amount > m_iMaxHp)
                m_sHp = m_iMaxHp;
            else
                m_sHp += (short)amount;

            result.SetShort(m_iMaxHp).SetShort(m_sHp).SetShort(tid);

            if (GetHealth() > 0
            && isMastered()
            && !isMage() && GetZoneID() != ZONE_CHAOS_DUNGEON)
                {
                    short hp30Percent = (short)((30 * GetMaxHealth()) / 100);
                    if ((oldHP >= hp30Percent && m_sHp < hp30Percent)
                        || (m_sHp > hp30Percent))
                    {
                        SetUserAbility();

                        if (m_sHp < hp30Percent)
                            ShowEffect(106800); // skill ID for "Boldness", shown when a player takes damage.
                    }
                }

            Send(result);

            if (isInParty() && GetZoneID() != ZONE_CHAOS_DUNGEON)
                SendPartyHPUpdate();

            if (pAttacker != null)
                    SendTargetHP(0, GetID(), originalAmount);

            if (m_sHp == 0)
                OnDeath(pAttacker);
        }

        private void SendTargetHP(byte echo, int tid, int damage)
        {
            int hp = 0, maxhp = 0;

            CNpc npcTarget = null;
            User userTarget = null;
            if (tid >= NPC_BAND)
            {
                if (g_pMain.m_bPointCheckFlag == false) return;
                CNpc pNpc = g_pMain.GetNpcPtr(tid);
                if (pNpc == null)
                    return;

                hp = pNpc.m_iHP;
                maxhp = pNpc.m_iMaxHP;
                npcTarget = pNpc;
            }
            else
            {
                User pUser = g_pMain.GetUserPtr(tid);
                if (pUser == null || pUser.isDead())
                    return;

                hp = pUser.m_sHp;
                maxhp = pUser.m_iMaxHp;
                userTarget = pUser;
            }

            Packet result = new Packet(WIZ_TARGET_HP);
            result.SetShort((short)tid).SetByte(echo).SetDword(maxhp).SetDword(hp).SetShort((short)damage);
            Send(result);
        }

        private void OnDeath(User pAttacker)
        {
            if (m_bResHpType == USER_DEAD)
                return;

            m_bResHpType = USER_DEAD;

            // Player is dead stop other process.
            ResetWindows();

            
        }

        private void ResetWindows()
        {
        }

        private void SendPartyHPUpdate()
        {
        }

        private void ShowEffect(int Effect)
        {
        }

        public void RemoveStealth()
        {
        }

        public void MSpChange(short m_sMp)
        {
        }

        public void SetMaxMp(int iFlag = 1)
        {
            COEFFICIENT p_TableCoefficient = null;
            p_TableCoefficient = g_pMain.GetCoefficientData(m_sClass);
            if (p_TableCoefficient == null) return;

            int temp_intel = 0, temp_sta = 0;
            temp_intel = getStatTotal(StatType.STAT_INT) + 30;
            //	if( temp_intel > 255 ) temp_intel = 255;
            temp_sta = getStatTotal(StatType.STAT_STA);
            //	if( temp_sta > 255 ) temp_sta = 255;

            if (p_TableCoefficient.Mp != 0)
            {
                m_iMaxMp = (short)((p_TableCoefficient.Mp * GetLevel() * GetLevel() * temp_intel)
                    + (0.1f * GetLevel() * 2 * temp_intel) + (temp_intel / 5) + m_sMaxMPAmount + m_sItemMaxMp + 20);
            }
            else if (p_TableCoefficient.Sp != 0)
            {
                m_iMaxMp = (short)((p_TableCoefficient.Sp * GetLevel() * GetLevel() * temp_sta)
                    + (0.1f * GetLevel() * temp_sta) + (temp_sta / 5) + m_sMaxMPAmount + m_sItemMaxMp);
            }

            if (m_iMaxMp < m_sMp)
            {
                m_sMp = m_iMaxMp;
                MSpChange(m_sMp);
            }
        }

        public void SetMaxHp(int iFlag = 1)
        {
            COEFFICIENT p_TableCoefficient = null;
            p_TableCoefficient = g_pMain.GetCoefficientData(m_sClass);
            if (p_TableCoefficient == null) return;

            int temp_sta = getStatTotal(StatType.STAT_STA);

            if (GetZoneID() == ZONE_SNOW_BATTLE && iFlag == 0)
                if (GetFame() == COMMAND_CAPTAIN || isKing())
                    m_iMaxHp = 300;
                else
                    m_iMaxHp = 100;
            else if (GetZoneID() == ZONE_CHAOS_DUNGEON && iFlag == 0)
                m_iMaxHp = 10000 / 10;
            else
            {
                m_iMaxHp = (short)(((p_TableCoefficient.Hp * GetLevel() * GetLevel() * temp_sta)
                    + 0.1 * (GetLevel() * temp_sta) + (temp_sta / 5)) + m_sMaxHPAmount + m_sItemMaxHp + 20);

                // A player's max HP should be capped at (currently) 14,000 HP.
                if (m_iMaxHp > MAX_PLAYER_HP && !isGM())
                    m_iMaxHp = MAX_PLAYER_HP;

                if (iFlag == 1)
                    m_sHp = m_iMaxHp;
                else if (iFlag == 2)
                    m_iMaxHp = 100;
            }

            if (m_iMaxHp < m_sHp)
            {
                m_sHp = m_iMaxHp;
                HpChange(m_sHp);
            }
        }

        public void SetSlotItemValue()
        {
            // Burada item bonusları verilecek
        }

        public void BlinkStart()
        {
            if (isGM() || GetZoneID() == ZONE_ARDREAM
            || GetZoneID() == ZONE_RONARK_LAND
            || GetZoneID() == ZONE_JURAD_MOUNTAIN
            || GetZoneID() == ZONE_CHAOS_DUNGEON
            || GetZoneID() == ZONE_BORDER_DEFENSE_WAR)
                    return;

            m_bAbnormalType = ABNORMAL_BLINKING;
            m_tBlinkExpiryTime = UNIXTIME + BLINK_TIME;
            m_bRegeneType = REGENE_ZONECHANGE;

            // Monsterler için ai server olmayacağından gerek yok
            //UpdateVisibility(INVIS_DISPEL_ON_ATTACK); // AI shouldn't see us
            m_bInvisibilityType = (byte)InvisibilityType.INVIS_NONE; // but players should. 

            StateChangeServerDirect(3, ABNORMAL_BLINKING);
        }

        public void StateChangeServerDirect(byte type, int buff)
        {
            if (type == 5 && m_bAuthority != 0) return;	//  Operators only!!!

            if (type == 1)
            {
                m_bResHpType = Convert.ToByte(buff);
            }
            else if (type == 2)
            {
                m_bNeedParty = Convert.ToByte(buff);
            }
            else if (type == 3)
            {
                m_bAbnormalType = buff;
            }
            else if (type == 6)
            {
                m_bPartyLeader = Convert.ToBoolean(buff);
            }

            Packet result = new Packet(WIZ_STATE_CHANGE);
            result.SetShort(GetSocketID());
            result.SetByte(type);

            if (type == 1)
            {
                result.SetDword(m_bResHpType);
            }
            else if (type == 2)
            {
                result.SetDword(m_bNeedParty);
            }
            //
            else if (type == 3)
            {
                result.SetDword(m_bAbnormalType);
            }

            else
            {		// Just plain echo :)
                result.SetDword(buff);
                //		N3_SP_STATE_CHANGE_ACTION = 0x04,			// 1 - 인사, 11 - 도발
                //		N3_SP_STATE_CHANGE_VISIBLE = 0x05 };		// 투명 0 ~ 255
            }

            g_pMain.Send_All(result);
            /*
            switch (bType)
            {
                case 1:
                    m_bResHpType = buff;
                    break;

                case 2:
                    m_bNeedParty = buff;
                    break;

                case 3:
                    m_nOldAbnormalType = m_bAbnormalType;

                    // If we're a GM, we need to show ourselves before transforming.
                    // Otherwise the visibility state is completely desynced.
                    if (isGM())
                        StateChangeServerDirect(5, 1);

                    m_bAbnormalType = buff;
                    break;

                case 5:
                    m_bAbnormalType = buff;
                    break;

                case 6:
                    buff = Convert.ToByte(m_bPartyLeader); // we don't set this here.
                    break;

                case 7:
                    UpdateVisibility((InvisibilityType)buff);
                    break;

                case 8: // beginner quest
                    break;
            }

            Packet result = new Packet(WIZ_STATE_CHANGE);
            result.SetShort(GetSocketID());
            result.SetByte(bType); result.SetByte(buff);
            SendToRegion(result);*/
        }

        public void SendWeather()
        {
            Packet result = new Packet(WIZ_WEATHER);
            result.SetByte(1);
            result.SetShort(10 * 60);
            Send(result);
        }

        public void SendTime()
        {
            Packet result = new Packet(WIZ_TIME);
            result.SetShort((short)DateTime.Now.Year);
            result.SetShort((short)DateTime.Now.Month);
            result.SetShort((short)DateTime.Now.Day);
            result.SetShort((short)DateTime.Now.Hour);
            result.SetShort((short)DateTime.Now.Minute);

            Send(result);
        }

        public void TopSendNotice()
        {
            Packet result = new Packet(WIZ_NOTICE);
            byte count = 0;//uint8

            result.SetByte(1); // Old-style notices (top-right of screen)
            result.SetByte(count); // placeholder the 
            Send(result);
        }

        public void SendNotice()
        {
          //  throw new NotImplementedException();
        }

        public void SendMyInfo()
        {

            SetUserAbility(false);

            Packet result = new Packet(WIZ_MYINFO);

            result.SByte();
            result.SetShort(sSid);
            result.SetString(GetName());

            result.SetShort(Convert.ToInt16(m_curx * 10)); result.SetShort(Convert.ToInt16(m_curz * 10)); result.SetShort(Convert.ToInt16(m_cury * 10));

            result.SetByte(m_bNation);
            result.SetByte(m_bRace); result.SetShort(m_sClass); result.SetByte(m_bFace);

            result.SetDword(m_nHair);

            result.SetByte(m_bRank); result.SetByte(m_bTitle);

            result.SetShort(0);

            result.SetByte(GetLevel());
            result.SetShort(m_sPoints);

            result.SetInt64(1/*SetMaxExp()*/); result.SetInt64(m_iExp);

            result.SetDword(m_iLoyalty); result.SetDword(m_iLoyaltyMonthly);

            result.SetShort(GetClanID()); result.SetByte(GetFame());

            Knights pKnights = g_pMain.GetClanPtr(GetClanID());

            if (pKnights == null)
            {
                result.SetInt64(0);
                result.SetShort(-1);
                result.SetDword(0);
            }
            else
            {
                pKnights.OnLogin(this);
                Knights pAllyClan = null;
                if (pKnights.GetAllianceID() > 0)
                    pAllyClan = g_pMain.GetClanPtr(pKnights.GetAllianceID());

                result.SetShort(pKnights.GetAllianceID());
                result.SetByte(pKnights.m_byFlag);
                result.SetString(pKnights.m_strName);
                result.SetByte(pKnights.m_byGrade);
                result.SetByte(pKnights.m_byRanking);

                result.SetShort(pKnights.m_sMarkVersion);
                if (pAllyClan == null)
                    result.SetShort(pKnights.m_sCape);
                else
                    result.SetShort(pAllyClan.m_sCape);

                result.SetByte(pKnights.m_bCapeR);
                result.SetByte(pKnights.m_bCapeG);
                result.SetByte(pKnights.m_bCapeB);
                result.SetByte(0);
            }

            result.SetByte(2);
            result.SetByte(3);
            result.SetByte(4);
            result.SetByte(5);

            result.SetShort(m_iMaxHp); result.SetShort(m_sHp);
            result.SetShort(m_iMaxMp); result.SetShort(m_sMp);

            result.SetDword(m_sMaxWeight); result.SetDword(m_sItemWeight);

            result.SetByte(m_bStats[(int)StatType.STAT_STR]); result.SetByte((byte)GetStatItemBonus(StatType.STAT_STR));
            result.SetByte(m_bStats[(int)StatType.STAT_STA]); result.SetByte((byte)GetStatItemBonus(StatType.STAT_STA));
            result.SetByte(m_bStats[(int)StatType.STAT_DEX]); result.SetByte((byte)GetStatItemBonus(StatType.STAT_DEX));
            result.SetByte(m_bStats[(int)StatType.STAT_INT]); result.SetByte((byte)GetStatItemBonus(StatType.STAT_INT));
            result.SetByte(m_bStats[(int)StatType.STAT_CHA]); result.SetByte((byte)GetStatItemBonus(StatType.STAT_CHA));

            result.SetShort(m_sTotalHit); result.SetShort(m_sTotalAc);

            result.SetByte((byte)m_sFireR);
            result.SetByte((byte)m_sColdR);
            result.SetByte((byte)m_sLightningR);
            result.SetByte((byte)m_sMagicR);
            result.SetByte((byte)m_sDiseaseR);
            result.SetByte((byte)m_sPoisonR);

            result.SetDword(m_iGold);
            result.SetByte(m_bAuthority);
            result.SetByte(m_bKnightsRank); result.SetByte(m_bPersonalRank);

            result.append(m_bstrSkill, 9);

            for (int i = 0; i < INVENTORY_TOTAL; i++)
            {
                _ITEM_DATA pItem = GetItem(i);
                if (pItem == null)
                {
                    pItem = new _ITEM_DATA();
                }

                if (pItem.sRemainingRentalTime < UNIXTIME)
                    pItem.sRemainingRentalTime = 0;

                if (pItem.nExpirationTime < UNIXTIME)
                    pItem.nExpirationTime = 0;

                result.SetDword(pItem.nNum);
                result.SetShort(pItem.sDuration); result.SetShort(pItem.sCount);
                result.SetByte(pItem.bFlag);	// item type flag (e.g. rented)
                result.SetShort(pItem.sRemainingRentalTime);	// remaining time
                result.SetDword(0); // unknown
                result.SetDword(pItem.nExpirationTime); // expiration date in unix time
            }

            result.SetByte(m_bAccountStatus);
            result.SetByte(m_bPremiumType);
            result.SetShort(m_sPremiumTime);
            result.SetByte(m_bIsChicken); // Yumurta
            result.SetDword(m_iMannerPoint);
            result.SetShort(0);
            result.SetDword(0); // Military Camp
            result.SetShort(m_sGenieTime);// Genie
           

            Send(result);

            if (!g_pMain.AddUserInGame(GetID(), this))
                OnDisconnect();
            else
                bGameStart = true;

            Console.WriteLine("Send My info Gönderildi.");          
        }

        public void ServerIndex(Packet pkt)
        {
            Packet result= new Packet(WIZ_SERVER_INDEX);
            result.SetShort(1).SetShort(1); // Server index eklenecek
            Send(result);
        }

        public void SelectCharacter(byte bResult, byte bInit)
        {
            Packet result = new Packet(WIZ_SEL_CHAR);

            if (m_bAuthority == 255) // Bannet 
            {
                OnDisconnect();
                return;
            }

            result.SetByte(bResult);

            if (bResult == 0 || m_bZone <= 0)
                goto fail_return;

            result.SetByte(m_bZone);
            result.SetShort(Convert.ToInt16(m_curx));
            result.SetShort(Convert.ToInt16(m_curz));
            result.SetShort(Convert.ToInt16(m_cury));
            result.SetByte(KARUS);
            bSelectChar = true;

            Send(result);

            return;
            fail_return:
            Send(result);
        }
        
        internal void GetUserInfo(ref Packet result)
        {            
            Knights pKnights = null;

            result.SByte();
            result.SetString(GetName());
            result.SetShort(GetNation());
            result.SetShort(m_bKnights);
            result.SetByte(GetFame());

            pKnights = g_pMain.GetClanPtr(m_bKnights);

            if (pKnights == null)
            {
                result.SetDword(0);
                result.SetShort(0);
                result.SetByte(0);
                result.SetShort(-1);
                result.SetDword(0);
                result.SetByte(0);
            }
            else
            {
                result.SetShort(pKnights.GetAllianceID());

                result.SetString(pKnights.m_strName);

                result.SetByte(pKnights.m_byGrade);
                result.SetByte(pKnights.m_byRanking);

                result.SetShort(pKnights.m_sMarkVersion);
                result.SetShort(pKnights.m_sCape);

                result.SetByte(pKnights.m_bCapeR);
                result.SetByte(pKnights.m_bCapeG);
                result.SetByte(pKnights.m_bCapeB);
                result.SetByte(0);
                result.SetByte(1);
            }

            InvisibilityType bInvisibilityType = (InvisibilityType)m_bInvisibilityType;
            if (bInvisibilityType != InvisibilityType.INVIS_NONE)
                bInvisibilityType = InvisibilityType.INVIS_DISPEL_ON_MOVE;

            result.SetByte(GetLevel()); result.SetByte(m_bRace); result.SetShort(m_sClass);
            result.SetShort(GetSPosX()); result.SetShort(GetSPosZ()); result.SetShort(GetSPosY());
            result.SetByte(m_bFace); result.SetDword(m_nHair);
            result.SetByte(m_bResHpType); result.SetDword(m_bAbnormalType);
            result.SetByte(m_bNeedParty);
            result.SetByte(m_bAuthority);
            result.SetByte(m_bPartyLeader);
            result.SetByte((byte)bInvisibilityType);
            result.SetByte(m_teamColour);
            result.SetByte(m_bIsHidingHelmet);
            result.SetByte((byte)bInvisibilityType);
            result.SetShort(m_sDirection);
            result.SetByte(m_bIsChicken);
            result.SetByte(m_bRank);
            result.SetShort(0);
            result.SetByte(m_bKnightsRank);
            result.SetByte(m_bPersonalRank);

            byte[] equippedItems =
            {
                BREAST, LEG, HEAD, GLOVE, FOOT, SHOULDER, RIGHTHAND, LEFTHAND,
                CWING, CHELMET, CRIGHT, CLEFT, CTOP, CFAIRY
            };

            foreach (byte i in equippedItems)
            {

                _ITEM_DATA pItem = GetItem(i);

                if (pItem == null)
                    continue;

                result.SetDword(pItem.nNum);
                result.SetShort(pItem.sDuration);
                result.SetByte(pItem.bFlag);
            }

            result.SetByte(GetZoneID());
            result.SetShort(-1);
            result.SetDword(0); result.SetShort(0); result.SetByte(0); result.SetByte(isGenieActive());

            if (__VERSION > 2000)
            {
                result.SetByte(isRebirth());
                result.SetShort(GetAchieveTitle());
                result.SetShort(0);
                result.SetByte(0);
                result.SetByte(1);
            }
        }

    }
}