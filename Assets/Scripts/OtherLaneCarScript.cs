using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherLaneCarScript : MonoBehaviour{
    // Reference
    private Rigidbody rb;

    // Reference for movement variable
    public float speed = 24.0f;

    public float original;

    void Start(){
        // Sets the Zvelocity of this Object with use of RigidBody
        rb = this.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0,0,-speed);

        // Reference for the first original Yposition of this GameObject
        original = transform.position.y;
    }

    void Update(){
        // Destroys the object if it has already passed the player's sight
        if (Camera.main.transform.position.z > transform.position.z){
            Destroy(this.gameObject);
        }

        // Destroys this object if accident (*cough* bug) happens by referencing the original Yposition of the object
        else if (original != transform.position.y){
            Destroy(this.gameObject);
        }
    }
}
