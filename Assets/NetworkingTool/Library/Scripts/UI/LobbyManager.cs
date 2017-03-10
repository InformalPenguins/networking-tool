using Assets.NetworkingTool.Library.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//[ExecuteInEditMode]
//[System.Serializable]
public class LobbyManager : MonoBehaviour
{

    public static bool isOwner = false;

    //Constants
    public const string LOBBY_CONTAINER = "Canvas/Lobby";
    public const string TEAMS_CONTAINER = LOBBY_CONTAINER + "/Teams";
    public const string SPECTATE_SECTION = LOBBY_CONTAINER + "/SpectateSection";
    public const string SPECTATORS_LIST_TEXT = SPECTATE_SECTION + "/SpectatorsListText";
    public const string SPECTATORS_LABEL = SPECTATE_SECTION + "/SpectatorsText";

    public const string TEAM_A_CONTAINER = TEAMS_CONTAINER + "/TeamA/Viewport/Content";
    public const string TEAM_B_CONTAINER = TEAMS_CONTAINER + "/TeamB/Viewport/Content";

    public const string JOIN_A_BUTTON = TEAMS_CONTAINER + "/TeamA/Viewport/Content/GameObject/JoinButton";
    public const string JOIN_B_BUTTON = TEAMS_CONTAINER + "/TeamB/Viewport/Content/GameObject/JoinButton";
    public const string SPECTATE_BUTTON = SPECTATE_SECTION + "/SpectateButton";

    public const string START_BUTTON = LOBBY_CONTAINER + "/Actions/StartButton";

    public const string CONNECTED_USERNAME_TEXT = "Panel/Text";


    // Private variables
    private LobbyPlayer me;

    private GameObject teamAContainer, teamBContainer, spectateButton;
    private GameObject joinAButton, joinBButton, startButton;
    private Text spectateText, spectatorsListText;
    private List<LobbyPlayer> playersList = new List<LobbyPlayer>();

    // UI Variables

    public bool allowSpectators = true;
    //TODO: Implement
    //public int maxSpectators = 3; 
    public int maxPlayersByTeam = 2;
    public int minimumPlayersByTeam = 1;
    //TODO: Implement
    //[Range(1, 2)]
    // public int teamsCount;

    [Space(10)]
    [Header("Player prefabs")]
    public GameObject player1LobbyPrefab;
    public GameObject player2LobbyPrefab;

    [Header("Scene Handlers")]
    public GameObject lobbyMessageHandler;
    // END UI Variables

