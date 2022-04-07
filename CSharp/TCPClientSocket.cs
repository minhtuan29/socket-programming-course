using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace CLIENT_TPC
{
    class CliCnt
    {
        private static int BUF_SZ = 1024;

        private Socket sk;
        private byte[] buf;


        public CliCnt(string ip,int port)
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse(ip), port);
            sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("dang ket noi");
            sk.Connect(iep);
            Console.WriteLine("ket noi thanh cong");
        }


        ~CliCnt()
        {
            sk.Disconnect(true);
            sk.Close();
        }


        private string getMsg()
        {
            buf = new byte[BUF_SZ];
            return Encoding.ASCII.GetString(buf, 0, sk.Receive(buf));
        }

        private void sendMsg(string msg)
        {
            buf = Encoding.ASCII.GetBytes(msg);
            sk.Send(buf, buf.Length, SocketFlags.None);
        }

        private void sendNum(int n)
        {
            Console.WriteLine(n);
            this.sendMsg(n.ToString());
        }

        private int getNum()
        {
            return int.Parse(getMsg());
        }
      


        public void process()
        {
            Console.WriteLine("so : " + this.getNum());
            Console.WriteLine("so : " + this.getNum());
            Console.WriteLine("so : " + this.getNum());
            Console.WriteLine("so : " + this.getNum());
            Console.WriteLine("so : " + this.getNum());
            Console.WriteLine("so : " + this.getNum());
            Console.WriteLine("so : " + this.getNum());
            Console.WriteLine("so : " + this.getNum());
        }


        static void Main(string[] args)
        {
            CliCnt cliCnt = new CliCnt("127.0.0.1",8888);
            cliCnt.process();
            Console.Read();
        }
    }
}
