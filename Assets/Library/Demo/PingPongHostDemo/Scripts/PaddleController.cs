using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour {
    public int id { get; set; }
    Vector3 velocity = Vector3.zero;
    public enum PlayerCommand {
        UP, DOWN, STOP
    }

    Rigidbody rigidBody;
    public float paddleSpeed = 3f;

    void Start() {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void moveUp() {
        //rigidBody.velocity = Vector3.up * paddleSpeed;
        velocity = Vector3.up * paddleSpeed;
    }

    public void moveDown()
    {
        //rigidBody.velocity = Vector3.down * paddleSpeed;
        velocity = Vector3.down * paddleSpeed;
    }

    void Update() {
        transform.position = transform.position + velocity;
    }

    public void stop(){
        rigidBody.velocity = Vector3.zero;
    }
}
