using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
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
    private Player me;

    private GameObject teamAContainer, teamBContainer, spectateButton;
    private GameObject joinAButton, joinBButton, startButton;
    private Text spectateText, spectatorsListText;

    // UI Variables

    [Header("Not yet implemented")]
    public int maxPlayers = 2;
    public int maxSpectators = 3;
    public int minimumPlayers = 2;
    public bool allowSpectators = true;
    [Range(1, 2)]
    public int teamsCount;

    [Space(10)]
    [Header("Player prefabs")]
    public GameObject player1LobbyPrefab;
    public GameObject player2LobbyPrefab;

    [Header("Scene Handlers")]
    public GameObject lobbyMessageHandler;
    // END UI Variables

    //public Scene PlayScene;
    public bool autoStart = false;
    public int startTime = 5;

    void Start() {
        if (lobbyMessageHandler == null) {
            throw new MissingComponentException("LobbyMessageHandler not found. This object will receive all messages coming from the LobbyManager.");
        }
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
        lobbyMessageHandler.SendMessage(LobbyManagerListenerMethods.JOIN_TEAM, "A", SendMessageOptions.RequireReceiver);
    }
    public void JoinBClicked()
    {
        lobbyMessageHandler.SendMessage(LobbyManagerListenerMethods.JOIN_TEAM, "B", SendMessageOptions.RequireReceiver);
    }
    public void SpectatePressed()
    {
        lobbyMessageHandler.SendMessage(LobbyManagerListenerMethods.SPECTATE, SendMessageOptions.RequireReceiver);
    }

    private List<Player> playersList = new List<Player>();
    public void AddPlayer(int id, string name)
    {

        playersList.Add(new Player(id, name));
        renderSpectatorsListText();
    }
    public void JoinTeam(string teamName, int playerId)
    {
        Player found = findPlayer(playerId);
        if (found == null) {
            return;
        }
        Destroy(found.getGameObject());
        GameObject playerCard = null, localButton = null, counterButton = null;
        switch (teamName)
        {
            case "A":
                playerCard = Instantiate(player1LobbyPrefab, teamAContainer.transform, false);
                counterButton = joinBButton;
                localButton = joinAButton;
                break;
            case "B":
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
        renderSpectatorsListText();
    }
    public void Spectate(int playerId)
    {
        Player found = findPlayer(playerId);
        Destroy(found.getGameObject());
        found.setGameObject(null, null);
        renderSpectatorsListText();
    }
    public void AssignPlayer(int id)
    {
        me = findPlayer(id);
        if (me == null) {
            //Localize error messages.
            sendError("Wrong player id");
            return;
        }

        resetStates();
        spectateButton.SetActive (false);
        joinAButton.SetActive (true);
        joinBButton.SetActive (true);
    }
    private Player findPlayer(int id)
    {
        Player found = null;
        foreach (Player player in playersList)
        {
            if (player.getId() == id)
            {
                found = player;
                break;
            }
        }
        return found;
    }
    private Player findPlayer(string userName)
    {
        Player found = null;
        foreach (Player player in playersList)
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
        Player[] playersArray = playersList.ToArray();
        for (int i = 0; i < playersArray.Length; i++)
        {
            Player player = playersArray[i];
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

    public class Player{
        int id;
        string name;
        GameObject gameObject;
        string team;
        public Player(int id, string name) {
            this.id = id;
            this.name = name;
        }
        public GameObject getGameObject()
        {
            return gameObject;
        }
        public void setGameObject(string team, GameObject gameObject)
        {
            this.gameObject = gameObject;
            this.team = team;
        }
        public bool isSpectating() {
            //Not having a live UI element means the user is in spectate mode.
            return gameObject == null;
        }
        public int getId() { return id; }
        public string getName() { return name;  }
    }
    private void sendError(string msg) {
        lobbyMessageHandler.SendMessage(LobbyManagerListenerMethods.ERROR, msg, SendMessageOptions.RequireReceiver);
    }

    class LobbyManagerListenerMethods
    {
        public const string JOIN_TEAM = "JoinTeam";
        public const string SPECTATE = "Spectate";
        public const string ERROR = "ErrorCallback";
    }
}
