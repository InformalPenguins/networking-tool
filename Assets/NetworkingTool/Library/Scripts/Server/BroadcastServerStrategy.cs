using System;
using System.Net;
using UnityEngine;

public class BroadcastServerStrategy : IServerStrategy
{
    public BroadcastServerStrategy()
    {
    }
    public void Start()
    {
        #if UNITY_EDITOR
        Debug.Log("Starting BroadcastServerStrategy");
        #endif
    }
    public override void processText(string text, Player player)
    {
        if (text == null || text.Length == 0)
        {
            return;
        }
        udpServer.broadCast(text);
    }

    public override void setUdpServer(UDPServer udpServer)
    {
        this.udpServer = udpServer;
    }
}
