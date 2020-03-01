namespace Assets.Network
{
    public class Packet
    {
        public byte Type { get; set; }

        public int Length { get; set; }

        public byte[] LengthBytes { get; set; }

        public byte Code { get; set; }

        public byte SubCode { get; set; }

        public byte[] Data { get; set; }

        public bool IsCompleted
        {
            get
            {
                return (Data.Length + 3 + LengthBytes.Length) == Length;
            }
        }
    }
}
