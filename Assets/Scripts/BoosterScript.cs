using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterScript : MonoBehaviour
{
    // Detects if the object was hit by the player
    void OnTriggerEnter(Collider hit){
        // Player gains addition boost fuel and destroys this object when collision happens
        PlayerScript.boost += 25;
        // Limits the Boost Fuel up to 100
        if (PlayerScript.boost > 100){
            PlayerScript.boost = 100;
        }
        Destroy(this.gameObject);
    }

    void Update()
    {
        // Destroys when players passed by it
        if (Camera.main.transform.position.z > transform.position.z){
            Destroy(this.gameObject);
        }
    }
}
