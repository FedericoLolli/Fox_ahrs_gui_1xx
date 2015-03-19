using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AHRSInterface
{
    public class Packet
    {
        public byte PacketType { get; set; }
        public byte DataLength { get; set; }
        public byte[] Data { get; set; }
    }
}
