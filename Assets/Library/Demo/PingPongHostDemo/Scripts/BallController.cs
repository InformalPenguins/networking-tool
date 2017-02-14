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
        //AddForce ();
    }

    void AddForce(int horizontalForce, int verticalForce)
    {
        #if UNITY_EDITOR
        Debug.Log("BallController.AddForce");
        Debug.Log("horizontalForce: " + horizontalForce);
        Debug.Log("verticalForce: " + verticalForce);
        #endif

        rigidBody.AddForce(horizontalForce, verticalForce, 0, ForceMode.Impulse);
    }
    float delayLog = 10, actualDelay = 0;
    void Update(){
        actualDelay -= Time.deltaTime;
        if (actualDelay <= 0) {
            actualDelay = delayLog;
            Debug.Log("BALL: Velocity: " + rigidBody.velocity);
        }
    }
    public static int[] CalculateForces()
    {
        bool right = Random.Range(0, 1) == 0;
        bool up = Random.Range(0, 1) == 0;
        int verticalForce = Random.Range(5, 7) * (right ? 1 : -1);
        int horizontalForce = Random.Range(6, 12) * (up ? 1 : -1);
        return new int[] { horizontalForce, verticalForce };
    }
    public void Reset(int horizontalForce, int verticalForce)
    {
        rigidBody.velocity = Vector3.zero;
        transform.position = origin;
        AddForce (horizontalForce, verticalForce);
    }
}
