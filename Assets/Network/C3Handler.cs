using System;
using System.Linq;

public class C3Handler : ITcpHandler
{
    public C3Handler()
    {
    }

    public Packet GetPacket(byte[] tcpPacket)
    {
        return new Packet()
        {
            Type = tcpPacket[0],
            Length = tcpPacket[1],
            Code = tcpPacket[2],
            SubCode = tcpPacket[3],
            Data = tcpPacket.Skip(4).ToArray(),
        };
    }
}
