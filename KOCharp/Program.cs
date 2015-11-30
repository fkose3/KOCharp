using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KOCharp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Console.Title = "Knight Online Server";
            LoginServerDLG dlg = new LoginServerDLG();
            for (int i = 0; i < 10; i++)
                THREADCALL_LOGIN(15100 + i, dlg);

            THREADCALL_GAME(15001);
        }

        public static Thread THREADCALL_LOGIN(int Port, LoginServerDLG mainLogin)
        {
            Thread thd = new Thread(() => { new KOSocket(mainLogin).Read(Port); });
            thd.Start();
            Console.WriteLine(string.Format("Login Server : {0} Numaralı port başlatıldı.", Port));
            return thd;
        }
        public static Thread THREADCALL_GAME(int Port)
        {
            Thread thd = new Thread(() => { new KOSocket(new GameServerDLG(null)).Read(Port); });
            thd.Start();
            Console.WriteLine(string.Format("Game Server : {0} Numaralı port başlatıldı.", Port));
            return thd;
        }
    }
}
