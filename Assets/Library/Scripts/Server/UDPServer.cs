using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;


public class UDPServer : MonoBehaviour
{
    public int port = 9000; // Server listening port
    public bool startListening = false; // Should auto start or wait to run HOST mode.

    private Thread receiveThread; // The thread to process multiple users.
    private UdpClient udpServer; // The actual UDP connection listener
    private IServerStrategy serverStrategy; // The Server Strategy to read messages
    private int countErrors = 0; // Used for killing APP purposes
    private int playerCounter = 0;

    //TODO: stablish a ping behavior to kick players.
    private Dictionary<string, Player> playersList = new Dictionary<string, Player>();

    public void Start()
    {
        Debug.Log("UDPServer: Starting");
        if (startListening)
        {
            init();
        }
    }

    public void init()
    {
        print("UDPServer: Listening " + port);
        serverStrategy = GetComponent<IServerStrategy>();
        serverStrategy.setUdpServer(this);
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }
    private void ReceiveData()
    {
        //Server loop
        udpServer = new UdpClient(port);
        udpServer.EnableBroadcast = true;
        while (true)
        {
            try
            {
                // Client msg arrived.
                IPEndPoint senderIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                byte[] data = udpServer.Receive(ref senderIpEndPoint);

                int senderPort = senderIpEndPoint.Port;
                string senderIp = senderIpEndPoint.Address.ToString();
                string playerKey = senderIp + ":" + senderPort;
                Player player = null;

                if (!playersList.TryGetValue(playerKey, out player))
                {
                    //New user connected.
                    #if UNITY_EDITOR
                    Debug.Log("UDPServer :: Adding player with key: " + playerKey);
                    #endif
                    player = new Player(senderIpEndPoint);
                    player.id = playerCounter ++;
                    playersList.Add(playerKey, player);
                }

                // Converting data to string
                string text = Encoding.UTF8.GetString(data);

                #if UNITY_EDITOR
                Debug.Log("UDPServer :: Received message from: " + playerKey + ": " + text);
                #endif

                if(player.id < 0){
                    //Ignore user until has an id.
                    continue;
                }

                serverStrategy.processText(text, player);
            }
            catch (ObjectDisposedException err)
            {
                Debug.LogError("UDPServer :: Object disposed. Exiting.");
                Debug.LogError(err.StackTrace);
                break;
            }
            catch (SocketException err)
            {
                Debug.LogError("UDPServer :: Connection error, retrying...");
                Debug.LogError(err.StackTrace);
                //break;
            }
            catch (Exception err)
            {
                Debug.LogError("UDPServer :: Exception");
                Debug.LogError(err.ToString());
                Debug.LogError(err.StackTrace);
                if (countErrors++ > 10)
                {
                    Debug.LogError("UDPServer :: Too many errors");
                    Application.Quit();
                    break;
                }
            }
        }
    }

    /**
     * Sends a message to all players, except one, usually the sender.
     * @param msg: The message to send
     * */
    public void broadCast(string msg, Player excludedPlayer)
    {
        if(msg == null){
            return;
        }

        Player[] players = UDPServer.toArray(this.playersList);

        if(players.Length == 0){
            return;
        }

        IPEndPoint excludedIPEndPoint = null;

        if(excludedPlayer != null){
            excludedIPEndPoint = excludedPlayer.ipEndPoint;
        }

        foreach (Player player in players)
        {
            if (player != null && player.ipEndPoint != null && !player.ipEndPoint.Equals(excludedIPEndPoint))
            {
                sendMessage(msg, player);
            }
        }
    }

    /**
     * Sends a message to all players.
     * @param msg: The message to send
     * */
    public void broadCast(string msg)
    {
        this.broadCast(msg, null);
    }

    /**
     * Sends a message to a player.
     * @param msg: The message to send
     * @param player: The player to receive the message bind by its IPEndPoint value.
     * */
    public void sendString(string msg, Player player)
    {
        this.sendMessage (msg, player);
    }

    /**
     * Sends a message to a player. Do NOT get confused by GameObject.SendMessage which tries to execute methods in the children.
     * @param msg: The message to send
     * @param player: The player to receive the message bind by its IPEndPoint value.
     * */
    public void sendMessage(string msg, Player player)
    {
        IPEndPoint ipEndPoint = player.ipEndPoint;
        byte[] msgBytes = Encoding.UTF8.GetBytes(msg);
        udpServer.Send(msgBytes, msgBytes.Length, ipEndPoint);
    }


    private static Player[] toArray(Dictionary<string, Player> dictionary)
    {
        Player[] ipEndpointList = new Player[dictionary.Count];
        dictionary.Values.CopyTo(ipEndpointList, 0);
        return ipEndpointList;
    }
    public Dictionary<string, Player> getPlayersList(){
        return this.playersList;
    }
    public Player getPlayer(string key){
        Player player;
        this.playersList.TryGetValue(key, out player);
        return player;
    }
    public Player FindPlayer(string username){
        Player[] players = toArray (playersList);
        foreach(Player player in players){
            if(username.Equals(player.username)){
                return player;
            }
        }
        return null;
    }

    private void OnApplicationQuit()
    {
        try
        {
            udpServer.Close();
            receiveThread.Abort();
        }
        catch (Exception e)
        {
            Debug.Log("Errror closing server connection. \r" + e.StackTrace);
        }
    }
}