    //TODO: Implement
    //public Scene PlayScene;
    //public bool autoStart = false;
    //public int startTime = 5;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        if (lobbyMessageHandler == null) {
            throw new MissingComponentException("LobbyMessageHandler not found. This object will receive all messages coming from the LobbyManager.");
        }
        LobbyManager.isOwner = false; //wait until server assigns you as owner.
        initializeObjects();
        setSpectateControls();
        renderSpectatorsListText();
        resetStates();
    }
    private void resetStates()
    {
        //Initially hide all buttons until you add yourself.
        joinAButton.SetActive(false);
        joinBButton.SetActive(false);
        spectateButton.SetActive(false);
        startButton.SetActive(false);
        //        if (me != null)
        //        {
        //            //Attempt to destroy existing position in UI
        //            Destroy(me.getGameObject());
        //        }
        //spectatorsListText.text = "";
        renderSpectatorsListText();
    }
    private void setSpectateControls()
    {
        if (!allowSpectators)
        {
            spectateText.text = "PICKING: ";
        }
    }
    private void initializeObjects()
    {
        teamAContainer = GameObject.Find(TEAM_A_CONTAINER);
        teamBContainer = GameObject.Find(TEAM_B_CONTAINER);
        joinAButton = GameObject.Find(JOIN_A_BUTTON);
        joinBButton = GameObject.Find(JOIN_B_BUTTON);
        spectateButton = GameObject.Find(SPECTATE_BUTTON);
        spectateText = GameObject.Find(SPECTATORS_LABEL).GetComponent<Text>();
        spectatorsListText = GameObject.Find(SPECTATORS_LIST_TEXT).GetComponent<Text>();
        startButton = GameObject.Find(START_BUTTON);
    }

    public void SetLobbyMessageHandler(GameObject lobbyMessageHandler) {
        this.lobbyMessageHandler = lobbyMessageHandler;
    }
    public void JoinAClicked(){
        lobbyMessageHandler.SendMessage(LobbyManagerListenerMethods.JOIN_TEAM, LobbyManagerTeamNames.TEAM_A, SendMessageOptions.RequireReceiver);
    }
    public void JoinBClicked()
    {
        lobbyMessageHandler.SendMessage(LobbyManagerListenerMethods.JOIN_TEAM, LobbyManagerTeamNames.TEAM_B, SendMessageOptions.RequireReceiver);
    }
    public void SpectatePressed()
    {
        lobbyMessageHandler.SendMessage(LobbyManagerListenerMethods.SPECTATE, SendMessageOptions.RequireReceiver);
    }
    public void NotifyPlayerListChanged()
    {
        lobbyMessageHandler.SendMessage(LobbyManagerListenerMethods.GET_PLAYERS, playersList, SendMessageOptions.RequireReceiver);
    }
    public void StartPressed()
    {
        lobbyMessageHandler.SendMessage(LobbyManagerListenerMethods.START, SendMessageOptions.RequireReceiver);
    }
    public void QuitPressed()
    {
        lobbyMessageHandler.SendMessage(LobbyManagerListenerMethods.QUIT, SendMessageOptions.RequireReceiver);
    }

    private bool hasAllowedPlayerSpots()
    {
        return hasAllowedPlayerSpotsByTeam(LobbyManagerTeamNames.TEAM_A) && hasAllowedPlayerSpotsByTeam(LobbyManagerTeamNames.TEAM_B);
    }
    private int getPlayersCountByTeam(string team)
    {
        if (team == null || playersList.Count == 0) { return 0; }
        int c = 0;
        foreach (LobbyPlayer p in playersList)
        {
            if (team.Equals(p.getTeam()))
            {
                c++;
            }
        }
        return c;
    }
    private bool hasAllowedPlayerSpotsByTeam(string team)
    {
        if (team == null) { return false; }
        return getPlayersCountByTeam(team) < maxPlayersByTeam;
    }
    private bool hasMinPlayerSpotsTakenByTeam(string team)
    {
        if (team == null || playersList.Count == 0) { return false; }
        return getPlayersCountByTeam(team) >= minimumPlayersByTeam;
    }
    public void AddPlayer(int id, string name)
    {
        playersList.Add(new LobbyPlayer(id, name));
        renderSpectatorsListText();
        NotifyPlayerListChanged();
    }
    public void RemovePlayer(int id)
    {
        LobbyPlayer player = findPlayer(id);
        if (player == null)
        {
            //Localize error messages.
            sendError("Cannot remove player: Wrong player id {"+ id +"}");
            return;
        }
        if (player.getGameObject() != null) {
            Destroy(player.getGameObject());
        }

        player.setGameObject(null, null);

        playersList.Remove(player);
        if (player.Equals(me)) {
            me = null;
        }

        resetStates();
        checkMeStates();

        NotifyPlayerListChanged();
    }

    public void JoinTeam(string teamName, int playerId)
    {
        if (!hasAllowedPlayerSpotsByTeam(teamName))
        {
            return;
        }
        LobbyPlayer found = findPlayer(playerId);
        if (found == null) {
            return;
        }
        Destroy(found.getGameObject());
        GameObject playerCard = null, localButton = null, counterButton = null;
        switch (teamName)
        {
            case LobbyManagerTeamNames.TEAM_A:
                playerCard = Instantiate(player1LobbyPrefab, teamAContainer.transform, false);
                counterButton = joinBButton;
                localButton = joinAButton;
                break;
            case LobbyManagerTeamNames.TEAM_B:
                playerCard = Instantiate(player2LobbyPrefab, teamBContainer.transform, false);
                counterButton = joinAButton;
                localButton = joinBButton;
                break;
        }
        if (playerCard != null)
        {
            playerCard.transform.SetAsFirstSibling();
            playerCard.transform.Find(CONNECTED_USERNAME_TEXT).GetComponent<Text>().text = found.getName();
        }
        //TODO: Check if team is full
        if (me != null && found.getId() == me.getId()) {
            //You are switching teams!
            counterButton.SetActive(true);
            localButton.SetActive(false);
            if (allowSpectators)
            {
                spectateButton.SetActive(true);
            }
        }

        found.setGameObject(teamName, playerCard);
        //found.setTeam(teamName);
        renderSpectatorsListText();
        checkMeStates();
    }
    public void Spectate(int playerId)
    {
        LobbyPlayer found = findPlayer(playerId);
        if (found == null)
        {
            //Localize error messages.
            sendError("Wrong player id");
            return;
        }

        Destroy(found.getGameObject());
        found.setGameObject(null, null);
        renderSpectatorsListText();
        checkMeStates();
    }
    private void checkMeStates()
    {
        if (me == null)
        {
            return;
        }
        string team = me.getTeam();
        spectateButton.SetActive(team != null);
        joinAButton.SetActive(!(LobbyManagerTeamNames.TEAM_A.Equals(team)) && hasAllowedPlayerSpotsByTeam(LobbyManagerTeamNames.TEAM_A));
        joinBButton.SetActive(!(LobbyManagerTeamNames.TEAM_B.Equals(team)) && hasAllowedPlayerSpotsByTeam(LobbyManagerTeamNames.TEAM_B));
        if (LobbyManager.isOwner && hasMinPlayerSpotsTakenByTeam(LobbyManagerTeamNames.TEAM_A) && hasMinPlayerSpotsTakenByTeam(LobbyManagerTeamNames.TEAM_B)) {
            startButton.SetActive(true);
        }
    }
    public void AssignPlayer(int id)
    {
        me = findPlayer(id);
        if (me == null)
        {
            //Localize error messages.
            sendError("Wrong player id");
            return;
        }

        resetStates();
        checkMeStates();
    }
    
    private LobbyPlayer findPlayer(int id)
    {
        LobbyPlayer found = null;
        foreach (LobbyPlayer player in playersList)
        {
            if (player.getId() == id)
            {
                found = player;
                break;
            }
        }
        return found;
    }
    private LobbyPlayer findPlayer(string userName)
    {
        LobbyPlayer found = null;
        foreach (LobbyPlayer player in playersList)
        {
            if (userName.Equals(player.getName()))
            {
                found = player;
                break;
            }
        }
        return found;
    }
    public void renderSpectatorsListText()
    {
        string names = "";
        LobbyPlayer[] playersArray = playersList.ToArray();
        for (int i = 0; i < playersArray.Length; i++)
        {
            LobbyPlayer player = playersArray[i];
            if (!player.isSpectating()) {
                continue;
            }

            names += player.getName();

            if (i != playersArray.Length - 1)
            {
                names += ", ";
            }
        }

        spectatorsListText.text = names;
    }
    private void sendError(string msg) {
        lobbyMessageHandler.SendMessage(LobbyManagerListenerMethods.ERROR, msg, SendMessageOptions.RequireReceiver);
    }

    class LobbyManagerListenerMethods
    {
        public const string JOIN_TEAM = "JoinTeam";
        public const string SPECTATE = "Spectate";
        public const string ERROR = "ErrorCallback";
        public const string GET_PLAYERS = "GetPlayersList";
        public const string START = "LobbyStart";
        public const string QUIT = "LobbyQuit";
        
    }
    class LobbyManagerTeamNames
    {
        public const string TEAM_A = "A";
        public const string TEAM_B = "B";
    }
}
