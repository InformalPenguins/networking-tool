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
        if (text.StartsWith(NetworkConstants.ACTION_BROADCAST))
        {
            udpServer.broadCast(text);
        }
        else if (text.StartsWith(NetworkConstants.ACTION_UPDATE_POSITION + " ") || text.StartsWith(NetworkConstants.INPUT_POSITION))
        {
            udpServer.broadCast(text, senderIpEndPoint);
        }
        else if (text.StartsWith(NetworkConstants.ACTION_SERVER_LOGIN + " "))
        {
            udpServer.sendMessage(getNextPlayerMessage(), senderIpEndPoint);
        }
        else
        {
            udpServer.print(playerKey + " - UNKNOWN MESSAGE :: " + text);
        }
    }
    private int currentPlayer = 0;
    private string getNextPlayerMessage()
    {
        //TODO: Rework this assign of player
        String nextPlayerMessage = NetworkConstants.ACTION_SERVER_LOGIN + " " + currentPlayer++;
        return nextPlayerMessage;
    }
}
