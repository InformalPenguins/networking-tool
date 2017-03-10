using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongInputManager : MonoBehaviour {
    UDPClient udpClient;
	void Start () {
        udpClient = GetComponent<UDPClient> ();
        #if UNITY_STANDALONE
        //Windows|Mac OS
        #else
        //Assume mobile
        #endif
	}

    // PC Update
#if UNITY_STANDALONE
    void Update() {
        if (ClientGameLogic.state != ClientGameLogic.PingPongState.RUNNING) {
            //Ignore UP/DOWN input if NOT running.
            return;
        }

        //Windows|Mac OS
        if (Input.GetKeyDown(KeyCode.W)){
            sendCommand(PaddleController.PlayerCommand.UP);
        } else if(Input.GetKeyDown(KeyCode.S)){
            sendCommand(PaddleController.PlayerCommand.DOWN);
        }
    }
#else
    //Mobile variables
    bool direction = false;
    // Mobile Update
    void Update() {
        if (ClientGameLogic.state != PingPongState.RUNNING) {
            //Ignore UP/DOWN input if NOT running.
            return;
        }
        //assume mobile
        if (Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Ended)
        {
            direction = !direction;

            if (direction)
            {
                sendCommand(PaddleController.PlayerCommand.UP);
            }
            else
            {
                sendCommand(PaddleController.PlayerCommand.DOWN);
            }

            Debug.Log("Input.touches: " + Input.touches.Length);
            Debug.Log("Input.touchCount: " + Input.touchCount);
        }
    }
#endif
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
