using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnButtonClick : MonoBehaviour {
    public UDPClient udpClient;
    public InputField textBox;
    public void sendMessage (){
        string msg = textBox.text;
        udpClient.sendString (msg);
    }
}
