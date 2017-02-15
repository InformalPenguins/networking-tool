using System.Collections.Generic;

public class NetworkMessageHelper
{
    public const char separator = ' ';
    public const string separatorStr = " ";
    public const string LOCALHOST = "127.0.0.1";
    //TYPE OF UPDATABLES
    public const int TYPE_PLAYER = 0;
    public const int TYPE_MINION = 1;

    //IN GAME ACTIONS POSITION/ROTATION
    public const int ACTION_UPDATE_POSITION = 0;
    public const int ACTION_START = 1;
    public const int ACTION_ATTACK = 2;
    public const int ACTION_SCORE = 3;
    

    // INPUT ACTIONS
    public const int INPUT_POSITION = 10;

    //NETWORK SERVER FUNCTIONS
    public const int ACTION_SERVER_LOGIN = 101;
    public const int ACTION_BROADCAST = 100;

    public static int asInt(string constant){
        return int.Parse (constant.Trim());
    }
    public static bool StartsWith(string message, int number){
        return message.StartsWith (number + " ");
    }
    /**
     * */
    public static string BuildMessage(params object[] arguments)
    {
        string message = "";
        for (int i = 0; i < arguments.Length; i++)
        {
            message += arguments[i] + NetworkMessageHelper.separatorStr;
        }
        return message;
    }
    public static string BuildMessage(params int[] arguments){
        string message = "";
        for ( int i = 0 ; i < arguments.Length ; i++ )
        {
            message += arguments [i] + NetworkMessageHelper.separatorStr;
        }
        return message;
    }
}