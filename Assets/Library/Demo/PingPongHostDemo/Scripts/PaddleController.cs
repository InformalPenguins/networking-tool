using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour {
    public enum PlayerCommand {
        UP, DOWN, STOP
    }

    Rigidbody rigidBody;
    private static float speed = 1f;

    void Start(){
        rigidBody = GetComponent<Rigidbody> ();
    }

    public void moveUp(){
        rigidBody.velocity = Vector3.up * speed;
    }

    public void moveDown(){
        rigidBody.velocity = Vector3.down * speed;
    }

    public void stop(){
        rigidBody.velocity = Vector3.zero;
    }
}
