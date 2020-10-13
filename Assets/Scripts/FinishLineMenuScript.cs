using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishLineMenuScript : MonoBehaviour
{
    // Reference Objects
    public Button playAgainBtn, quitBtn;
    public Text placingText;

    // Assigning to a function
    void Start(){
        playAgainBtn.onClick.AddListener(playAgain);
        quitBtn.onClick.AddListener(quit);
    }

    // Load the game scene
    public void playAgain(){
        SceneManager.LoadScene(1);
    }

    // Quit the game
    public void quit(){
        Application.Quit();
    }

    // Display the correct placement when finishing the race
    public void placement(){
        if (PlayerScript.place == 1){
            placingText.text = PlayerScript.place.ToString() + "ST PLACE";
        }
        else if (PlayerScript.place == 2){
            placingText.text = PlayerScript.place.ToString() + "ND PLACE";
        }
        else if (PlayerScript.place == 3){
            placingText.text = PlayerScript.place.ToString() + "RD PLACE";
        }
        else if (PlayerScript.place == 4){
            placingText.text = PlayerScript.place.ToString() + "TH PLACE";
        }
    }
    void Update(){
        placement();
    }
}
