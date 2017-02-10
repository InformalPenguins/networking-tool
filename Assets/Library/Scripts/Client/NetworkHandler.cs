using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkHandler : MonoBehaviour, INetworkHandler
{
    void INetworkHandler.processMessage(string text)
    {
        if (text == null || text.Length == 0)
        {
            return;
        }
        string[] parameters = text.Split (' ');
        int idx = 0;
        int startAction = int.Parse(parameters [idx++]);
        if(startAction == NetworkMessageHelper.ACTION_SERVER_LOGIN){
            int identifier = int.Parse(parameters [idx++]);
            //Identify 
            UDPClient.IDENTIFIER = identifier;
        } else {
            //Leave it for the UI processor to handle.
            MainThreadProcessor.Instance().Enqueue(MainThreadProcessor.Instance().processMessage(text));
        }
    }

}
