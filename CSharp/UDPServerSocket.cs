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

        // CHO NH???P 1 ??O???N V??N, T??M T??? D??I NH???T TRONG ??O???N V??N ???? V?? V??? TR?? C???A N??
        // y??u c???u kh??ng d??ng max ho???c th?? vi???n nh?? tr??n google
        /**ch???y gi???i thu???t theo ki???u dictionary
         * v?? d??? nh???p :      t??i t??n l?? ho??ng minh tu???n
         * th??:             key        |  valueIndex
         *                 t??i(len=3)         0
         *                 t??n(len=3)         4
         *                 l??(len=2)          8
         *              ho??ng(len=5)         11       --> k???t qu??? l?? ????y
         *               minh(len=4)         17
         *               tu???n(len=4)         22
         *               
         * ?? t?????ng gi???i thu???t:
         *        ch???y t??? ?????u text ?????n cu???i text, t??m c??ch l???y c??c ch??? ??em pushBack v??o listDictString
         *        
         *   c??ch th???c hi???n gi???i thu???t: d??ng k??? thu???t c??? hi???u (flag) khi ch???y v??ng l???p
         *   ????? b???t ???????c m???t ch??? th?? c???n 2 c??? hi???u:
         *      c??? hi???u ??ang b???t ???????c k?? t???:  flag_is_catching_char
         *      c??? hi???u ??ang b???t k?? t??? ?????u ti??n, t???c l?? ????nh d???u ????y l?? ch??? v?? get ???????c idx:  flag_first
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
