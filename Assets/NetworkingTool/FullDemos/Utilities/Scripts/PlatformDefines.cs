using UnityEngine;
using System.Collections;

public class PlatformDefines : MonoBehaviour {
    void Start () {
#if UNITY_EDITOR
        Debug.Log("Unity Editor");
#endif

#if UNITY_ANDROID || UNITY_IOS
        Debug.Log("Mobile");
#endif

#if UNITY_STANDALONE
        Debug.Log("Stand Alone");
#endif
    }
}
