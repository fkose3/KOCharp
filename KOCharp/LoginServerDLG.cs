
using KOCharp.Classes.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KOCharp
{
    public enum LogonOpcodes
    {
        LS_VERSION_REQ = 0x01,
        LS_DOWNLOADINFO_REQ = 0x02,
        LS_CRYPTION = 0xF2,
        LS_LOGIN_REQ = 0xF3,
        LS_MGAME_LOGIN = 0xF4,
        LS_SERVERLIST = 0xF5,
        LS_NEWS = 0xF6,
        LS_UNKF7 = 0xF7,

        NUM_LS_OPCODES
    };

    public enum LoginErrorCode
    {
        AUTH_SUCCESS = 0x01,
        AUTH_NOT_FOUND = 0x02,
        AUTH_INVALID = 0x03,
        AUTH_BANNED = 0x04,
        AUTH_IN_GAME = 0x05,
        AUTH_ERROR = 0x06,
        AUTH_AGREEMENT = 0xF,
        AUTH_FAILED = 0xFF
    }

    public class LoginServerDLG
    {
        public main m_fDialog;
        public short version = 0;
        public List<VERSION> PatchList = new List<VERSION>();
        public string FTP_URL = String.Empty;
        public string FTP_PATH = String.Empty;

        public LoginServerDLG(main main)
        {
            this.m_fDialog = main;

            KODatabase db = new KODatabase();
            foreach(VERSION lstVersion in db.VERSIONs)
            {
                version = version > lstVersion.sVersion ? version : lstVersion.sVersion;
                PatchList.Add(lstVersion);
            }

            main.ProgressList.Items.Add("Login Versiyon : "+version);

            INIReader ini = new INIReader(Environment.CurrentDirectory + "/LogInServer.ini");
            FTP_URL = ini.Read("DOWNLOAD", "URL");
            FTP_PATH = ini.Read("DOWNLOAD", "PATH");

            int ServerCount = ini.GetInt("SERVER_LIST", "COUNT");

            for(int i=0; i<ServerCount; i++)
            {
                SERVER_INFO info = new SERVER_INFO();
                info.strServerIP = ini.GetString("SERVER_LIST", string.Format("SERVER_{0}", i.ToString("00")));
                info.strLanIP = ini.GetString("SERVER_LIST", string.Format("LANIP_{0}", i.ToString("00")));
                info.strServerName = ini.GetString("SERVER_LIST", string.Format("NAME_{0}", i.ToString("00")));
                info.sServerID = ini.GetShort("SERVER_LIST", string.Format("ID_{0}", i.ToString("00")));
                info.sGroupID = ini.GetShort("SERVER_LIST", string.Format("GROUPID_{0}", i.ToString("00")));
                info.sPlayerCap = ini.GetShort("SERVER_LIST", string.Format("PREMLIMIT_{0}", i.ToString("00")));
                info.sFreePlayerCap = ini.GetShort("SERVER_LIST", string.Format("FREELIMIT_{0}", i.ToString("00")));
                info.strKarusKingName = ini.GetString("SERVER_LIST", string.Format("KING1_{0}", i.ToString("00")));
                info.strElMoradKingName = ini.GetString("SERVER_LIST", string.Format("KING2_{0}", i.ToString("00")));
                info.strKarusNotice = ini.GetString("SERVER_LIST", string.Format("KINGMSG1_{0}", i.ToString("00")));
                info.strElMoradNotice = ini.GetString("SERVER_LIST", string.Format("KINGMSG2_{0}", i.ToString("00")));
                ServerList.Add(info);
            }
        }

        private List<SERVER_INFO> ServerList = new List<SERVER_INFO>();

        internal void GetServerList(ref Packet result)
        {
            KODatabase db = new KODatabase();
            short CurrentUserCount = (short)db.CURRENTUSERs.Count();
            result.SetByte((byte)ServerList.Count);

            foreach(SERVER_INFO server in ServerList)
            {
                result.SetString(server.strServerIP);
                result.SetString(server.strLanIP);
                result.SetString(server.strServerName);

                if (CurrentUserCount <= server.sPlayerCap)
                    result.SetShort(CurrentUserCount);
                else
                    result.SetShort(-1);

                result.SetShort(server.sServerID);
                result.SetShort(server.sGroupID);

                result.SetShort(server.sPlayerCap);
                result.SetShort(server.sFreePlayerCap);

                result.SetByte(0);

                result.SetString(server.strKarusKingName);
                result.SetString(server.strKarusNotice);
                result.SetString(server.strElMoradKingName);
                result.SetString(server.strElMoradNotice);
            }
        }

    }

    public struct SERVER_INFO
    {
        public string strServerIP;
        public string strLanIP;
        public string strServerName;
        public short sServerID;
        public short sGroupID;
        public short sPlayerCap;
        public short sFreePlayerCap;
        public string strKarusKingName;
        public string strElMoradKingName;
        public string strKarusNotice;
        public string strElMoradNotice;
    }
}