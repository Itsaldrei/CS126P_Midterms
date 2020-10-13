using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SameLaneCarScript : MonoBehaviour{
    // Reference
    private Rigidbody rb;  

    // Reference for movement variable
    public static float speed = 10.0f;

    public float original;

    void Start(){
        // Sets the Zvelocity of this Object with use of RigidBody
        rb = this.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0,0,speed);

        // Reference for the first original Yposition of this GameObject
        original = transform.position.y;
    }
    
    void Update(){
        // Checks if the player has passed this object
        if (Camera.main.transform.position.z > transform.position.z){
            // Destroys the object and updates the AI's obstacle tracker
            SpawnObjects.cars.Remove(this.gameObject);
            EnemyScript1.index--;
            EnemyScript2.index--;
            EnemyScript3.index--;
            Destroy(this.gameObject);
        }
        // Destroys this object if it's out of the player's range
        else if (Mathf.Abs(Camera.main.transform.position.z - transform.position.z) >= 150){
            Destroy(this.gameObject);
        }
        // Destroys this object if accident (*cough* bug) happens by referencing the original Yposition of the object
        else if (original != transform.position.y){
            Destroy(this.gameObject);
        }
    }
}
