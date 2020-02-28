using System;
public interface ITcpHandler
{
    Packet GetPacket(byte[] tcpPacket);
}
