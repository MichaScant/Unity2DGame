using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject TimerObject;
    private TextMeshProUGUI timerDisplay;
    static float timer;
    //public GameObject DestinationTrigger;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        timerDisplay = TimerObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("escape")) {
            UpdatePauseStatus();
        }

        if (!pauseMenu.active /*&& !DestinationTrigger.GetComponent<Destination>().isTriggered*/) {
            timer += Time.deltaTime;
            int min = Mathf.FloorToInt(timer/60);
            int sec = Mathf.FloorToInt(timer - min * 60);

            string time = string.Format("{0:0}:{1:00}", min, sec);
            timerDisplay.text = time;
        }
    }

    public void finish() {
        GameObject.Find("LeaderboardData").GetComponent<LeaderBoardData>().addNewTime(timer);
        timer = 0.0f;
        QuitToMainMenu();
    }

    public void UpdatePauseStatus() {
        pauseMenu.SetActive(!pauseMenu.active);
        
        if (pauseMenu.active) {
            Time.timeScale = 0f;
        } else {
            Time.timeScale = 1f;
        }
    }

    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
        timer = 0f;
    }

    public void QuitToMainMenu() {
        SceneManager.LoadScene("Title Screen");
        Time.timeScale = 1f;
    }
    
}
