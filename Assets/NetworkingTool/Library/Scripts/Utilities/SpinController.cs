using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinController : MonoBehaviour {
    public enum Axis { X, Y, Z};
    public Axis rotationAngle;
    public float tilt = -20, smooth = 2f;
    Vector3 vx = new Vector3(1, 0, 0), 
            vy = new Vector3(0, 1, 0), 
            vz = new Vector3(0, 0, 1);
	void Update () {
        //Quaternion target = Quaternion.identity;
        //innerTilt = tilt;
        switch (rotationAngle)
        {
            case Axis.X:
                transform.Rotate(vx, tilt * Time.deltaTime * smooth);
                break;
            case Axis.Y:
                transform.Rotate(vy, tilt * Time.deltaTime * smooth);
                break;
            case Axis.Z:
                transform.Rotate(vz, tilt * Time.deltaTime * smooth);
                break;
        }
    }
}
