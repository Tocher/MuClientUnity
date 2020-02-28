using System;
using System.Collections.Generic;
using System.Linq;

public static class TcpHelper
{
    public static List<Packet> Handle(byte[] tcpPacket, Packet lastNotCompletedPacket)
    {
        var packets = new List<Packet>();
        var notUsed = tcpPacket;

        while (notUsed.Length > 0)
        {
            //if (lastNotCompletedPacket != null)
            //{

            //}

            var packet = GetSinglePacket(tcpPacket);
            var isCompleted = packet.IsCompleted;

            packets.Add(packet);

            if (!isCompleted || packet.Length == tcpPacket.Length)
            {
                return packets;
            }

            notUsed = notUsed.Skip(packets.Sum(p => p.Length)).ToArray();
        }

        return packets;
    }

    private static Packet GetSinglePacket(byte[] used)
    {
        var handler = GetHandler(used[0]);
        var packet = handler.GetPacket(used);

        return packet;
    }

    private static ITcpHandler GetHandler(byte type)
    {
        switch (type)
        {
            case 0xc1:
                return new C1Handler();
            case 0xc2:
                return new C2Handler();
            case 0xc3:
                return new C3Handler();
            case 0xc4:
                return new C4Handler();
            default:
                throw new Exception("packet govno");
        }
    }
}
