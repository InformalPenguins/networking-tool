using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour {
    public const string TEAMS_CONTAINER = "Canvas/Lobby/Teams";
    public const string TEAM_A_CONTAINER = TEAMS_CONTAINER + "/TeamA/Viewport/Content";
    public const string TEAM_B_CONTAINER = TEAMS_CONTAINER + "/TeamB/Viewport/Content";

    [Header("Not yet implemented")]
    public int maxPlayers = 2;
    public int maxSpectators = 3;
    public int minimumPlayers = 2;
    [Range(1, 2)]
    public int teamsCount;

    [Space(10)]
    [Header("Player prefabs")]
    public GameObject player1LobbyPrefab;
    public GameObject player2LobbyPrefab;


    [Header("Scene Handlers")]
    public GameObject lobbyMessageHandler;
    //public Scene PlayScene;

//    private bool autoStart = false;
//    private int startTime = 5;

    private GameObject teamAContainer, teamBContainer;
    // Use this for initialization
    void Start () {
        teamAContainer = GameObject.Find(TEAM_A_CONTAINER);
        teamBContainer = GameObject.Find(TEAM_B_CONTAINER);
        if(lobbyMessageHandler == null){
            throw new MissingComponentException ("LobbyMessageHandler not found. This object will receive all messages coming from the LobbyManager.");
        }
    }
    public void JoinAClicked(){
        GameObject player = Instantiate (player1LobbyPrefab, teamAContainer.transform, false);
        JoinTeamClicked (player, "A");
    }
    public void JoinBClicked(){
        GameObject player = Instantiate (player2LobbyPrefab, teamBContainer.transform, false);
        JoinTeamClicked (player, "B");
    }
    private void JoinTeamClicked(GameObject player, string team){
        player.transform.SetAsFirstSibling();
        lobbyMessageHandler.SendMessage (LobbyManagerListenerMethods.JOIN_TEAM, team, SendMessageOptions.RequireReceiver);
    }

    class LobbyManagerListenerMethods{
        public const string JOIN_TEAM = "JoinTeam";
    }
}
