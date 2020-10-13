using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    // Reference Objects
    public Button playBtn, instBtn, quitBtn;

    void Start(){
        // Assigning to a function
        playBtn.onClick.AddListener(play);
        instBtn.onClick.AddListener(inst);
        quitBtn.onClick.AddListener(quit);
    }

    // Load the game scene
    public void play(){
        SceneManager.LoadScene(1);
    }

    // Load the instruction scene
    public void inst(){
        SceneManager.LoadScene(3);
    }

    // Quit the game
    public void quit(){
        Application.Quit();
    }
}
