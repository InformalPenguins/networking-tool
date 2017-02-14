using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPClient : MonoBehaviour
{
    //TODO: Feed server from config file to allow dynamic server|port
    public string serverIP = "127.0.0.1";
    public int serverPort = 9000;
    public int clientListeningPort = 9001;
    public static int IDENTIFIER = -1;

    IPEndPoint remoteEndPoint;
    UdpClient server;
    INetworkHandler serverHandler;
    Thread receiveThread;
    int countErrors = 0;
    public bool startListening = true;
    public void Start()
    {
        Application.runInBackground = true;
        serverHandler = GetComponent<INetworkHandler>();
        if (startListening)
        {
            init();
        }
    }
    
    public void init()
    {
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
        while (true) { 
            try
            {
                server = new UdpClient(clientListeningPort);
                break;
            }
            catch {
                clientListeningPort++;
                continue;
            }
        }
        server.EnableBroadcast = true;

        print("Sending to " + serverIP + " : " + serverPort);

        receiveThread = new Thread(new ThreadStart(serverListener));
        receiveThread.IsBackground = true;
        receiveThread.Start();

        ////Ask for identification
        requestIdentifier();
    }

    private bool checkIdentifier = true;
    private int checkIdentifierDelay = 2;
    private Coroutine requestIdentifierCoroutine;
    public IEnumerator requestIdentifierDelayed()
    {
        while (checkIdentifier)
        {
            yield return new WaitForSeconds(checkIdentifierDelay);

            if (UDPClient.IDENTIFIER >= 0)
            {
                checkIdentifier = false;
                StopCoroutine(requestIdentifierCoroutine);
            }
            else {
                sendString(NetworkMessageHelper.BuildMessage(NetworkMessageHelper.ACTION_SERVER_LOGIN));
            }
        }
    }
    private void requestIdentifier()
    {
        requestIdentifierCoroutine = StartCoroutine(requestIdentifierDelayed());
    }

    /**
     * TODO: Handle Reconnect
     * 
     * Receives the message from the server
     * */
    private bool isClientListening = false;
    private void serverListener()
    {
        while (isClientListening)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = server.Receive(ref anyIP);
                string text = Encoding.UTF8.GetString(data);
                serverHandler.processMessage(text);
            }
            catch (ObjectDisposedException err)
            {
                Debug.LogError("Object disposed");
                Debug.LogError(err.StackTrace);
                break;
            }
            catch (SocketException err)
            {
                Debug.LogError("Connection lost");
                Debug.LogError(err.StackTrace);
                //break;
            }
            catch (Exception err)
            {
                Debug.LogError(err.ToString());
                Debug.LogError(err.StackTrace);
                if (countErrors++ > 10)
                {
                    Debug.LogError("Too many errors");
                    Application.Quit();
                    break;
                }
            }
        }
    }

    public void sendString(string message)
    {
        try
        {
            if (message != "")
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                server.Send(data, data.Length, remoteEndPoint);
            }
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
    }


    private void OnApplicationQuit()
    {
        try
        {
            isClientListening = false;
            if (server != null)
            {
                server.Close();
            }
            if (receiveThread != null)
            {
                receiveThread.Abort();
            }
        }
        catch (Exception e)
        {
            Debug.Log("Errror closing server connection. \r" + e.StackTrace);
        }
    }


}