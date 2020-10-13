using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
 
public class PlayerScript : MonoBehaviour{
    // References
    [Header("References")]
    public Transform trans;
    public Transform modelTrans;
    public CharacterController characterController;
    public Text speedText;

    // Movement Variables
    [Header("Movement")]
    [Tooltip("Units moved per second at maximum speed.")]
    public static float movespeed = 24;
    [Tooltip("Time, in seconds, to reach maximum speed.")]
    public float timeToMaxSpeed = .26f;
    private float VelocityGainPerSecond { get { return movespeed / timeToMaxSpeed; } }
    [Tooltip("Time, in seconds, to go from maximum speed to stationary.")]
    public float timeToLoseMaxSpeed = 1f;
    private float VelocityLossPerSecond { get { return movespeed / timeToLoseMaxSpeed; } }
    [Tooltip("Multiplier for momentum when attempting to move in a direction opposite the current traveling direction (e.g. trying to move right when already moving left).")]
    public float reverseMomentumMultiplier = 2.2f;
    private Vector3 movementVelocity = Vector3.zero;

    // Wheelie Feature Variables
    [Header("Wheelie Properties")]
    public static float wheelie = 0;
    [Tooltip("How fast that would take to get into a higher wheelie")] // Future Upgrades
    public float wheelieSkill = 10F;
    [Tooltip("Wheelie Turning Penalties, you can't turn when you are at a full wheelie")] // Future Upgrades
    public float wheeliePenalty = 1.8F;

    // Identify if the player got an accident or not.
    [Header("Death and Respawning")]
    [Tooltip("How long after the player's death, in seconds, before they are respawned?")]
    public float respawnWaitTime = 1f;
    private bool dead = false;

    // Booster Feature Variables
    [Header("Player Booster")]
    [Tooltip("Accumulated Booster Fuel")]
    public static float boost = 0f;
    public float boostPenalty = 0F;
    [Tooltip("How fast that would consume the booster")]
    public float boostConsumption = 20F;

    // Placement
    public static int place = 4;

    // Function when the player get into an accident
    public void Die(){
        if (!dead){
            dead = true;
            Invoke("Respawn", respawnWaitTime);
            modelTrans.rotation = Quaternion.Euler(90,0,0);
            movementVelocity = Vector3.zero;
            characterController.enabled = false;
        }
    }
    
    // Respawns after accident
    public void Respawn(){
        dead = false;
        if (trans.position.x > -2){
            trans.position = new Vector3(6, trans.position.y, trans.position.z);
        }
        else{
            trans.position = new Vector3(-14, trans.position.y, trans.position.z);
        }
        characterController.enabled = true;
    }

    // Functions for doing a Wheelie
    private void WheelieUp(){
        wheelie = Mathf.Min(30, wheelie + wheelieSkill * Time.deltaTime);
        modelTrans.rotation = Quaternion.Slerp(modelTrans.rotation, Quaternion.Euler(0,90,-wheelie*2), .2F);
    }

    // Functions for stopping wheelie
    private void WheelieDown(float mul = 0F){
        if (wheelie > 0){
            wheelie -= mul + wheelieSkill * Time.deltaTime;
            modelTrans.rotation = Quaternion.Slerp(modelTrans.rotation, Quaternion.Euler(0,90,-wheelie*2), .2F);
        }
        else{
            wheelie = 0F;
        }
    }

    // generating movement speed for the player
    public void moveForward(float speed){
        movespeed = speed + wheelie;
        movementVelocity.z = Mathf.Min(movespeed, movementVelocity.z + VelocityGainPerSecond * Time.deltaTime);
    }

    // Function for player controls
    private void Movement(){
        // faster way of Wheelie Down
        if (Input.GetKey(KeyCode.W)){
            WheelieDown(0.5F);
        }
        // Doing a wheelie
        else if (Input.GetKey(KeyCode.S)){
            WheelieUp();
        }
        // Slower way of wheelie down
        else {
            WheelieDown();
        }

        // Right Movement
        if (Input.GetKey(KeyCode.D)){
            if (movementVelocity.x >= 0) 
                movementVelocity.x = Mathf.Min(movespeed - (boostPenalty + wheelie * wheeliePenalty), movementVelocity.x + VelocityGainPerSecond * Time.deltaTime);

            else 
                movementVelocity.x = Mathf.Min(0, movementVelocity.x + VelocityGainPerSecond * reverseMomentumMultiplier * Time.deltaTime);
        }
        // Left Movement
        else if (Input.GetKey(KeyCode.A)){
            if (movementVelocity.x > 0)
                movementVelocity.x = Mathf.Max(0, movementVelocity.x - VelocityGainPerSecond * reverseMomentumMultiplier * Time.deltaTime);

            else 
                movementVelocity.x = Mathf.Max(-movespeed + (boostPenalty + wheelie * wheeliePenalty), movementVelocity.x - VelocityGainPerSecond * Time.deltaTime);
        }
        // Slowly removing side-to-side movements
        else{
            if (movementVelocity.x > 0) 
                movementVelocity.x = Mathf.Max(0, movementVelocity.x - VelocityLossPerSecond * Time.deltaTime);

            else
                movementVelocity.x = Mathf.Min(0, movementVelocity.x + VelocityLossPerSecond * Time.deltaTime);
        }
        // Booster Control: Increase maximum speed if booster fuel being used
        if(Input.GetKey(KeyCode.Space) && (boost > 0)){
                moveForward(34);
                boostPenalty = 10.0F;
                boost = Mathf.Max(0, boost - boostConsumption * Time.deltaTime);
        }
        // Maximum speeds turns back to normal if booster not being use
        else{
            if (movespeed > 24){
                movementVelocity.z = 24 + wheelie;
            }
            moveForward(24);
            boostPenalty = 0F;
        }
        
        // Update player's position if there's any movement
        if (movementVelocity.x != 0 || movementVelocity.z != 0){
            characterController.Move(movementVelocity * Time.deltaTime);
            modelTrans.rotation = Quaternion.Slerp(modelTrans.rotation, Quaternion.LookRotation(movementVelocity) * Quaternion.Euler(0,90,0), .2F);
        }
    }
    // Function when there's a viable collision with the player
    void OnControllerColliderHit(ControllerColliderHit hit){
        // If player reaches the finish line, this would redirect to the "finish scene"
        if (hit.gameObject.name == "Line"){
            SceneManager.LoadScene(2);
        }
        // Put the player into an accident when collides into a viable object
        else{
            Debug.Log("Player Collide!");
            movementVelocity.z = 0F;
            wheelie = 0F;
            Die();
        }
        
    }

    void Update(){
        // code runs if the player is not dead or in an accident
        if (!dead){
            Movement();
            speedText.text = movementVelocity.z.ToString("0");
            Debug.Log(place);
        }
    }
}