using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;



namespace NETSOCKET_BYTE_SERVER_UDP
{

    class Lst
    {
        class Node
        {
            public int value;
            public Node next;

            public Node(int value)
            {
                this.value = value;
                this.next = null;
            }
        }


        Node head;
        Node tail;


        public Lst()
        {
            head = null;
            tail = null;
        }


        private bool isEmpty()
        {
            return head == null && tail == null;
        }


        public void pushBack(int newValue)
        {
            if (isEmpty())
            {
                head = tail = new Node(newValue);
                return;
            }
            Node newNode = new Node(newValue);
            tail.next = newNode;
            tail = newNode;
        }


        public int len()
        {
            int count = 0;
            for (Node cur = head; cur != null; cur = cur.next)
                count++;
            return count;
        }


        public int at(int idx)
        {
            Node cur = head;
            for (int i = 0; i < idx; i++)
                cur = cur.next;
            return cur.value;
        }

    }




    
    class StringListDict
    {
        StringNode head;
        StringNode tail;

        public class StringNode
        {
            public string key;
            public int valueIndex;
            public StringNode next;
            public StringNode(string key, int valueIndex)
            {
                this.key = key;
                this.valueIndex = valueIndex;
                this.next = null;
            }
            public StringNode()
            {
                this.key = "";
                this.valueIndex = 0;
                this.next = null;
            }
        }


        public StringListDict()
        {
            head = null;
            tail = null;
        }


        private bool isEmpty()
        {
            return head == null && tail == null;
        }


        public void pushBack(string _key, int _valueIndex)
        {
            if (isEmpty())
            {
                head = tail = new StringNode(_key, _valueIndex) ;
                return;
            }

            StringNode newElement = new StringNode(_key, _valueIndex);
            tail.next = newElement;
            tail = newElement;
        }


        public int len()
        {
            int count = 0;
            for (StringNode cur = head; cur != null; cur = cur.next)
                count++;
            return count;
        }


        public StringNode at(int idx)
        {
            StringNode cur = head;
            for (int i = 0; i < idx; i++)
                cur = cur.next;
            return cur;
        }
    }



    class SerCnt
    {
        
        private static int BUF_SZ = 1024;
        private Socket sk;
        private EndPoint remote;
        private byte[] buf;


        public SerCnt(string ip, int port)
        {
            sk = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sk.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            remote = (EndPoint)new IPEndPoint(IPAddress.Any, 0);
            buf = new byte[BUF_SZ];
        }


        ~SerCnt()
        {
            this.sk.Close();
        }

        private string getMsg()
        {
            buf = new byte[BUF_SZ];
            return Encoding.ASCII.GetString(buf, 0, sk.ReceiveFrom(buf, ref remote));
        }


        private int getNum()
        {
            return int.Parse(getMsg());
        }


        private void sendNum(int n)
        {
            this.sendMsg(n.ToString());
        }


        private void sendMsg(string msg)
        {
            buf = Encoding.ASCII.GetBytes(msg);
            sk.SendTo(buf, 0, buf.Length, SocketFlags.None, remote);
        }

        // CHO NHẬP 1 ĐOẠN VĂN, TÌM TỪ DÀI NHẤT TRONG ĐOẠN VĂN ĐÓ VÀ VỊ TRÍ CỦA NÓ
        // yêu cầu không dùng max hoặc thư viện như trên google
        /**chạy giải thuật theo kiểu dictionary
         * ví dụ nhập :      tôi tên là hoàng minh tuấn
         * thì:             key        |  valueIndex
         *                 tôi(len=3)         0
         *                 tên(len=3)         4
         *                 là(len=2)          8
         *              hoàng(len=5)         11       --> kết quả là đây
         *               minh(len=4)         17
         *               tuấn(len=4)         22
         *               
         * ý tưởng giải thuật:
         *        chạy từ đầu text đến cuối text, tìm cách lấy các chữ đem pushBack vào listDictString
         *        
         *   cách thực hiện giải thuật: dùng kỹ thuật cờ hiệu (flag) khi chạy vòng lặp
         *   để bắt được một chữ thì cần 2 cờ hiệu:
         *      cờ hiệu đang bắt được kí tự:  flag_is_catching_char
         *      cờ hiệu đang bắt kí tự đầu tiên, tức là đánh dấu đây là chữ và get được idx:  flag_first
         */
        public void process()
        {
            while (true)
            {
                StringListDict stringListDict = new StringListDict();

                string text = this.getMsg();
                // process
                text += " ";
                bool flag_is_catching_char = true;
                bool flag_is_end_char_of_word = false;
                string insert = "";

                for (int i = 0; i < text.Length-1; i++)
                {
                    if (text[i] != ' ')
                        flag_is_catching_char = true;
                    else
                        flag_is_catching_char = false;

                    if (text[i] != ' ' && text[i + 1] == ' ')
                        flag_is_end_char_of_word = true;
                    else
                        flag_is_end_char_of_word = false;


                    if (flag_is_catching_char)
                    {
                        if (!flag_is_end_char_of_word)
                        {
                            insert += text[i].ToString();
                        }
                        else
                        {
                            insert += text[i].ToString();
                            stringListDict.pushBack(insert, i-insert.Length+1);
                            insert = "";
                        }  
                    }
                }

                int len = stringListDict.len();
                int max = 0;
                for(int i = 0; i < len; i++)
                {
                    if (stringListDict.at(i).key.Length > max)
                        max = stringListDict.at(i).key.Length;
                }

                
                for (int i = 0; i < len; i++)
                {
                    if (stringListDict.at(i).key.Length == max)
                    {
                        this.sendMsg(stringListDict.at(i).key);
                        this.sendNum(stringListDict.at(i).valueIndex);
                    }
                }


            }
        }



        static void Main(string[] args)
        {
            SerCnt serCnt = new SerCnt("127.0.0.1", 8888);
            serCnt.process();
        }


    }

}
