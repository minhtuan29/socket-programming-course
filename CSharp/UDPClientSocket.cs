using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace NETSOCKET_BYTE_CLIENT_UDP
{



    class CliCnt
    {
        
        private static int BUF_SZ = 1024;
        private Socket sk;
        private byte[] buf;
        private EndPoint remote;

        public CliCnt(string ip, int port)
        {
            remote = (EndPoint)new IPEndPoint(IPAddress.Parse(ip), port);
            sk = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            buf = new byte[BUF_SZ];
        }

        ~CliCnt()
        {
            sk.Close();
        }

        private void sendMsg(string msg)
        {
            buf = Encoding.ASCII.GetBytes(msg);
            sk.SendTo(buf, remote);
        }

        private string getMsg()
        {
            buf = new byte[BUF_SZ];
            return Encoding.ASCII.GetString(buf, 0, sk.ReceiveFrom(buf, ref remote));
        }

        public void process()
        {
            while (true)
            {
                Console.Write("gui tin nhan: ");
                this.sendMsg(Console.ReadLine());
                string maxWord = getMsg();
                int maxWordIndex = int.Parse(getMsg());
                Console.WriteLine(maxWord + " | index: " + maxWordIndex.ToString());
            }
            
        }

        static void Main(string[] args)
        {
            CliCnt cliCnt = new CliCnt("127.0.0.1", 8888);
            cliCnt.process();
        }

    }

}
