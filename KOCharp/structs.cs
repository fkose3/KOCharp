using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOCharp
{
    public class _SAVED_MAGIC
    {
        public int nSkillID;
        public int nExpiry;
    };

    public enum ItemFlag
    {
        ITEM_FLAG_NONE = 0,
        ITEM_FLAG_RENTED = 1,
        ITEM_FLAG_DUPLICATE = 3,
        ITEM_FLAG_SEALED = 4,
        ITEM_FLAG_NOT_BOUND = 7,
        ITEM_FLAG_BOUND = 8
    };

    public class _ITEM_DATA
    {
        public Int32 nNum;
        public short sDuration;
        public short sCount;
        public byte bFlag; // see ItemFlag
        public short sRemainingRentalTime; // in minutes
        public Int32 nExpirationTime; // in unix time
        public Int64 nSerialNum;
        public bool IsSelling;

        public bool isSealed() { return bFlag == (byte)ItemFlag.ITEM_FLAG_SEALED; }
        public bool isBound() { return bFlag == (byte)ItemFlag.ITEM_FLAG_BOUND; }
        public bool isRented() { return bFlag == (byte)ItemFlag.ITEM_FLAG_RENTED; }
        public bool isDuplicate() { return bFlag == (byte)ItemFlag.ITEM_FLAG_DUPLICATE; }

        public _ITEM_DATA()
        {
            nNum = 0;
            sDuration = 0;
            sCount = 0;
            bFlag = 0; // see ItemFlag
            sRemainingRentalTime = 0; // in minutes
            nExpirationTime = 0; // in unix time
            nSerialNum = 0;
            IsSelling = false;
        }
    };

    public enum ChatType
    {
        GENERAL_CHAT = 1,
        PRIVATE_CHAT = 2,
        PARTY_CHAT = 3,
        FORCE_CHAT = 4,
        SHOUT_CHAT = 5,
        KNIGHTS_CHAT = 6,
        PUBLIC_CHAT = 7,
        WAR_SYSTEM_CHAT = 8,
        PERMANENT_CHAT = 9,
        END_PERMANENT_CHAT = 10,
        MONUMENT_NOTICE = 11,
        GM_CHAT = 12,
        COMMAND_CHAT = 13,
        MERCHANT_CHAT = 14,
        ALLIANCE_CHAT = 15,
        ANNOUNCEMENT_CHAT = 17,
        SEEKING_PARTY_CHAT = 19,
        GM_INFO_CHAT = 21,	// info window : "Level: 0, UserCount:16649" (NOTE: Think this is the missing overhead info (probably in form of a command (with args))
        COMMAND_PM_CHAT = 22,	// Commander Chat PM??
        CLAN_NOTICE = 24,
        KROWAZ_NOTICE = 25,
        DEATH_NOTICE = 26,
        CHAOS_STONE_ENEMY_NOTICE = 27,	// The enemy has destroyed the Chaos stone something (Red text, middle of screen)
        CHAOS_STONE_NOTICE = 28,
        ANNOUNCEMENT_WHITE_CHAT = 29	// what's it used for?
    };

    public enum HairData
    {
        HAIR_R,
        HAIR_G,
        HAIR_B,
        HAIR_TYPE
    };

    public class _MERCH_DATA
    {
        Int32 nNum;
        short sDuration;
        short sCount;
        short bCount;
        Int64 nSerialNum;
        Int32 nPrice;
        byte bOriginalSlot;
        bool IsSoldOut;
    };

    public enum AuthorityTypes
    {
        AUTHORITY_GAME_MASTER = 0,
        AUTHORITY_PLAYER = 1,
        AUTHORITY_MUTED = 11,
        AUTHORITY_ATTACK_DISABLED = 12,
        AUTHORITY_LIMITED_GAME_MASTER = 250,
        AUTHORITY_BANNED = 255
    };

    public enum StatType : int
    {
        STAT_STR = 0,
        STAT_STA = 1,
        STAT_DEX = 2,
        STAT_INT = 3,
        STAT_CHA = 4, // MP
        STAT_COUNT
    };

    public enum AttackResult
    {
        ATTACK_FAIL = 0,
        ATTACK_SUCCESS = 1,
        ATTACK_TARGET_DEAD = 2,
        ATTACK_TARGET_DEAD_OK = 3,
        MAGIC_ATTACK_TARGET_DEAD = 4
    };

    public enum GetTypes
    {
        AccountID = 0x01,
        CharID = 0x02
    };

    public class _CLASS_COEFFICIENT
    {
        public short sClassNum;
        public float ShortSword;
        public float Sword;
        public float Axe;
        public float Club;
        public float Spear;
        public float Pole;
        public float Staff;
        public float Bow;
        public float HP;
        public float MP;
        public float SP;
        public float AC;
        public float Hitrate;
        public float Evasionrate;
    };

    public class _LEVEL_UP
    {
        public byte Level;
        public long iExp;
    };

    public class _ITEM_TABLE
    {
        public  int m_iNum;
        public string m_sName;
        public byte m_bKind;
        public byte m_bSlot;
        public byte m_bRace;
        public byte m_bClass;
        public short m_sDamage;
        public short m_sDelay;
        public short m_sRange;
        public short m_sWeight;
        public short m_sDuration;
        public int m_iBuyPrice;
        public int m_iSellPrice;
        public short m_sAc;
        public byte m_bCountable;
        public int m_iEffect1;
        public int m_iEffect2;
        public byte m_bReqLevel;
        public byte m_bReqLevelMax;
        public byte m_bReqRank;
        public byte m_bReqTitle;
        public byte m_bReqStr;
        public byte m_bReqSta;
        public byte m_bReqDex;
        public byte m_bReqIntel;
        public byte m_bReqCha;
        public byte m_bSellingGroup;
        public byte m_ItemType;
        public short m_sHitrate;
        public short m_sEvarate;
        public short m_sDaggerAc;
        public short m_sSwordAc;
        public short m_sMaceAc;
        public short m_sAxeAc;
        public short m_sSpearAc;
        public short m_sBowAc;
        public byte m_bFireDamage;
        public byte m_bIceDamage;
        public byte m_bLightningDamage;
        public byte m_bPoisonDamage;
        public byte m_bHPDrain;
        public byte m_bMPDamage;
        public byte m_bMPDrain;
        public byte m_bMirrorDamage;
        public short m_sStrB;
        public short m_sStaB;
        public short m_sDexB;
        public short m_sIntelB;
        public short m_sChaB;
        public short m_MaxHpB;
        public short m_MaxMpB;
        public short m_bFireR;
        public short m_bColdR;
        public short m_bLightningR;
        public short m_bMagicR;
        public short m_bPoisonR;
        public short m_bCurseR;
        public short ItemClass;
        public short ItemExt;

        public bool isStackable() { return m_bCountable != 0; }

        public byte GetKind() { return m_bKind; }
        public byte GetItemGroup() { return (byte)(m_bKind / 10); }

        public bool isDagger() { return GetItemGroup() == Define.WEAPON_DAGGER; }
        public bool isSword() { return GetItemGroup() == Define.WEAPON_SWORD; }
        public bool is2HSword() { return GetKind() == Define.WEAPON_2H_SWORD; }
        public bool isAxe() { return GetItemGroup() == Define.WEAPON_AXE; }
        public bool is2HAxe() { return GetKind() == Define.WEAPON_2H_AXE; }
        public bool isMace() { return GetItemGroup() == Define.WEAPON_MACE || GetItemGroup() == Define.WEAPON_MACE2; }
        public bool is2HMace() { return GetKind() == Define.WEAPON_2H_MACE || GetItemGroup() == Define.WEAPON_MACE2; }
        public bool isSpear() { return GetItemGroup() == Define.WEAPON_SPEAR; }
        public bool is2HSpear() { return GetKind() == Define.WEAPON_2H_SPEAR; }
        public bool isShield() { return GetItemGroup() == Define.WEAPON_SHIELD; }
        public bool isStaff() { return GetItemGroup() == Define.WEAPON_STAFF; }
        public bool isBow() { return GetItemGroup() == Define.WEAPON_BOW || GetItemGroup() == Define.WEAPON_LONGBOW; }
        public bool isPickaxe() { return GetKind() == Define.WEAPON_PICKAXE; }

        public bool isAccessory() { return GetKind() == Define.ACCESSORY_EARRING || GetKind() == Define.ACCESSORY_NECKLACE || GetKind() == Define.ACCESSORY_RING || GetKind() == Define.ACCESSORY_BELT; }
        public bool isEarring() { return GetKind() == Define.ACCESSORY_EARRING; }
        public bool isNecklace() { return GetKind() == Define.ACCESSORY_NECKLACE; }
        public bool isRing() { return GetKind() == Define.ACCESSORY_RING; }
        public bool isBelt() { return GetKind() == Define.ACCESSORY_BELT; }

        public bool is2Handed() { return m_bSlot == (byte)ItemSlotType.ItemSlot2HLeftHand || m_bSlot == (byte)ItemSlotType.ItemSlot2HRightHand; }
    };

    public enum ItemSlotType
    {
        ItemSlot1HEitherHand = 0,
        ItemSlot1HRightHand = 1,
        ItemSlot1HLeftHand = 2,
        ItemSlot2HRightHand = 3,
        ItemSlot2HLeftHand = 4,
        ItemSlotPauldron = 5,
        ItemSlotPads = 6,
        ItemSlotHelmet = 7,
        ItemSlotGloves = 8,
        ItemSlotBoots = 9,
        ItemSlotEarring = 10,
        ItemSlotNecklace = 11,
        ItemSlotRing = 12,
        ItemSlotShoulder = 13,
        ItemSlotBelt = 14,
        ItemSlotBag = 25,
        ItemSlotCospreGloves = 100,
        ItemSlotCosprePauldron = 105,
        ItemSlotCospreHelmet = 107,
        ItemSlotCospreWings = 110
    };

    public enum ClassType
    {
        ClassWarrior = 1,
        ClassRogue = 2,
        ClassMage = 3,
        ClassPriest = 4,
        ClassWarriorNovice = 5,
        ClassWarriorMaster = 6,
        ClassRogueNovice = 7,
        ClassRogueMaster = 8,
        ClassMageNovice = 9,
        ClassMageMaster = 10,
        ClassPriestNovice = 11,
        ClassPriestMaster = 12
    };

    public enum AbnormalType
    {
        ABNORMAL_INVISIBLE = 0,	// Hides character completely (for GM visibility only).
        ABNORMAL_NORMAL = 1,	// Shows character. This is the default for players.
        ABNORMAL_GIANT = 2,	// Enlarges character.
        ABNORMAL_DWARF = 3,	// Shrinks character.
        ABNORMAL_BLINKING = 4,	// Forces character to start blinking.
        ABNORMAL_GIANT_TARGET = 6		// Enlarges character and shows "Hit!" effect.
    };

    public class _ZONE_INFO
    {
        public short m_nServerNo;
        public short m_nZoneNumber;
        public string m_MapName;

        public float m_fInitX, m_fInitY, m_fInitZ;
        public short m_bType, isAttackZone;
    };

    public class _START_POSITION
    {
        public short ZoneID;
        public short sKarusX;
        public short sKarusZ;
        public short sElmoradX;
        public short sElmoradZ;
        public short sKarusGateX;
        public short sKarusGateZ;
        public short sElmoradGateX;
        public short sElmoradGateZ;
        public byte bRangeX;
        public byte bRangeZ;
    };

    public enum UserUpdateType
    {
        UPDATE_LOGOUT,
        UPDATE_ALL_SAVE,
        UPDATE_PACKET_SAVE,
    };

    public enum ClanTypeFlag
    {
        ClanTypeNone = 0,
        ClanTypeTraining = 1,
        ClanTypePromoted = 2,
        ClanTypeAccredited5 = 3,
        ClanTypeAccredited4 = 4,
        ClanTypeAccredited2 = 5,
        ClanTypeAccredited3 = 6,
        ClanTypeAccredited1 = 7,
        ClanTypeRoyal5 = 8,
        ClanTypeRoyal4 = 9,
        ClanTypeRoyal3 = 10,
        ClanTypeRoyal2 = 11,
        ClanTypeRoyal1 = 12
    };

    public enum InvisibilityType
    {
        INVIS_NONE = 0,
        INVIS_DISPEL_ON_MOVE = 1,
        INVIS_DISPEL_ON_ATTACK = 2
    };

    public enum MerchantState
    {
        MERCHANT_STATE_NONE = -1,
        MERCHANT_STATE_SELLING = 0,
        MERCHANT_STATE_BUYING = 1
    };

    public class _MAGIC_TABLE
    {
        public int iNum;
        public int nBeforeAction;
        public byte bTargetAction;
        public byte bSelfEffect;
        public short bFlyingEffect;
        public short iTargetEffect;
        public byte bMoral;
        public short sSkillLevel;
        public short sSkill;
        public short sMsp;
        public short sHP;
        public byte bItemGroup;
        public int iUseItem;
        public byte bCastTime;
        public short sReCastTime;
        public byte bSuccessRate;
        public byte[] bType = new byte[2];
        public short sRange;
        public byte sUseStanding;
        public short sEtc;
    }

    public class _MAGIC_TYPE1
    {
        public int iNum;
        public byte bHitType;
        public short sHitRate;
        public short sHit;
        public short sAddDamage;
        public byte bDelay;
        public byte bComboType;
        public byte bComboCount;
        public short sComboDamage;
        public short sRange;
    }

    public class _MAGIC_TYPE2
    {
        public int iNum;
        public byte bHitType;
        public short sHitRate;
        public short sAddDamage;
        public short sAddRange;
        public byte bNeedArrow;
    }

    public class _MAGIC_TYPE3
    {
        public int iNum;
        public byte bDirectType;
        public short sAngle;
        public short sFirstDamage;
        public short sEndDamage;
        public short sTimeDamage;
        public byte bRadius;
        public byte bDuration;  // duration, in seconds
        public byte bAttribute;
    }

    public class _MAGIC_TYPE4
    {
        public int iNum;
        public byte bBuffType;
        public byte bRadius;
        public short sDuration;  // duration, in seconds
        public byte bAttackSpeed;
        public byte bSpeed;
        public short sAC;
        public short sACPct;
        public byte bAttack;
        public byte bMagicAttack;
        public short sMaxHP;
        public short sMaxHPPct;
        public short sMaxMP;
        public short sMaxMPPct;
        public byte bHitRate;
        public short sAvoidRate;
        public sbyte bStr;
        public sbyte bSta;
        public sbyte bDex;
        public sbyte bIntel;
        public sbyte bCha;
        public byte bFireR;
        public byte bColdR;
        public byte bLightningR;
        public byte bMagicR;
        public byte bDiseaseR;
        public byte bPoisonR;
        public short sExpPct;
        public short sSpecialAmount;

        public bool bIsBuff; // true if buff, false if debuff

        public bool isBuff() { return bIsBuff; }
        public bool isDebuff() { return !bIsBuff; }
    }

    public class _MAGIC_TYPE5
    {
        public int iNum;
        public byte bType;
        public byte bExpRecover;
        public short sNeedStone;
    }

    public class _MAGIC_TYPE6
    {
        public int iNum;
        public short sSize;
        public short sTransformID;
        public short sDuration; // duration, in seconds
        public short sMaxHp;
        public short sMaxMp;
        public byte bSpeed;
        public short sAttackSpeed;
        public short sTotalHit;
        public short sTotalAc;
        public short sTotalHitRate;
        public short sTotalEvasionRate;
        public short sTotalFireR;
        public short sTotalColdR;
        public short sTotalLightningR;
        public short sTotalMagicR;
        public short sTotalDiseaseR;
        public short sTotalPoisonR;
        public short sClass;
        public byte bUserSkillUse;
        public byte bNeedItem;
        public byte bSkillSuccessRate;
        public byte bMonsterFriendly;
        public byte bNation;
    }

    public enum TransformationSkillUse
    {
        TransformationSkillUseSiege = 0,
        TransformationSkillUseMonster = 1,
        TransformationSkillUseNPC = 3,
        TransformationSkillUseSpecial = 4, // e.g. Snowman transformations
        TransformationSkillUseSiege2 = 7  // e.g. Moving Towers
    }

    public class _MAGIC_TYPE7
    {
        public int iNum;
        public byte bValidGroup;
        public byte bNationChange;
        public short sMonsterNum;
        public byte bTargetChange;
        public byte bStateChange;
        public byte bRadius;
        public short sHitRate;
        public short sDuration;
        public short sDamage;
        public byte bVision;
        public int nNeedItem;
    }

    public class _MAGIC_TYPE8
    {
        public int iNum;
        public byte bTarget;
        public short sRadius;
        public byte bWarpType;
        public short sExpRecover;
        public short sKickDistance; // used exclusively by soccer ball-control skills.
    }

    public class _MAGIC_TYPE9
    {
        public int iNum;
        public byte bValidGroup;
        public byte bNationChange;
        public short sMonsterNum;
        public byte bTargetChange;
        public byte bStateChange;
        public short sRadius;
        public short sHitRate;
        public short sDuration;
        public short sDamage;
        public short sVision;
        public int nNeedItem;
    }

    public class _SERVER_RESOURCE
    {
        public int nResourceID;
        public string strResource;
    }

    public class _BUFF_TYPE4_INFO
    {
        public int m_nSkillID;
        public byte m_bBuffType;
        public bool m_bIsBuff; // Is it a buff or a debuff?
        public bool m_bDurationExtended;
        public Double m_tEndTime;

        public bool isBuff() { return m_bIsBuff; }
        public bool isDebuff() { return !m_bIsBuff; }

        public _BUFF_TYPE4_INFO()
        {
            m_nSkillID = 0;
            m_bBuffType = 0;
            m_bIsBuff = false;
            m_bDurationExtended = false;
            m_tEndTime = 0;
        }
    };

    public class _ACHIEVE_KILL_DATA
    {
        public int KillCountType1;
        public int KillCountType2;
        public bool isComplate;
        public bool isGiveItem;

        byte GetStatus()
        {
            if (isGiveItem && isComplate)
                return 5;
            else if (isComplate)
                return 4;
            else if (KillCountType1 > 0 || KillCountType2 > 0)
                return 1;
            else
                return 0;
        }
    };
}
