
using KOCharp.Classes.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KOCharp
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        List<Thread> m_thdsLoginServer = new List<Thread>();
        Thread GameServerThread = null;
        private bool isLoginServerOpen = false;
        private bool isGameServerOpen = false;

        private void btnLoginServer_Click(object sender, EventArgs e)
        {
            if (!isLoginServerOpen)
            {
                m_thdsLoginServer.Clear();
                
                LoginServerDLG dlg = new LoginServerDLG();
                for (int i = 0; i < 10; i++)
                    m_thdsLoginServer.Add(THREADCALL_LOGIN(15100+ i, dlg));

                txtActiveLoginPort.Text = m_thdsLoginServer.Count().ToString("00");
                btnLoginServer.Enabled = false;
            }
            else
            {
                foreach (Thread thd in m_thdsLoginServer)
                {
                    try {
                        thd.Abort();
                        btnLoginServer.Text = "Login Server Başlat";
                    }
                    catch(Exception ex)
                    {
                        ProgressList.Items.Add(ex.Message);
                    }
                }
            }


            isLoginServerOpen = !isLoginServerOpen;
        }

        public Thread THREADCALL_LOGIN(int Port, LoginServerDLG mainLogin)
        {
            Thread thd = new Thread(() => { new KOSocket(mainLogin).Read(Port); });
            thd.Start();
            ProgressList.Items.Add(string.Format("{0} Numaralı port başlatıldı.", Port));
            return thd;
        }
        public Thread THREADCALL_GAME(int Port)
        {
            Thread thd = new Thread(() => { new KOSocket(new GameServerDLG(this)).Read(Port); });
            thd.Start();
            ProgressList.Items.Add(string.Format("{0} Numaralı port başlatıldı.", Port));
            return thd;
        }

        private void btnGameServer_Click(object sender, EventArgs e)
        {
            if (!isGameServerOpen)
            {
                GameServerThread = THREADCALL_GAME(int.Parse(txtGameserverPort.Text));
                btnGameServer.Enabled = false;
            }
            else
            {
                GameServerThread.Abort();
                isGameServerOpen = false;
                ProgressList.Items.Add(string.Format("Game server kapatıldı"));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            KODatabase db = new KODatabase();
            int result = db.CREATE_NEW_CHAR("fkose3", 3, "teswet2", 1, 10, 1, 1, 60,60,60,60,60);
            MessageBox.Show(result.ToString());
        }
    }
}
