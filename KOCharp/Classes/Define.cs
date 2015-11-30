using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KOCharp
{
    public class Define
    {
        public const int TIME_PORT_RESERVE = 60*60; // 1 dakika

        public const int MAX_ID_SIZE = 21;
        public const int MAX_PW_SIZE = 28;

        public const int MAX_READ_LEN = 4084;

        public const int __VERSION = 2004;

        public const int VIEW_DISTANCE = 48;

        public const int DBAGENTPORT = 28562;

        public const int USER_MAX = 1500;

        public const int ACHIEVE_MAX = 437;

        public const int NPC_BAND = 3000;
        public const int USER_BAND = 0;
        public const int CONST_BAND = 30000;

        public const byte CHIEF = 0x01;// ����
        public const byte VICECHIEF = 0x02;// �δ���
        public const byte TRAINEE = 0x05;// ���
        public const byte COMMAND_CAPTAIN = 100;    // ���ֱ���

        // Item Weapon Type Define
        public const byte WEAPON_DAGGER = 1;
        public const byte WEAPON_SWORD = 2;
        public const byte WEAPON_2H_SWORD = 22;// Kind field as-is
        public const byte WEAPON_AXE = 3;
        public const byte WEAPON_2H_AXE = 32; // Kind field as-is
        public const byte WEAPON_MACE = 4;
        public const byte WEAPON_2H_MACE = 42;// Kind field as-is
        public const byte WEAPON_SPEAR = 5;
        public const byte WEAPON_2H_SPEAR = 52;// Kind field as-is
        public const byte WEAPON_SHIELD = 6;
        public const byte WEAPON_BOW = 7;
        public const byte WEAPON_LONGBOW = 8;
        public const byte WEAPON_LAUNCHER = 10;
        public const byte WEAPON_STAFF = 11;
        public const byte WEAPON_ARROW = 12;// ��ų ���
        public const byte WEAPON_JAVELIN = 13;	// ��ų ���
        public const byte WEAPON_MACE2 = 18;
        public const byte WEAPON_WORRIOR_AC = 21;	// ��ų ���
        public const byte WEAPON_LOG_AC = 22;	// ��ų ���
        public const byte WEAPON_WIZARD_AC = 23;	// ��ų ���
        public const byte WEAPON_PRIEST_AC = 24;	// ��ų ���
        public const byte WEAPON_PICKAXE = 61;  // Unlike the others, this is just the Kind field as-is (not / 10).

        public const byte ACCESSORY_EARRING = 91;
        public const byte ACCESSORY_NECKLACE = 92;
        public const byte ACCESSORY_RING = 93;
        public const byte ACCESSORY_BELT = 94;

        public const byte ITEM_KIND_COSPRE = 252;

        public const byte KARUS = 1;
        public const byte ELMORAD = 2;

        public const byte RIGHTEAR = 0;
        public const byte HEAD = 1;
        public const byte LEFTEAR = 2;
        public const byte NECK = 3;
        public const byte BREAST = 4;
        public const byte SHOULDER = 5;
        public const byte RIGHTHAND = 6;
        public const byte WAIST = 7;
        public const byte LEFTHAND = 8;
        public const byte RIGHTRING = 9;
        public const byte LEG = 10;
        public const byte LEFTRING = 11;
        public const byte GLOVE = 12;
        public const byte FOOT = 13;
        public const byte RESERVED = 14;

        public const byte CWING = 42;
        public const byte CHELMET = 43;
        public const byte CLEFT = 44;
        public const byte CRIGHT = 45;
        public const byte CTOP = 46;
        public const byte BAG1 = 47;
        public const byte BAG2 = 48;
        public const byte CFAIRY = 49;

        public const byte COSP_WINGS = 0;
        public const byte COSP_HELMET = 1;
        public const byte COSP_GLOVE = 2;
        public const byte COSP_GLOVE2 = 3;
        public const byte COSP_BREAST = 4;
        public const byte COSP_FAIRY = 5;

        public const byte COSP_BAG1 = 5; // relative bag slot from cospre items
        public const byte COSP_BAG2 = 6; // relative bag slot from cospre items

        public const byte SLOT_MAX = 14; // 14 equipped item slots
        public const byte HAVE_MAX = 28; // 28 inventory slots
        public const byte COSP_MAX = 6; // 4 cospre slots wings +1 =5
        public const byte MBAG_COUNT = 2; // 2 magic bag slots
        public const byte MBAG_MAX = 12; // 12 slots per magic bag
        public const byte EXTRA_MAX = (MBAG_COUNT * MBAG_MAX) + COSP_MAX + MBAG_COUNT;

        // Total number of magic bag slots
        public const byte MBAG_TOTAL = (MBAG_MAX * MBAG_COUNT);

        // Start of inventory area                                                           ;
        public const byte INVENTORY_INVENT = (SLOT_MAX);

        // Start of cospre area                                                              ;
        public const byte INVENTORY_COSP = (SLOT_MAX + HAVE_MAX + COSP_MAX);

        // Start of magic bag slots (after the slots for the bags themselves)                ;
        public const byte INVENTORY_MBAG = (SLOT_MAX + HAVE_MAX + COSP_MAX + MBAG_COUNT);

        // Start of magic bag 1 slots (after the slots for the bags themselves)              ;
        public const byte INVENTORY_MBAG1 = (INVENTORY_MBAG);

        // Start of magic bag 2 slots (after the slots for the bags themselves)              ;
        public const byte INVENTORY_MBAG2 = (INVENTORY_MBAG + MBAG_MAX);

        // Total slots in the general-purpose inventory storage                              ;
        public const byte INVENTORY_TOTAL = (INVENTORY_MBAG2 + MBAG_MAX);

        public const byte WAREHOUSE_MAX = 192;
        public const int QUEST_ARRAY_SIZE = 600;


        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        /*
        public  static float UNIXTIME
        {
            get
            {
                TimeSpan t = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0));
                return Convert.ToInt32(t.TotalSeconds);
                DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
                TimeSpan span = (DateTime.Now.ToLocalTime() - epoch);
                return (float)span.TotalSeconds;

                bool bInit = false;
                bool bUseHWTimer = false;
                long nTime = 0, nFrequency = 0;

                if (bInit == false)
                {
                    if (QueryPerformanceCounter(out nTime))
                    {
                        QueryPerformanceFrequency(out nFrequency);
                        bUseHWTimer = true;
                    }
                    else
                    {
                        bUseHWTimer = false;
                    }

                    bInit = true;
                }

                if (bUseHWTimer)
                {
                    QueryPerformanceCounter(out nTime);
                    return (float)((double)(nTime) / (double)nFrequency);
                }

                return (float)System.DateTime.Now.Ticks;
            }
        }
        */

        public static Int32 UNIXTIME = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

        public int myrand(int min, int max) { return new Random().Next(min, max); }

        public const byte MAX_LEVEL = 83;
        public const int SHOUT_COIN_REQUIREMENT = 3000;

        public const int MAX_KNIGHTS_MARK = 2400;
        public const int CLAN_SYMBOL_COST = 5000000;
        public const int MAX_CLAN_USERS = 36;
        public const int MIN_NATIONAL_POINTS = 500;
        public const int MIN_NP_TO_DONATE = 1000;
        public const int MAX_CLAN_NOTICE_LENGTH = 128;

        public const int BLINK_TIME = 10;

        public const byte MAX_CLASS = 26;
        public const short MAX_PLAYER_HP = 14000;
        public const short MAX_DAMAGE = 32000; // Game uses a signed 2 byte integer, so the limit is technically 32,767. The game, however, caps it at 32,000.

        // Zone IDs
        public const byte ZONE_KARUS = 1;
        public const byte ZONE_ELMORAD = 2;
        public const byte ZONE_KARUS_ESLANT = 11;
        public const byte ZONE_ELMORAD_ESLANT = 12;
        public const byte ZONE_MORADON = 21;
        public const byte ZONE_DELOS = 30;
        public const byte ZONE_BIFROST = 31;
        public const byte ZONE_DESPERATION_ABYSS = 32;
        public const byte ZONE_HELL_ABYSS = 33;
        public const byte ZONE_DRAGON_CAVE = 34;
        public const byte ZONE_ARENA = 48;
        public const byte ZONE_ORC_ARENA = 51;
        public const byte ZONE_BLOOD_DON_ARENA = 52;
        public const byte ZONE_GOBLIN_ARENA = 53;
        public const byte ZONE_CAITHAROS_ARENA = 54;
        public const byte ZONE_FORGOTTEN_TEMPLE = 55;

        public const byte ZONE_BATTLE_BASE = 60;

        public const byte ZONE_BATTLE = ZONE_BATTLE_BASE + 1;/// Napies Gorge
        public const byte ZONE_BATTLE2 = ZONE_BATTLE_BASE + 2;/// Alseids Prairie
        public const byte ZONE_BATTLE3 = ZONE_BATTLE_BASE + 3;/// Nieds Triangle
        public const byte ZONE_BATTLE4 = ZONE_BATTLE_BASE + 4;/// Nereid's Island
        public const byte ZONE_BATTLE5 = ZONE_BATTLE_BASE + 5;/// Zipang
        public const byte ZONE_BATTLE6 = ZONE_BATTLE_BASE + 6;/// Oread=                     ;

        public const byte ZONE_SNOW_BATTLE = 69;
        public const byte ZONE_RONARK_LAND = 71;
        public const byte ZONE_ARDREAM = 72;
        public const byte ZONE_RONARK_LAND_BASE = 73;

        public const byte ZONE_KROWAZ_DOMINION = 75;
        public const byte ZONE_BORDER_DEFENSE_WAR = 84;
        public const byte ZONE_CHAOS_DUNGEON = 85;
        public const byte ZONE_JURAD_MOUNTAIN = 87;
        public const byte ZONE_PRISON = 92;
        public const byte ZONE_ISILOON_ARENA = 93;
        public const byte ZONE_FELANKOR_ARENA = 94;

        public const byte GROUP_WARRIOR = 1;
        public const byte GROUP_ROGUE = 2;
        public const byte GROUP_MAGE = 3;
        public const byte GROUP_CLERIC = 4;
        public const byte GROUP_ATTACK_WARRIOR = 5;
        public const byte GROUP_DEFENSE_WARRIOR = 6;
        public const byte GROUP_ARCHERER = 7;
        public const byte GROUP_ASSASSIN = 8;
        public const byte GROUP_ATTACK_MAGE = 9;
        public const byte GROUP_PET_MAGE = 10;
        public const byte GROUP_HEAL_CLERIC = 11;
        public const byte GROUP_CURSE_CLERIC = 12;

        // REGENE TYPES
        public const byte REGENE_NORMAL = 0;
        public const byte REGENE_MAGIC = 1;
        public const byte REGENE_ZONECHANGE = 2;

        public const byte ABNORMAL_INVISIBLE = 0;
        public const byte ABNORMAL_NORMAL = 1;
        public const byte ABNORMAL_GIANT = 2;
        public const byte ABNORMAL_DWARF = 3;
        public const byte ABNORMAL_BLINKING = 4;
        public const byte ABNORMAL_GIANT_TARGET = 6;

        /////////////////////////////////////////////////////////////
        // ITEM TYPE DEFINE
        public const byte ITEM_TYPE_FIRE = 0x01;
        public const byte ITEM_TYPE_COLD = 0x02;
        public const byte ITEM_TYPE_LIGHTNING = 0x03;
        public const byte ITEM_TYPE_POISON = 0x04;
        public const byte ITEM_TYPE_HP_DRAIN = 0x05;
        public const byte ITEM_TYPE_MP_DAMAGE = 0x06;
        public const byte ITEM_TYPE_MP_DRAIN = 0x07;
        public const byte ITEM_TYPE_MIRROR_DAMAGE = 0x08;

        // enum MerchantState
        public const sbyte MERCHANT_STATE_NONE = -1;
        public const sbyte MERCHANT_STATE_SELLING = 0;
        public const sbyte MERCHANT_STATE_BUYING = 1;


        public static bool STRCMP(string param1, string param2)
        {
            return param1.TrimEnd(' ', '/') == param2.TrimEnd(' ', '/');
        }

        public const byte WIZ_LOGIN = 0x01; // Account Login
        public const byte WIZ_NEW_CHAR = 0x02; // Create Character DB
        public const byte WIZ_DEL_CHAR = 0x03; // Delete Character DB
        public const byte WIZ_SEL_CHAR = 0x04; // Select Character
        public const byte WIZ_SEL_NATION = 0x05; // Select Nation
        public const byte WIZ_MOVE = 0x06; // Move ( 1 Second )
        public const byte WIZ_USER_INOUT = 0x07; // User Info Insert, delete
        public const byte WIZ_ATTACK = 0x08; // General Attack 
        public const byte WIZ_ROTATE = 0x09; // Rotate
        public const byte WIZ_NPC_INOUT = 0x0A; // Npc Info Insert, delete
        public const byte WIZ_NPC_MOVE = 0x0B; // Npc Move ( 1 Second )
        public const byte WIZ_ALLCHAR_INFO_REQ = 0x0C; // Account All Character Info Request
        public const byte WIZ_GAMESTART = 0x0D; // Request Other User, Npc Info
        public const byte WIZ_MYINFO = 0x0E; // User Detail Data Download
        public const byte WIZ_LOGOUT = 0x0F; // Request Logout
        public const byte WIZ_CHAT = 0x10; // User Chatting..
        public const byte WIZ_DEAD = 0x11; // User Dead
        public const byte WIZ_REGENE = 0x12; // User	Regeneration
        public const byte WIZ_TIME = 0x13; // Game Timer
        public const byte WIZ_WEATHER = 0x14; // Game Weather
        public const byte WIZ_REGIONCHANGE = 0x15; // Region UserInfo Receive
        public const byte WIZ_REQ_USERIN = 0x16; // Client Request UnRegistered User List
        public const byte WIZ_HP_CHANGE = 0x17; // Current HP Download
        public const byte WIZ_MSP_CHANGE = 0x18; // Current MP Download
        public const byte WIZ_ITEM_LOG = 0x19; // Send To Agent for Writing Log
        public const byte WIZ_EXP_CHANGE = 0x1A; // Current EXP Download
        public const byte WIZ_LEVEL_CHANGE = 0x1B; // Max HP, MP, SP, Weight, Exp Download
        public const byte WIZ_NPC_REGION = 0x1C; // Npc Region Change Receive
        public const byte WIZ_REQ_NPCIN = 0x1D; // Client Request UnRegistered NPC List
        public const byte WIZ_WARP = 0x1E; // User Remote Warp
        public const byte WIZ_ITEM_MOVE = 0x1F; // User Item Move
        public const byte WIZ_NPC_EVENT = 0x20; // User Click Npc Event
        public const byte WIZ_ITEM_TRADE = 0x21; // Item Trade 
        public const byte WIZ_TARGET_HP = 0x22; // Attack Result Target HP 
        public const byte WIZ_ITEM_DROP = 0x23; // Zone Item Insert
        public const byte WIZ_BUNDLE_OPEN_REQ = 0x24; // Zone Item list Request
        public const byte WIZ_TRADE_NPC = 0x25; // ITEM Trade start
        public const byte WIZ_ITEM_GET = 0x26; // Zone Item Get
        public const byte WIZ_ZONE_CHANGE = 0x27; // Zone Change
        public const byte WIZ_POINT_CHANGE = 0x28; // Str, Sta, dex, intel, cha, point up down
        public const byte WIZ_STATE_CHANGE = 0x29; // User Sitdown or Stand
        public const byte WIZ_LOYALTY_CHANGE = 0x2A; // Nation Contribution
        public const byte WIZ_VERSION_CHECK = 0x2B; // Client version check 
        public const byte WIZ_CRYPTION = 0x2C; // Cryption
        public const byte WIZ_USERLOOK_CHANGE = 0x2D; // User Slot Item Resource Change
        public const byte WIZ_NOTICE = 0x2E; // Update Notice Alarm 
        public const byte WIZ_PARTY = 0x2F; // Party Related Packet
        public const byte WIZ_EXCHANGE = 0x30; // Exchange Related Packet
        public const byte WIZ_MAGIC_PROCESS = 0x31; // Magic Related Packet
        public const byte WIZ_SKILLPT_CHANGE = 0x32; // User changed particular skill point
        public const byte WIZ_OBJECT_EVENT = 0x33; // Map Object Event Occur ( ex : Bind Point Setting )
        public const byte WIZ_CLASS_CHANGE = 0x34; // 10 level over can change class 
        public const byte WIZ_CHAT_TARGET = 0x35; // Select Private Chanting User
        public const byte WIZ_CONCURRENTUSER = 0x36; // Current Game User Count
        public const byte WIZ_DATASAVE = 0x37; // User GameData DB Save Request
        public const byte WIZ_DURATION = 0x38; // Item Durability Change
        public const byte WIZ_TIMENOTIFY = 0x39; // Time Adaption Magic Time Notify Packet ( 2 Seconds )
        public const byte WIZ_REPAIR_NPC = 0x3A; // Item Trade, Upgrade and Repair
        public const byte WIZ_ITEM_REPAIR = 0x3B; // Item Repair Processing
        public const byte WIZ_KNIGHTS_PROCESS = 0x3C; // Knights Related Packet..
        public const byte WIZ_ITEM_COUNT_CHANGE = 0x3D; // Item cout change.  
        public const byte WIZ_KNIGHTS_LIST = 0x3E; // All Knights List Info download
        public const byte WIZ_ITEM_REMOVE = 0x3F; // Item Remove from inventory
        public const byte WIZ_OPERATOR = 0x40; // Operator Authority Packet
        public const byte WIZ_SPEEDHACK_CHECK = 0x41; // Speed Hack Using Check
        public const byte WIZ_COMPRESS_PACKET = 0x42; // Data Compressing Packet
        public const byte WIZ_SERVER_CHECK = 0x43; // Server Status Check Packet
        public const byte WIZ_CONTINOUS_PACKET = 0x44; // Region Data Packet 
        public const byte WIZ_WAREHOUSE = 0x45; // Warehouse Open, In, Out
        public const byte WIZ_SERVER_CHANGE = 0x46; // When you change the server
        public const byte WIZ_REPORT_BUG = 0x47; // Report Bug to the manager
        public const byte WIZ_HOME = 0x48; // 'Come back home' by Seo Taeji & Boys
        public const byte WIZ_FRIEND_PROCESS = 0x49; // Get the status of your friend
        public const byte WIZ_GOLD_CHANGE = 0x4A; // When you get the gold of your enemy.
        public const byte WIZ_WARP_LIST = 0x4B; // Warp List by NPC or Object
        public const byte WIZ_VIRTUAL_SERVER = 0x4C; // Battle zone Server Info packet	(IP, Port)
        public const byte WIZ_ZONE_CONCURRENT = 0x4D; // Battle zone concurrent users request packet
        public const byte WIZ_CORPSE = 0x4e; // To have your corpse have an ID on top of it.
        public const byte WIZ_PARTY_BBS = 0x4f; // For the party wanted bulletin board service..
        public const byte WIZ_MARKET_BBS = 0x50; // For the market bulletin board service...
        public const byte WIZ_KICKOUT = 0x51; // Account ID forbid duplicate connection
        public const byte WIZ_CLIENT_EVENT = 0x52; // Client Event (for quest)
        public const byte WIZ_MAP_EVENT = 0x53; // 클라이언트에서 무슨 에코로 쓰고 있데요.
        public const byte WIZ_WEIGHT_CHANGE = 0x54; // Notify change of weight
        public const byte WIZ_SELECT_MSG = 0x55; // Select Event Message...
        public const byte WIZ_NPC_SAY = 0x56; // Select Event Message...
        public const byte WIZ_BATTLE_EVENT = 0x57; // Battle Event Result
        public const byte WIZ_AUTHORITY_CHANGE = 0x58; // Authority change 
        public const byte WIZ_EDIT_BOX = 0x59; // Activate/Receive info from Input_Box.
        public const byte WIZ_SANTA = 0x5A; // Activate motherfucking Santa Claus!!! :(
        public const byte WIZ_ITEM_UPGRADE = 0x5B;
        public const byte WIZ_PACKET1 = 0x5C;
        public const byte WIZ_PACKET2 = 0x5D;
        public const byte WIZ_ZONEABILITY = 0x5E;
        public const byte WIZ_EVENT = 0x5F;
        public const byte WIZ_STEALTH = 0x60; // stealth related.
        public const byte WIZ_ROOM_PACKETPROCESS = 0x61; // room system
        public const byte WIZ_ROOM = 0x62;
        public const byte WIZ_PACKET3 = 0x63; // new clan
        public const byte WIZ_QUEST = 0x64;
        public const byte WIZ_PACKET4 = 0x65; // login
        public const byte WIZ_KISS = 0x66;
        public const byte WIZ_RECOMMEND_USER = 0x67;
        public const byte WIZ_MERCHANT = 0x68;
        public const byte WIZ_MERCHANT_INOUT = 0x69;
        public const byte WIZ_SHOPPING_MALL = 0x6A;
        public const byte WIZ_SERVER_INDEX = 0x6B;
        public const byte WIZ_EFFECT = 0x6C;
        public const byte WIZ_SIEGE = 0x6D;
        public const byte WIZ_NAME_CHANGE = 0x6E;
        public const byte WIZ_WEBPAGE = 0x6F;
        public const byte WIZ_CAPE = 0x70;
        public const byte WIZ_PREMIUM = 0x71;
        public const byte WIZ_HACKTOOL = 0x72;
        public const byte WIZ_RENTAL = 0x73;
        public const byte WIZ_PACKET5 = 0x74; //s?eli item
        public const byte WIZ_CHALLENGE = 0x75;
        public const byte WIZ_PET = 0x76;
        public const byte WIZ_CHINA = 0x77; // we shouldn't need to worry about this
        public const byte WIZ_KING = 0x78;
        public const byte WIZ_SKILLDATA = 0x79;
        public const byte WIZ_PROGRAMCHECK = 0x7A;
        public const byte WIZ_BIFROST = 0x7B;
        public const byte WIZ_REPORT = 0x7C;
        public const byte WIZ_LOGOSSHOUT = 0x7D;
        public const byte WIZ_PACKET6 = 0x7E;
        public const byte WIZ_PACKET7 = 0x7F;
        public const byte WIZ_RANK = 0x80;
        public const byte WIZ_STORY = 0x81;
        public const byte WIZ_PACKET8 = 0x82;
        public const byte WIZ_PACKET9 = 0x83;
        public const byte WIZ_PACKET10 = 0x84;
        public const byte WIZ_PACKET11 = 0x85;
        public const byte WIZ_MINING = 0x86;
        public const byte WIZ_HELMET = 0x87;
        public const byte WIZ_PVP = 0x88;
        public const byte WIZ_CHANGE_HAIR = 0x89; // Changes hair colour/facial features at character selection
        public const byte WIZ_PACKET12 = 0x8A;
        public const byte WIZ_PACKET13 = 0x8B;
        public const byte WIZ_PACKET14 = 0x8C;
        public const byte WIZ_PACKET15 = 0x8D;
        public const byte WIZ_PACKET16 = 0x8E;
        public const byte WIZ_PACKET17 = 0x8F;
        public const byte WIZ_DEATH_LIST = 0x90;
        public const byte WIZ_CLANPOINTS_BATTLE = 0x91; // not sure
        public const byte WIZ_LOGINCHECKFORV2 = 0x9F;
        public const byte WIZ_TEST_PACKET = 0xff; // Test packet

        public const byte WIZ_GENIE = 0x97;
        public const byte WIZ_USER_INFORMATIN = 0x98;
        public const byte WIZ_ACHIEVE = 0x99;
        public const byte WIZ_EXP_SEAL = 0x9A;
        public const byte WIZ_SP_CHANGE = 0x9B;

        public const byte PARTY_CREATE = 0x01;	// Party Group Create
        public const byte PARTY_PERMIT = 0x02;	// Party Insert Permit
        public const byte PARTY_INSERT = 0x03;	// Party Member Insert
        public const byte PARTY_REMOVE = 0x04;	// Party Member Remove
        public const byte PARTY_DELETE = 0x05;	// Party Group Delete
        public const byte PARTY_HPCHANGE = 0x06;	// Party Member HP change
        public const byte PARTY_LEVELCHANGE = 0x07;	// Party Member Level change
        public const byte PARTY_CLASSCHANGE = 0x08;	// Party Member Class Change
        public const byte PARTY_STATUSCHANGE = 0x09;	// Party Member Status ( disaster, poison ) Change
        public const byte PARTY_REGISTER = 0x0A;	// Party Message Board Register
        public const byte PARTY_REPORT = 0x0B;	// Party Request Message Board Messages
        public const byte PARTY_PROMOTE = 0x1C;	// Promotes user to party leader
        public const byte PARTY_ALL_STATUSCHANGE = 0x1D;	// Sets the specified status of the selected party members to 1.


        public const byte NEWCHAR_SUCCESS = (0);
        public const byte NEWCHAR_NO_MORE = (1);
        public const byte NEWCHAR_INVALID_DETAILS = (2);
        public const byte NEWCHAR_EXISTS = (3);
        public const byte NEWCHAR_DB_ERROR = (4);
        public const byte NEWCHAR_INVALID_NAME = (5);
        public const byte NEWCHAR_BAD_NAME = (6);
        public const byte NEWCHAR_INVALID_RACE = (7);
        public const byte NEWCHAR_NOT_SUPPORTED_RACE = (8);
        public const byte NEWCHAR_INVALID_CLASS = (9);
        public const byte NEWCHAR_POINTS_REMAINING = (10);
        public const byte NEWCHAR_STAT_TOO_LOW = (11);


        ////////////////////////////////////////////////////////////
        // User Status //
        public const byte USER_STANDING = 0X01;		// �� �ִ�.
        public const byte USER_SITDOWN = 0X02;		// �ɾ� �ִ�.
        public const byte USER_DEAD = 0x03;		// ��Ŷ�
        public const byte USER_BLINKING = 0x04;		// ��� ��Ƴ���!!

        public enum InOutType
        {
            INOUT_IN = 1,
            INOUT_OUT = 2,
            INOUT_RESPAWN = 3,
            INOUT_WARP = 4,
            INOUT_SUMMON = 5
        };

        // ---------------------------------------------------------------------
        // AI Server¿Í °ÔÀÓ¼­¹ö°£ÀÇ Npc¿¡ °ü·ÃµÈ ÆÐÅ¶Àº 1¹ø~49¹ø 
        // ---------------------------------------------------------------------
        public const byte AI_SERVER_CONNECT = 1;
        public const byte NPC_INFO_ALL = 2;
        public const byte MOVE_REQ = 3;
        public const byte MOVE_RESULT = 4;
        public const byte MOVE_END_REQ = 5;
        public const byte MOVE_END_RESULT = 6;
        public const byte AG_NPC_INFO = 7;
        public const byte AG_NPC_GIVE_ITEM = 8;
        public const byte AG_NPC_GATE_OPEN = 9;
        public const byte AG_NPC_GATE_DESTORY = 10;
        public const byte AG_NPC_INOUT = 11;
        public const byte AG_NPC_EVENT_ITEM = 12;
        public const byte AG_NPC_HP_REQ = 13;
        public const byte AG_NPC_SPAWN_REQ = 14;	// spawns an NPC/monster at the desired location
        public const byte AG_NPC_REGION_UPDATE = 15;
        public const byte AG_NPC_UPDATE = 16;
        public const byte AG_NPC_KILL_REQ = 17;

        // ---------------------------------------------------------------------
        // AI Server¿Í °ÔÀÓ¼­¹ö°£ÀÇ User, Npc °øÅë °ü·ÃµÈ ÆÐÅ¶Àº 50¹ø~100¹ø 
        // ---------------------------------------------------------------------
        public const byte AG_SERVER_INFO = 50;	// 
        public const byte AG_ATTACK_REQ = 51;	// Attck Packet
        public const byte AG_ATTACK_RESULT = 52;	// Attck Packet
        public const byte AG_DEAD = 53;	// Dead Packet
        public const byte AG_SYSTEM_MSG = 54;	// System message Packet
        public const byte AG_CHECK_ALIVE_REQ = 55;	// Server alive check
        public const byte AG_COMPRESSED_DATA = 56;	// Packet Data compressed
        public const byte AG_ZONE_CHANGE = 57;	// Zone change
        public const byte AG_MAGIC_ATTACK_REQ = 58;	// Magic Attck Packet
        public const byte AG_MAGIC_ATTACK_RESULT = 59;	// Magic Attck Packet
        public const byte AG_USER_INFO_ALL = 60;	// UserÀÇ ¸ðµç Á¤º¸ Àü¼Û
        public const byte AG_LONG_MAGIC_ATTACK = 61;	// Magic Attck Packet
        public const byte AG_PARTY_INFO_ALL = 62;	// PartyÀÇ ¸ðµç Á¤º¸ Àü¼Û
        public const byte AG_HEAL_MAGIC = 63;	// Healing magic
        public const byte AG_TIME_WEATHER = 64;	// time and whether info
        public const byte AG_BATTLE_EVENT = 65;	// battle event
        public const byte AG_COMPRESSED = 66;
    }
}
