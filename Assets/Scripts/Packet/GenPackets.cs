using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using ServerCore;

public enum PacketID
{
}

public interface IPacket
{
    ushort Protocol { get; }
    void Read(ArraySegment<byte> segment);
    ArraySegment<byte> Write();
}


