using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerDebug : MonoBehaviour {
    public ServerGameLogic serverGameLogic;
    private UDPServer udpServer;
    public InputField ServerUIText;
    // Use this for initialization
    void Start () {
        if (serverGameLogic != null)
        {
            udpServer = serverGameLogic.getUdpServer();
        }
    }

    // Update is called once per frame
    void Update() {
        if (serverGameLogic != null)
        {
            udpServer = serverGameLogic.getUdpServer();
        }
        else {
            Destroy(gameObject);
        }
    }

    public void debugBroadCast()
    {
        string msg = ServerUIText.text;
        udpServer.broadCast(msg);
    }
}
