using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class LeaderBoardUI : MonoBehaviour
{

    private LeaderBoardData data;
    public RowUi rowUi;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("LeaderboardData") != null) {
            data = GameObject.Find("LeaderboardData").GetComponent<LeaderBoardData>();
        }
        
        if (data && data.getLength() != 0) {
            List<float> array = LeaderBoardData.getData();
            List<string> names = LeaderBoardData.getNames();
            for (int i = 0; i < array.Count; i++) {

                int min = Mathf.FloorToInt(array[i]/60);
                int sec = Mathf.FloorToInt(array[i] - min * 60);
                string time = string.Format("{0:0}:{1:00}", min, sec);
                
                var row = Instantiate(rowUi, transform).GetComponent<RowUi>();
                row.name.text = names[i];
                row.timeScore.text = time;
            }
        }
    }
}
