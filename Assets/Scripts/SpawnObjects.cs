using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour{

    // Reference for the objects to spawn
    public GameObject sameLaneObstacle;
    public GameObject otherLaneObstacle;
    public GameObject boosterItem;
    public GameObject highway;

    // Creates a list of Spawned cars to use as reference for the AI
    public static List<GameObject> cars = new List<GameObject>();

    // Reference to identify player's location
    public float checkPoint = -1400F;
    public float playerPosition;

    void Start()
    {
        playerPosition = Camera.main.transform.position.z;
        spawnRoads(0);

        // Code to auto spawn obstacles and boosters in a specific time
        StartCoroutine(carSpawns(36.0F));
    }

    private void spawnCars(){
        GameObject sameLane = Instantiate(sameLaneObstacle) as GameObject;
        sameLane.transform.position = new Vector3(Random.Range(0,13.5F),0,playerPosition+150);
        cars.Add(sameLane);
        GameObject otherLane = Instantiate(otherLaneObstacle) as GameObject;
        otherLane.transform.position = new Vector3(-(Random.Range(6F,21.5F)),0,playerPosition+150);
        otherLane.transform.rotation = Quaternion.Euler(0,180,0);
    }

    private void spawnBoosters(){
        GameObject booster = Instantiate(boosterItem) as GameObject;
        booster.transform.position = new Vector3(-(Random.Range(6F,21.5F)),0,playerPosition+150);
    }

    private void spawnRoads(float indent = 1){
        if (playerPosition > checkPoint){
            GameObject road = Instantiate(highway) as GameObject;
            if (indent != 1) indent = 0; else indent = 150;
            road.transform.position = new Vector3(2.383483F,0.5F,playerPosition+indent);
            checkPoint += 200F;
        }
    }

    // Function that lets you use "yield return new WaitForSeconds(time)"
    IEnumerator carSpawns(float respawnTime){
        while (true){
            yield return new WaitForSeconds(respawnTime/PlayerScript.movespeed);
            spawnCars();

            // chances that would generate a booster
            if (Random.Range(0,11) > 6){
                spawnBoosters();
            }
        }
    }

    void Update(){
        playerPosition = Camera.main.transform.position.z;
        spawnRoads();
    }
}
