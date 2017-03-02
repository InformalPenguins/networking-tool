using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour {
    public int id { get; set; }
    private Vector3 velocity = Vector3.zero;
    private float limitBorder = 8.3f;

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
        Vector3 next = transform.position + velocity;
        if (next.y > -limitBorder && next.y < limitBorder)
        {
            transform.position = next;
        }
    }

    public void stop(){
        rigidBody.velocity = Vector3.zero;
    }
}
