using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerGameLogic : MonoBehaviour {
    private UDPServer udpServer;
    void Start () {
        udpServer = GetComponent<UDPServer>();
    }
    bool notifyStart = true;
    void Update()
    {
        if (notifyStart && udpServer.getPlayersList().Keys.Count > 1)
        {
            notifyStart = false;
            int[] hv = BallController.CalculateForces();
            udpServer.broadCast(NetworkMessageHelper.BuildMessage(NetworkMessageHelper.ACTION_START, hv[0], hv[1]));
        }
    }
}
