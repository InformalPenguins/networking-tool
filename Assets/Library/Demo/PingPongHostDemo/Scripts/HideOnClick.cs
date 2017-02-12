using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnClick : MonoBehaviour {
    GameObject NetworkButtons;
    void Start() {
        NetworkButtons = GameObject.Find("Canvas/NetworkButtons");
    }
    public void Hide() {
        NetworkButtons.SetActive(false);
    }
}
