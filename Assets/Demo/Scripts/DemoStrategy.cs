using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoStrategy : INetworkHandler{
    void INetworkHandler.processMessage(string message){
        GameObject button = GameObject.Find ("Canvas/Button/Text");
        Text buttonText = button.GetComponent<Text> ();
        buttonText.text = message;
    }

//    //TODO: Delegate to a GameLogic entity
//    if (text.StartsWith(NetworkConstants.ACTION_UPDATE_POSITION + " "))
//    {
//        text = text.Substring(NetworkConstants.ACTION_UPDATE_POSITION.ToString().Length + 1);
//        this.LoadPositions(text);
//    }
//    else if (text.StartsWith(NetworkConstants.INPUT_POSITION))
//    {
//        text = text.Substring(NetworkConstants.INPUT_POSITION.Length);
//        this.MoveToLocation(text);
//    }
//    else if (text.StartsWith(NetworkConstants.ACTION_SERVER_LOGIN))
//    {
//        text = text.Substring(NetworkConstants.ACTION_SERVER_LOGIN.ToString().Length);
//        this.IdentifyUser(text);
//    }
//
//    private void MoveToLocation(string text)
//    {
//        throw new NotImplementedException();
//    }
//    private void LoadPositions(string text)
//    {
//        throw new NotImplementedException();
//    }
//    private void IdentifyUser(string text)
//    {
//        throw new NotImplementedException();
//    }
}
