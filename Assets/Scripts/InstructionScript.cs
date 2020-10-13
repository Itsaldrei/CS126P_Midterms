using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InstructionScript : MonoBehaviour
{
    // Reference Objects
    public Button backBtn;

    void Start(){
        // Assigning to a function
        backBtn.onClick.AddListener(back);
    }

    // Load the Main Menu scene
    public void back(){
        SceneManager.LoadScene(0);
    }
}
