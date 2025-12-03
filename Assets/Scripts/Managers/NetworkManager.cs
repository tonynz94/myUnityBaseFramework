using DummyClient;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;
using UnityEditor.Sprites;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    ServerSession _session = new ServerSession();

    public void Send(ArraySegment<byte> sendBuff)
    {
        _session.Send(sendBuff);
    }

    public void Send(List<ArraySegment<byte>> sendBuffList)
    {
        _session.Send(sendBuffList);
    }


    public void ConnectServer()
    {
        // DNS (Domain Name System)
        string host = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        //IPAddress ipAddr = ipHost.AddressList[0];

        string strIPAddress = "192.168.55.46";
        Debug.Log(strIPAddress + " try Connect");
        IPAddress ipAddr = IPAddress.Parse(strIPAddress);
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

        Connector connector = new Connector();

        connector.Connect(endPoint,
            () => { return _session; },
            1);
    }

    public void Update()
    {
        List<IPacket> list = PacketQueue.Instance.PopAll();
        foreach (IPacket packet in list)
            PacketManager.Instance.HandlePacket(_session, packet);
    }
}
