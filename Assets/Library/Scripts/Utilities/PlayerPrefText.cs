using UnityEngine;
using UnityEngine.UI;
/**
 * Auto load the value from PlayerPrefs or use the default passed value instead.
 * This should be added to Text objects (children of buttons, input fields, etc)
 * */
public class PlayerPrefText : MonoBehaviour {
    public string defaultValue;
    public string prefKey;
    private Text textGameObject;
	void Start () {
        string value = PlayerPrefs.GetString(prefKey, defaultValue);
        textGameObject = GetComponent<Text>();
        if (textGameObject == null) {
            throw new System.MissingFieldException("The GameObject does not contain a Text element attached.");
        }
        textGameObject.text = value;
    }
}
