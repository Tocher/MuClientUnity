using System;
using System.Linq;

public class C4Handler : ITcpHandler
{
    public C4Handler()
    {
    }

    public Packet GetPacket(byte[] tcpPacket)
    {
        return new Packet()
        {
            Type = tcpPacket[0],
            Length = Convert.ToInt32(tcpPacket.Skip(1).Take(2).ToArray()),
            Code = tcpPacket[3],
            SubCode = tcpPacket[4],
            Data = tcpPacket.Skip(5).ToArray(),
            Source = tcpPacket,
        };
    }
}
