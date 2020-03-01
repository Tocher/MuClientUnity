namespace Assets.Network
{
    public interface ITcpHandler
    {
        Packet GetPacket(byte[] tcpPacket);
    }
}
