using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleScreenManager : MonoBehaviour
{
    public GameObject mainMenuContent;
    public GameObject Leaderboard;
    public GameObject nameEntry;
    
    void Start() {
        Leaderboard.SetActive(false);
    }

    public void LoadChosenLevel() {
        nameEntry.SetActive(true);
        mainMenuContent.SetActive(false);
    }

    public void LoadLeaderBoard() {
        mainMenuContent.SetActive(false);
        Leaderboard.SetActive(true);
    }
    void Update() {
        if(Input.GetKeyDown("escape")) {
            mainMenuContent.SetActive(true);
            Leaderboard.SetActive(false);
        }
    }

    public void ExitGame() {
        Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
