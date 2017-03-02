using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugLobbyManager : MonoBehaviour {
    public Text log;
    public InputField userName;
    public LobbyManager lobbyManager;
    private void Start()
    {
        lobbyManager.SetLobbyMessageHandler(gameObject);
    }

    public void SendJoinA()
    {
        string newUserName = userName.text;
        int id = int.Parse(newUserName);
        lobbyManager.JoinTeam("A", id);
    }
    public void SendJoinB()
    {
        string newUserName = userName.text;
        int id = int.Parse(newUserName);
        lobbyManager.JoinTeam("B", id);
    }
    public void SendSpectate()
    {
        string newUserName = userName.text;
        int id = int.Parse(newUserName);
        lobbyManager.Spectate(id);
    }
    int counter = 0;
    public void SendAddUser()
    {
        string newUserName = userName.text;
        lobbyManager.AddPlayer(counter++, newUserName);
    }
    public void SendAssign()
    {
        string newUserName = userName.text;
        int id = int.Parse(newUserName);
        lobbyManager.AssignPlayer(id);
    }

    //Messages from LobbyManager
    public void JoinTeam(string team)
    {
        log.text = "LobbyManagerListener.JoinTeam: " + team;
    }
    public void Spectate()
    {
        log.text = "LobbyManagerListener.Spectate";
    }
    public void ErrorCallback(string msg)
    {
        log.text = "Error: " + msg;
    }
    //END LobbyManager messages

}
