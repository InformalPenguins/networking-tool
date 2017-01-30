using System;

public class BroadcastServerStrategy : IServerStrategy
{
	public BroadcastServerStrategy()
	{
	}

    public void processText(string text)
    {
        if (text.StartsWith(NetworkConstants.ACTION_BROADCAST))
        {
            broadCast("BROADCAST( " + playerKey + " ):" + text);
        }
        else if (text.StartsWith(NetworkConstants.ACTION_UPDATE_POSITION + " ") || text.StartsWith(NetworkConstants.INPUT_POSITION))
        {
            broadCast(text, senderIpEndPoint);
        }
        else if (text.StartsWith(NetworkConstants.ACTION_SERVER_LOGIN + " "))
        {
            sendMessage(getNextPlayerMessage(), senderIpEndPoint);
        }
        else
        {
            print(playerKey + " - UNKNOWN MESSAGE :: " + text);
        }
    }
}
