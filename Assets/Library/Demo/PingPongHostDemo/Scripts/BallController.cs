using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {
    Rigidbody rigidBody;
    Vector3 origin;
    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody> ();
        origin = transform.position;
        AddForce ();
    }

    void AddForce(){
        bool right = Random.Range (0, 1) == 0;
        bool up = Random.Range (0, 1) == 0;
        float verticalForce = Random.Range (5, 7) * (right ? 1:-1);
        float horizontalForce = Random.Range (6, 12) * (up ? 1:-1);
        #if UNITY_EDITOR
        Debug.Log("BallController.AddForce");
        Debug.Log("horizontalForce: " + horizontalForce);
        Debug.Log("verticalForce: " + verticalForce);
        #endif

        rigidBody.AddForce(horizontalForce, verticalForce, 0, ForceMode.Impulse);
    }
    void Update(){
    }
    public void Reset(){
        rigidBody.velocity = Vector3.zero;
        transform.position = origin;
        //AddForce ();
    }
}
