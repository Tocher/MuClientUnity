using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Assets.Network;
using UnityEngine;

namespace Assets
{
    public class TcpConnector : MonoBehaviour
    {
        internal Boolean socketReady = false;
        TcpClient mySocket;
        NetworkStream theStream;
        StreamWriter theWriter;
        StreamReader theReader;

        String Host = "64.225.71.233";
        Int32 Port = 44405;
        byte[] ServerList = { 0xc1, 0x04, 0xF4, 0x06 };
        byte[] ServerInfo = { 0xc1, 0x05, 0xF4, 0x03, 0x00 };

        // Start is called before the first frame update
        void Start()
        {
            this.setupSocket();
        }

        // Update is called once per frame
        void Update()
        {
            this.readSocket();
        }

        public void setupSocket() {
            Debug.Log("Setup");

            try {
                mySocket = new TcpClient(Host, Port);
                theStream = mySocket.GetStream();
                theWriter = new StreamWriter(theStream);
                theReader = new StreamReader(theStream);
                theStream.ReadTimeout = 1;
                socketReady = true;             
            }
            catch (Exception e) {
                Debug.Log("Socket error: " + e);
            }
        }

        public void writeSocket(string theLine) {
            if (!socketReady)
                return;
            String foo = theLine + "\r\n";
            theWriter.Write(foo);
            theWriter.Flush();
        }
        public void readSocket() {
            if (!socketReady)
                return;

            if (theStream.DataAvailable)
            {
                byte[] data = new byte[256];
                int bytes = theStream.Read(data, 0, data.Length);
                byte[] packet = new byte[bytes];

                Array.Copy(data, 0, packet, 0, bytes);

                var packets = TcpHelper.Handle(packet, null).Where(p => p.IsCompleted);

                Debug.Log("EEE");

                foreach (Packet pac in packets)
                {
                    Debug.Log(ToHexString(pac.Data));
                }

                //Array.Copy(data, 0, packet, 0, bytes);

                //Debug.Log(ToHexString(packet));

                //if (packet[0] == 0xc1 && packet[1] == 0x04 && packet[2] == 0x00 && packet[3] == 0x01)
                //{
                //    Debug.Log("HELLO");

                //    //theWriter.Write(ServerList);
                //    theStream.Write(ServerList, 0, 4);
                //}

                //if (packet[0] == 0xc1)
                //{
                //    // Server info
                //    if (packet[2] == 0xf4 && packet[3] == 0x03)
                //    {
                //        int serverInfoSize = Convert.ToInt32(packet[1]);

                //        byte IpStartIndex = 4;

                //        var ipPlain = ExtractString(packet, IpStartIndex, 16, Encoding.UTF8);
                //        var ip = IPAddress.Parse(ipPlain);
                //        var port = MakeWordBigEndian(packet, serverInfoSize - 2);

                //        Debug.Log(ip);
                //        Debug.Log(port);
                //    }
                //}

                //if (packet[0] == 0xc2)
                //{
                //    // Server list
                //    if (packet[3] == 0xf4 && packet[4] == 0x06)
                //    {
                //        theStream.Write(ServerInfo, 0, ServerInfo.Length);
                //    }
                //}
            }
        }
        public void closeSocket() {
            if (!socketReady)
                return;
            theWriter.Close();
            theReader.Close();
            mySocket.Close();
            socketReady = false;
        }

        public string ToHexString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace('-', ' ');
        }

        public string ExtractString(byte[] array, int startIndex, int maximumBytes, Encoding encoding)
        {
            int count = array.Skip(startIndex).Take(maximumBytes).TakeWhile(b => b != 0).Count();
            return encoding.GetString(array, startIndex, count);
        }

        public ushort MakeWordBigEndian(byte[] array, int startIndex)
        {
            return (ushort)((array[startIndex + 1] << 8) | array[startIndex]);
        }

        //public void SetShortBigEndian(Span<byte> data, ushort value)
        //{
        //    data[0] = (byte)(value & 0xFF);
        //    data[1] = (byte)((value >> 8) & 0xFF);
        //}
    }
}
