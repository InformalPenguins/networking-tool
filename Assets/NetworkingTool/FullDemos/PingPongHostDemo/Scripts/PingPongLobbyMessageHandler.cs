using Assets.NetworkingTool.Library.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongLobbyMessageHandler : MonoBehaviour {

    //Messages from LobbyManager
    public void JoinTeam(string team)
    {
    }
    public void Spectate()
    {
    }
    public void ErrorCallback(string msg)
    {
    }
    public void GetPlayersList(List<LobbyPlayer> playersList)
    {
        //foreach (LobbyPlayer player in playersList)
        //{
        //}
    }
    public void LobbyStart()
    {
    }
    public void LobbyQuit()
    {
    }
    //END LobbyManager messages
}
