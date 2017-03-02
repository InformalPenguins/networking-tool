using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManagerListener : MonoBehaviour {
    public void JoinTeam(string team){
        Debug.Log ("LobbyManagerListener.JoinTeam: " + team);
    }
    public void Spectate()
    {
        Debug.Log("LobbyManagerListener.Spectate");
    }
    public void ErrorCallback(string msg)
    {
        Debug.Log("LobbyManagerListener.Error: " + msg);
    }
}
