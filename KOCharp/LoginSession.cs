
using KOCharp.Classes.Database;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KOCharp
{
    class LoginSession
    {
        private LoginServerDLG g_pMain;
        private Socket socket;
        #region NetworkListen
        public LoginSession(Socket soc, LoginServerDLG g_pLogin)
        {
            this.socket = soc;
            this.g_pMain = g_pLogin;
            
            ReceiveUser();
        }

        private void ReceiveUser()
        {
            Thread ThreadListener = new Thread(new ThreadStart(delegate { OnClientDataReceived(); }));
            ThreadListener.Start();

            while (socket.Connected)
            {
                Thread.Sleep(45);
            }

            OnClientDisconnect();
        }

        private void OnClientDisconnect()
        {
            if (socket.Connected)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                Thread.CurrentThread.Abort(0);
            }
        }

        public void OnClientDataReceived()
        {
            int bytes = 0;

            do
            {
              try
              {
                    byte[] read_byte = new byte[1024 * 8];

                    bytes = socket.Receive(read_byte, 1024 * 8, 0);

                    Packet pkt = new Packet(read_byte, socket);

                    Parsing(pkt);
               }
               catch
               {
                    OnClientDisconnect();
                    break;
               }
            }
            while (true);
        }

        public void Send(Packet resut)
        {
            resut.Send(socket);
        }
        #endregion

        private void Parsing(Packet pkt)
        {
            Debug.WriteLine("Clientten İstek geldi");
            byte command = pkt.GetByte();
            
            switch ((LogonOpcodes)command)
            {
                case LogonOpcodes.LS_VERSION_REQ:
                    HandleVersion(pkt);
                    break;
                case LogonOpcodes.LS_DOWNLOADINFO_REQ:
                    HandlePatches(pkt);
                    break;
                case LogonOpcodes.LS_CRYPTION:
                    HandleSetEncryptionPublicKey(pkt);
                    break;
                case LogonOpcodes.LS_LOGIN_REQ:
                    HandleLogin(pkt);
                    break;
                case LogonOpcodes.LS_MGAME_LOGIN:

                    break;
                case LogonOpcodes.LS_SERVERLIST:
                    HandeleServerList(pkt);
                    break;
                case LogonOpcodes.LS_NEWS:
                    HandleNews(pkt);
                    break;
                case LogonOpcodes.LS_UNKF7:

                    break;
            }
        }

        private void HandleNews(Packet pkt)
        {
            Packet result = new Packet((byte)LogonOpcodes.LS_NEWS);
            result.SetString("Login Notice").SetString("<empty>");
            
            Send(result);
        }

        private void HandeleServerList(Packet pkt)
        {
            short echo = pkt.GetShort();

            Packet result = new Packet((byte)LogonOpcodes.LS_SERVERLIST);
            result.SetShort(echo);

            g_pMain.GetServerList(ref result);

            Send(result);
        }

        private void HandleVersion(Packet pkt)
        {
            Packet result = new Packet((byte)LogonOpcodes.LS_VERSION_REQ);
            result.SetShort(g_pMain.version);
            Send(result);
        }

        private void HandlePatches(Packet pkt)
        {
            short cversion = pkt.GetShort();

            Packet result = new Packet((byte)LogonOpcodes.LS_DOWNLOADINFO_REQ);

            List<VERSION> versions = new List<VERSION>();

            foreach (VERSION vers in g_pMain.PatchList)
            {
                if (vers.sVersion > cversion)
                    versions.Add(vers);
            }

            result.SetString(g_pMain.FTP_URL).SetString(g_pMain.FTP_PATH).SetShort(short.Parse(versions.Count.ToString()));

            foreach (VERSION vrs in versions)
            {
                result.SetString(vrs.strFilename);
            }

            Send(result);
        }

        private void HandleSetEncryptionPublicKey(Packet pkt)
        {
            Packet result = new Packet((byte)LogonOpcodes.LS_CRYPTION);
            result.SetInt64(0);
            Send(result);
        }

        private void HandleLogin(Packet pkt)
        {
            KODatabase db = new KODatabase();
            string AccountID = pkt.GetString(), Passwd = pkt.GetString();
            short resultCode = 1;

            if (AccountID == string.Empty || AccountID.Length >= Define.MAX_ID_SIZE ||
                Passwd == string.Empty || Passwd.Length >= Define.MAX_PW_SIZE)
                resultCode = 2;
            else
                resultCode = db.TB_USER.Where(u=> u.strAccountID == AccountID && u.strPasswd == Passwd).Count() > 0 ? (short)1 : (short)2 ;

            Packet result = new Packet((byte)LogonOpcodes.LS_LOGIN_REQ);
            result.SetByte((byte)resultCode);
            if (resultCode == 1)
            {
                result.SetShort((short)db.ACCOUNT_PREMIUM(AccountID));
                result.SetString(AccountID);
            }

            Send(result);

            Debug.WriteLine("Kullanıcı girişi : {1} - {0}", resultCode, AccountID);
        }
    }
}
