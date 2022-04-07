using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace SERVER_TCP
{

    class SerCnt
    {
        private static int BUF_SZ = 1024;
        private Socket sk;
        private byte[] buf;

        public SerCnt(int port)
        {  
            IPEndPoint iep = new IPEndPoint(IPAddress.Any, port);
            Socket sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sk.Bind(iep);
            sk.Listen(5);
            Console.WriteLine("san sang, dang cho ket noi tu client");
            this.sk = sk.Accept();
        }

        ~SerCnt()
        {
            sk.Shutdown(SocketShutdown.Both);
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
            Console.WriteLine(1);
            this.sendMsg(n.ToString());
        }

        private int getNum()
        {
            return int.Parse(getMsg());
        }
        

        public void process()
        {
            sendNum(4);
            sendNum(5);
            sendNum(6);
            sendNum(7);
            sendNum(8);
            sendNum(9);
            sendNum(10);
            sendNum(11);
        }

        static void Main(String[] args)
        {
            SerCnt serCnt = new SerCnt(8888);
            serCnt.process();
            Console.Read();
        }
    }
}
