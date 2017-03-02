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
    private GameObject NetworkButtons, ServerUIText, ServerUI;
    public GameObject serverPrefab;
    private GameObject serverGameObject;
    private GameObject clientGameObject;
    private bool notifyStart = false, isServer = false;
    private UDPClient udpClient;

    private GameObject ball, paddle1, paddle2;
    void Start()
    {
        NetworkButtons = GameObject.Find("Canvas/NetworkButtons");
        ServerUI = GameObject.Find("Canvas/ServerUI");
        ServerUIText = GameObject.Find("Canvas/ServerUI/MessageText");
        ServerUI.SetActive(false);
        clientGameObject = GameObject.Find("Client");
        udpClient = clientGameObject.GetComponent<UDPClient>();
        ball = GameObject.Find("Actors/Ball");
        paddle1 = GameObject.Find("Actors/Paddle1");
        paddle2 = GameObject.Find("Actors/Paddle2");
    }
    public void StartAsServer()
    {
        Debug.Log("ServerGameLogic :: StartAsServer");
        serverGameObject = Instantiate(serverPrefab, transform);
        udpServer = serverGameObject.GetComponent<UDPServer>();
        udpServer.init();
        setupClient(NetworkMessageHelper.LOCALHOST);
        ServerUI.SetActive(true);
        isServer = notifyStart = true;
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
    float delayUpdate = .2f, nextUpdateSeconds = 0;
    private void broadCastBall() {
        Vector3 position = ball.transform.position;
        Vector3 velocity = ball.GetComponent<Rigidbody>().velocity;
        //Adjusts position and velocity of the ball
        udpServer.broadCast(PingPongMessageHelper.BuildMessage(PingPongMessageHelper.ACTION_UPDATE_POSITION, PingPongMessageHelper.TYPE_BALL, position.x, position.y, position.z, velocity.x, velocity.y, velocity.z));
    }
    private void broadCastPaddles() {

    }
    void Update()
    {
        if (!isServer) {
            return;
        }
        nextUpdateSeconds -= Time.deltaTime;
        if (nextUpdateSeconds <= 0)
        {
            nextUpdateSeconds = delayUpdate;
            broadCastBall();
            broadCastPaddles();
        }
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

    public void debugBroadCast() {
        string msg = ServerUIText.GetComponent<InputField>().text;
        udpServer.broadCast(msg);
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
    void OnApplicationQuit()
    {
        try
        {
            Debug.Log("OnApplicationQuit");
            if (udpServer != null)
            {
                udpServer.OnApplicationQuit();
            }
            if (udpClient != null)
            {
                udpClient.OnApplicationQuit();
            }
        }
        catch (Exception e)
        {
            Debug.Log("Errror closing server connection. \r" + e.StackTrace);
        }
    }
}
