using UnityEngine;
using UnityEngine.UI;
/**
 * Auto load the value from PlayerPrefs or use the default passed value instead.
 * This should be added to Text objects (children of buttons, input fields, etc)
 * */
public class PlayerPrefInputField : MonoBehaviour
{
    public string defaultValue;
    public string prefKey;
    private InputField inputField;
    void Start()
    {
        string value = PlayerPrefs.GetString(prefKey, defaultValue);
        inputField = GetComponent<InputField>();
        if (inputField == null)
        {
            throw new System.MissingFieldException("The GameObject does not contain an InputField element attached.");
        }
        inputField.text = value;
    }
}
