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
    Thread receiveThread;
    UdpClient udpServer;

    public int port = 9000;
    public bool startListening = false;
    private String lastMessage = "";

    private Dictionary<string, IPEndPoint> clientsList = new Dictionary<string, IPEndPoint>();

    private static void Main()
    {
        UDPServer receiveObj = new UDPServer();
        receiveObj.init();

        string text = "";
        do
        {
            text = Console.ReadLine();
        }
        while (!text.Equals("exit"));
    }
    // start from unity3d
    public void Start()
    {
        if (startListening)
        {
            init();
        }
    }
    public void init()
    {
        print("Listening " + port);
        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
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


                if (!clientsList.ContainsKey(playerKey))
                {
                    clientsList.Add(playerKey, senderIpEndPoint);
                }

                // Converting data to string
                //TODO: Move to ClientHandler logic
                string text = Encoding.UTF8.GetString(data);

                if (text.StartsWith(NetworkConstants.ACTION_BROADCAST))
                {
                    broadCast("BROADCAST( " + playerKey + " ):" + text);
                }
                else if (text.StartsWith(NetworkConstants.ACTION_UPDATE_POSITION + " ") || text.StartsWith(NetworkConstants.INPUT_POSITION))
                {
                    broadCast(text, senderIpEndPoint);
                }
                else if (text.StartsWith(NetworkConstants.ACTION_SERVER_LOGIN + " "))
                {
                    sendMessage(getNextPlayerMessage(), senderIpEndPoint);
                }
                else
                {
                    sendMessage(playerKey + " - UNKNOWN MESSAGE :: " + text, senderIpEndPoint);
                }
                //End Client Handler

                // latest UDPpacket
                lastMessage = text;
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }
    private void broadCast(string msg, IPEndPoint ipEndPoint)
    {
        IPEndPoint[] players = UDPServer.toArray(this.clientsList);
        foreach (IPEndPoint player in players)
        {
            if (player != null && !player.Equals(ipEndPoint))
            {
                sendMessage(msg, player);
            }
        }
    }

    private void broadCast(string msg)
    {
        this.broadCast(msg, null);
    }

    private void sendMessage(string msg, IPEndPoint ipEndPoint)
    {
        byte[] msgBytes = Encoding.UTF8.GetBytes(msg);
        udpServer.Send(msgBytes, msgBytes.Length, ipEndPoint);
    }

    private int currentPlayer = 0;
    private string getNextPlayerMessage()
    {
        //TODO: Rework this assign of player
        String nextPlayerMessage = NetworkConstants.ACTION_SERVER_LOGIN + " ";
        if (currentPlayer != 0)
        {
            nextPlayerMessage += "0";
            currentPlayer = 0;
        }
        else
        {
            nextPlayerMessage += "1";
            currentPlayer = 1;
        }
        return nextPlayerMessage;
    }

    private static IPEndPoint[] toArray(Dictionary<string, IPEndPoint> dictionary)
    {
        IPEndPoint[] ipEndpointList = new IPEndPoint[dictionary.Count];
        dictionary.Values.CopyTo(ipEndpointList, 0);
        return ipEndpointList;
    }
}
