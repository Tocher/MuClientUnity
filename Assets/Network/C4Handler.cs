using System;
using System.Linq;

namespace Assets.Network
{
    public class C4Handler : ITcpHandler
    {
        public Packet GetPacket(byte[] tcpPacket)
        {
            return new Packet()
            {
                Type = tcpPacket[0],
                Length = Convert.ToInt32(tcpPacket.Skip(1).Take(2).ToArray()),
                Code = tcpPacket[3],
                SubCode = tcpPacket[4],
                Data = tcpPacket.Skip(5).ToArray()
            };
        }
    }
}
