using System;
using System.Net;
using UnityEngine;

public class SimpleServerStrategy : IServerStrategy
{
    public SimpleServerStrategy()
    {
        Debug.Log("Starting: SimpleServerStrategy");
    }

    public override void setUdpServer(UDPServer udpServer) {
        this.udpServer = udpServer;
    }

    public override void processText(string text, Player player)
    {
        if (udpServer == null || text == null || text.Trim().Length == 0) {
            return;
        }
        string[] parameters = text.Split (NetworkMessageHelper.separator);
        int idx = 0;
        int startAction = int.Parse(parameters[idx++]);

        if (startAction == NetworkMessageHelper.ACTION_BROADCAST)
        {
            udpServer.broadCast(text);
        }
        else if (startAction == NetworkMessageHelper.ACTION_UPDATE_POSITION || startAction == NetworkMessageHelper.INPUT_POSITION)
        {
            udpServer.broadCast(text);
        }
        else if (startAction == NetworkMessageHelper.ACTION_SERVER_LOGIN)
        {
            udpServer.sendMessage(NetworkMessageHelper.BuildMessage(NetworkMessageHelper.ACTION_SERVER_LOGIN, player.id), player);
        }
    }
}
