using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManagerListener : MonoBehaviour {
    public void JoinTeam(string team){
        Debug.Log ("LobbyManagerListener.JoinTeam: " + team);
    }
}
