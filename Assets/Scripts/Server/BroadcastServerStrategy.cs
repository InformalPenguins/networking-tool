using System;
using System.Net;

public class BroadcastServerStrategy : IServerStrategy
{
    UDPServer udpServer;
    public BroadcastServerStrategy()
    {
    }
    public void setUdpServer(UDPServer udpServer) {
        this.udpServer = udpServer;
    }

    public void processText(string text, IPEndPoint senderIpEndPoint)
    {
        if (text == null || text.Length == 0)
        {
            return;
        }
        udpServer.broadCast(text);
    }
}
