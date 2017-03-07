using Assets.NetworkingTool.Library.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugLobbyManager : MonoBehaviour {
    public Text log;
    public InputField userName;
    public LobbyManager lobbyManager;
    public GameObject playersContainer;
    public GameObject lobbyPlayer;

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
        //Hack to set as owner while in debug mode.
        LobbyManager.isOwner = true;
    }
    public void SendRemove()
    {
        string newUserName = userName.text;
        int id = int.Parse(newUserName);
        lobbyManager.RemovePlayer(id);
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
    public void GetPlayersList(List<LobbyPlayer> playersList)
    {
        Transform parent = playersContainer.transform;
        while (parent.GetChildCount() > 0)
        {
            Destroy(parent.GetChild(0));
        }
        foreach (LobbyPlayer player in playersList)
        {
            GameObject newGO = Instantiate(lobbyPlayer, parent, false); //new GameObject("myTextGO");
            //newGO.transform.SetParent(parent);
            Text myText = newGO.GetComponent<Text>();
            myText.text = player.getId() + " - " + player.getName() + " - " + player.getTeam();
        }
    }
    public void LobbyStart()
    {
        log.text = "LobbyStart";
    }
    public void LobbyQuit()
    {
        log.text = "LobbyQuit";
    }
    //END LobbyManager messages

}
