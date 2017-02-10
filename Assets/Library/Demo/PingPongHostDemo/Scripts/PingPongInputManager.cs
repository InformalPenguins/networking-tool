using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongInputManager : MonoBehaviour {
    UDPClient udpClient;
	// Use this for initialization
	void Start () {
        udpClient = GetComponent<UDPClient> ();
        #if UNITY_STANDALONE
            #if UNITY_EDITOR
        Debug.Log("PingPongInputManager.Start:UNITY_EDITOR");
            #endif
        //Windows|Mac OS
        #else
        //Assume mobile
        if (mobileInput) {
            //Activate mobile GameObjects
        }
        #endif
	}
	
	// Update is called once per frame
	void Update () {
        #if UNITY_STANDALONE
        //Windows|Mac OS
        if(Input.GetKeyDown(KeyCode.W)){
            sendCommand(PaddleController.PlayerCommand.UP);
        } else if(Input.GetKeyDown(KeyCode.S)){
            sendCommand(PaddleController.PlayerCommand.DOWN);
        }
        #else 
        //assume mobile
        #endif
	}
    private void sendCommand(PaddleController.PlayerCommand command){
        //Use a Player Interpreter if you require to do a preliminary render in your UI or wait until the server lets you know to do so.
        switch(command){
            case PaddleController.PlayerCommand.UP:
                udpClient.sendString ( PingPongMessageHelper.MovePlayerUpMessage());
                break;
            case PaddleController.PlayerCommand.DOWN:
                udpClient.sendString ( PingPongMessageHelper.MovePlayerDownMessage());
                break;
        }
    }
}
