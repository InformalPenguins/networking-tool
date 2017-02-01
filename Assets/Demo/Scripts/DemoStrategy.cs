using UnityEngine;
using UnityEngine.UI;

public class DemoStrategy : IUIHandler {
    public override void processMessage(string message){
        GameObject button = GameObject.Find ("Canvas/Button/Text");
        Text buttonText = button.GetComponent<Text> ();
        buttonText.text = message;
    }
}
