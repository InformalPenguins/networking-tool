using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/**
* Server Facade
* */
public class ServerGameLogic : MonoBehaviour
{
    private UDPServer udpServer;
    private GameObject NetworkButtons;
    public GameObject serverPrefab;
    private GameObject serverGameObject;
    private GameObject clientGameObject;
    private bool notifyStart = false;
    private UDPClient udpClient;

    void Start()
    {
        NetworkButtons = GameObject.Find("Canvas/NetworkButtons");
        clientGameObject = GameObject.Find("Client");
        udpClient = clientGameObject.GetComponent<UDPClient>();
    }
    public void StartAsServer()
    {
        Debug.Log("ServerGameLogic :: StartAsServer");
        serverGameObject = Instantiate(serverPrefab, transform);
        udpServer = serverGameObject.GetComponent<UDPServer>();
        udpServer.init();
        setupClient("127.0.0.1");
        notifyStart = true;
    }
    public void StartAsClient()
    {
        Text ServerIPInputText = GameObject.Find("Canvas/NetworkButtons/ServerIPInput/Text").GetComponent<Text>();
        string ServerIP = ServerIPInputText.text.Trim();
        //TODO: Create player prefs manager.
        PlayerPrefs.SetString("ServerIP", ServerIP);
        setupClient(ServerIP);
        Debug.Log("ServerGameLogic :: StartAsClient");
        Debug.Log("ServerGameLogic :: Destroying Server Game Logic");
        Destroy(gameObject);
    }
    private void setupClient(string serverIP)
    {
        udpClient.serverIP = serverIP;
        HideButtons();
        udpClient.init();
    }
    public void HideButtons()
    {
        NetworkButtons.SetActive(false);
    }
    void Update()
    {
        if (notifyStart && udpServer.getPlayersList().Keys.Count > 1)
        {
            notifyStart = false;
            int[] hv = BallController.CalculateForces();
            udpServer.broadCast(NetworkMessageHelper.BuildMessage(NetworkMessageHelper.ACTION_START, hv[0], hv[1]));
        }
    }

    public static ServerGameLogic _instance;

    public static ServerGameLogic Instance()
    {
        if (!Exists())
        {
            throw new Exception("MainThreadProcessor could not find the UnityMainThreadDispatcher object. Please ensure you have added the MainThreadExecutor Prefab to your scene.");
        }
        return _instance;
    }

    public static bool Exists()
    {
        return _instance != null;
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            //inputManager = GetComponent<InputManager>();
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
