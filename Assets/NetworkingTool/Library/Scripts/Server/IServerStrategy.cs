using System;
using System.Net;
using UnityEngine;

public abstract class IServerStrategy : MonoBehaviour
{
    protected UDPServer udpServer;
    public abstract void processText(string text, Player player);
    public abstract void setUdpServer(UDPServer server);
}
