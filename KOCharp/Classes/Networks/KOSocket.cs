using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace KOCharp
{
    public class KOSocket : Define
    {
        GameServerDLG g_pMain = null;
        LoginServerDLG g_pLogin = null;
        bool isLoginServer;
        public KOSocket(GameServerDLG g_pMain)
        {
            this.g_pMain = g_pMain;
            isLoginServer = false;
        }

        public KOSocket(LoginServerDLG g_pLoginServer)
        {
            isLoginServer = true;
            this.g_pLogin = g_pLoginServer;
        }

        public void Read(int PORT)
        {
            TcpListener PortReader = new TcpListener(PORT);
            try
            {
                byte[] read_Data = new byte[255];


                while (true)
                {

                    PortReader.Start();
                    read_Data = new byte[MAX_READ_LEN];

                    Socket soc = null;

                    soc = PortReader.AcceptSocket();

                    if (isLoginServer)
                    {
                        Thread thd = new Thread(() => { new LoginSession(soc, g_pLogin); });
                        thd.Start();
                    
                    }
                    else
                    {
                        Thread thd = new Thread(() => { new User(soc, g_pMain); });
                        thd.Start();
                    }
                    PortReader.Stop();
                }

           }
           catch (Exception ex)
           {
               Console.WriteLine(ex.Message);
           }
        }
    }
}
