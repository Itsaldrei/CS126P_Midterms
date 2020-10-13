using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
// Scripts for the First AI
public class EnemyScript1 : MonoBehaviour{

    // Movement Variables
    public float movespeed = 24F; 
    public float timeToMaxSpeed = .26f;
    private float VelocityGainPerSecond { get { return movespeed / timeToMaxSpeed; } }
    public float reverseMomentumMultiplier = 2.2f;
    private Vector3 movementVelocity = Vector3.zero;

    // Wheelie Feature Variables
    public static float wheelie = 0F;
    public float wheelieSkill = 10F;
    public float wheeliePenalty = 1.8F;

    // Identify Obstacle's Location
    public static int index = 0;
    public float obstacleX, obstacleZ = 0;

    // Identify whether to go right or left... ROL == "Right or Left"
    public int rol = 0;

    // Identify if the character got an accident or not.
    public bool dead = false;

    // Identifies if the character is behind on the player
    public bool behind = false;

    // Functions for doing a wheelie
    private void WheelieUp(){
        wheelie = Mathf.Min(30, wheelie + wheelieSkill * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0,90,-wheelie*2), .2F);
    }
    
    // Functions for stopping wheelie
    private void WheelieDown(float mul = 0F){
        if (wheelie > 0){
            wheelie -= mul + wheelieSkill * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0,90,-wheelie*2), .2F);
        }
        else{
            wheelie = 0F;
        }
    }

    // Movement speed and position for the Character
    private void Movement(){
        if (transform.position.z - Camera.main.transform.position.z > 75){
            movementVelocity.z = 0;
            wheelie = 0f;
        }
        else{
            movespeed = 24 + wheelie;
            movementVelocity.z = Mathf.Min(movespeed, movementVelocity.z + VelocityGainPerSecond * Time.deltaTime);
        }

        // Position Movement of the Character
        if (movementVelocity.x != 0 || movementVelocity.z != 0){
            transform.position += movementVelocity * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementVelocity) * Quaternion.Euler(0,90,0), .2F);
        }
    }

    // Artificial Intelligence for the Character
    private void DecisionMaking(){
        // Updates the ROL.
        if (transform.position.x < 3){
            rol = 1;
        }
        else if (transform.position.x > 10){
            rol = 0;
        }

        // Tracks the Enemy
        try{
            Debug.Log("Enemy Detected!");
            obstacleX = SpawnObjects.cars[index].transform.position.x;
            obstacleZ = SpawnObjects.cars[index].transform.position.z;
        } catch {
            Debug.Log("No Enemy Detected!");
        }

        // Thinking Function for the Character
        if (transform.position.x < obstacleX+3 && transform.position.x > obstacleX-3){
            // Enemy at front
            WheelieDown(0.5F);
            if (rol == 0){
                movementVelocity.x = Mathf.Max(-movespeed + (wheelie * wheeliePenalty), movementVelocity.x - VelocityGainPerSecond * Time.deltaTime);
            }
            else {
                movementVelocity.x = Mathf.Min(movespeed - (wheelie * wheeliePenalty), movementVelocity.x + VelocityGainPerSecond * Time.deltaTime);
            }
        }
        else{
            // No Enemy at front
            if (!(transform.position.x < obstacleX+3 && transform.position.x > obstacleX-3)){
                if (rol == 0){
                    movementVelocity.x = Mathf.Max(0, movementVelocity.x - VelocityGainPerSecond * reverseMomentumMultiplier * Time.deltaTime);
                }
                else{
                    movementVelocity.x = Mathf.Min(0, movementVelocity.x + VelocityGainPerSecond * reverseMomentumMultiplier * Time.deltaTime);
                }
            }
            // Gain speed by WheelieUp if the path is clear
            if (movementVelocity.x == 0){
                WheelieUp();
            }
        }
        // Tracks new obstacles after passing the last obstacle
        if (transform.position.z > obstacleZ+4){
            index++;
        }
    }

    // Function that determines wheter the character has surppased the player or not
    public void placement(){
        if ((!behind) && Camera.main.transform.position.z+6 > transform.position.z){
            PlayerScript.place--;
            behind = true;
        }
        else if (behind && Camera.main.transform.position.z+6 < transform.position.z){
            PlayerScript.place++;
            behind = false;;
        }
    }

    public void OnTriggerEnter(Collider hit){
        // Collides to everything except for the player
        if (!(hit.gameObject.name == "Player")){
            Collide();
        }
    }

    // Accident Function
    public void Collide(){
        dead = true;
        Debug.Log("Enemy Collide!");
        wheelie = 0F;
        movementVelocity.z = 0F;
        Invoke("Respawn", 1f);
        transform.rotation = Quaternion.Euler(90,0,0);
    }

    // Respawn Function
    public void Respawn(){
        if (transform.position.x > -2){
            transform.position = new Vector3(6, transform.position.y, transform.position.z);
        }
        else{
            transform.position = new Vector3(-14, transform.position.y, transform.position.z);
        }
        dead = false;
    }

    void Update(){
        // Only works if it is not dead
        if (!dead){
            Movement();
            DecisionMaking();
            placement();
        }
    }
}