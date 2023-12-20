using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerName : MonoBehaviour
{
    public GameObject inputText;
    private string playerName;
    public GameObject error;
    private bool isValid = true;

    void Start() {
        gameObject.SetActive(false);
    }
    public void setName() {
        if (inputText.GetComponent<TMP_InputField>().text.Length < 3) {
            error.GetComponent<TextMeshProUGUI>().text = "Name must at least be 3 characters long";
            isValid = false;
            return;
        }
        isValid = true;
        playerName = inputText.GetComponent<TMP_InputField>().text;
        GameObject.Find("LeaderboardData").GetComponent<LeaderBoardData>().addNewPlayer(playerName);
        SceneManager.LoadScene("Lvl1 - AutoGeneration");
    }

    void Update() {
        if (!isValid) {
            error.SetActive(true);
        } else {
            error.SetActive(false);
        }
    }

}
