using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadScript : MonoBehaviour
{
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(Mathf.Abs(Camera.main.transform.position.z) - Mathf.Abs(transform.position.z)) >= 300){
            Destroy(this.gameObject);
        }
    }
}
