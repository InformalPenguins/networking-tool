using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour {
    public int maxPlayers = 2;
    public int maxSpectators = 3;
    public int minimumPlayers = 2;
    [Range(1, 2)]
    public int teamsCount;

    public GameObject player1LobbyPrefab;
    public GameObject player2LobbyPrefab;

    private const string TEAM_A_CONTAINER = "Canvas/Lobby/Teams/TeamA/Viewport/Content";
    private const string TEAM_B_CONTAINER = "Canvas/Lobby/Teams/TeamB/Viewport/Content";
    public Scene PlayScene;

    private bool autoStart = false;
    private int startTime = 5;

    private GameObject teamAGameObject, teamBGameObject;
    // Use this for initialization
    void Start ()
    {
        teamAGameObject = GameObject.Find(TEAM_A_CONTAINER);
        teamBGameObject = GameObject.Find(TEAM_B_CONTAINER);
    }
    public enum Team { A, B};
    public void JoinClicked(Team team) {
        switch (team) {
            case Team.A:
                GameObject newPlayerA = Instantiate(player1LobbyPrefab, teamAGameObject.transform, false);
                newPlayerA.transform.SetAsFirstSibling();
                //newPlayerA.transform.localScale = new Vector3(1, 1, 1);
                break;
            case Team.B:
                GameObject newPlayerB = Instantiate(player2LobbyPrefab,teamBGameObject.transform, false);
                newPlayerB.transform.SetAsLastSibling();
                //newPlayerB.transform.localScale = new Vector3(1, 1, 1);
                break;
        }
    }
    public void JoinAClicked()
    {
        JoinClicked(Team.A);
    }
    public void JoinBClicked()
    {
        JoinClicked(Team.B);
    }

        // Update is called once per frame
        void Update () {
		
	}
}
