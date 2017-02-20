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

    public Scene PlayScene;

    private bool autoStart = false;
    private int startTime = 5;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
