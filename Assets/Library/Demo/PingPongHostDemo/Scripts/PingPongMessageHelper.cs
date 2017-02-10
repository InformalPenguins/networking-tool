using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongMessageHelper : NetworkMessageHelper {
    //Paddle Commands
    public const int COMMAND_PADDLE_UP = 0;
    public const int COMMAND_PADDLE_DOWN = 1;

    public const int TYPE_BALL = 2;
    public const int TYPE_COLLISION = 3;

    public static string MovePlayerUpMessage(){
        return NetworkMessageHelper.BuildMessage(INPUT_POSITION, UDPClient.IDENTIFIER, COMMAND_PADDLE_UP);
    }
    public static string MovePlayerDownMessage(){
        return NetworkMessageHelper.BuildMessage(INPUT_POSITION, UDPClient.IDENTIFIER, COMMAND_PADDLE_DOWN);
    }
}
