using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardData : MonoBehaviour
{

    private static List<string> names;
    private static List<float> times;
    public static LeaderBoardData Instance;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        times = new List<float>();
        names = new List<string>();
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update() {
        Instance = Instance;
    }
    public void addNewTime(float time) {
        times.Add(time);
    }

    public void addNewPlayer(string player) {
        names.Add(player);
    }

    public int getLength() {
        return times.Count;
    }

    public static List<float> getData() {
        return times;
    }
    public static List<string> getNames() {
        return names;
    }
}
