using System;
using System.Net;

public interface IServerStrategy
{
    void processText(string text, IPEndPoint ipEndPoint);
    void setUdpServer(UDPServer server);
}
