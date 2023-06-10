using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using BayatGames.SaveGameFree;

public class NewBehaviourScript : MonoBehaviour
{
    public TextMeshProUGUI score;
    public int levelIndex;
    public int sum;


    void Start()
    {
        int tempsum = 0;
        for(int i = 1; i<=sum;i++)
        {
            if(SaveGame.Exists("Lv" + levelIndex + "in" + i))
            {
                if (SaveGame.Load<int>("Lv" + levelIndex + "in" + i) == 7)
                // if (PlayerPrefs.GetInt("Lv" + levelIndex + "in" + i) == 7)
                    tempsum += 1;
            }
        }
        SaveGame.Save<int>("Lv" + levelIndex, tempsum);
        // PlayerPrefs.SetInt("Lv" + levelIndex, tempsum);
        score.text = "completed:" + tempsum;
    }

    void Update()
    {
        int tempsum = 0;
        for(int i = 1; i<=sum;i++)
        {
            if(SaveGame.Exists("Lv" + levelIndex + "in" + i))
            {
                if (SaveGame.Load<int>("Lv" + levelIndex + "in" + i) == 7)
                // if (PlayerPrefs.GetInt("Lv" + levelIndex + "in" + i) == 7)
                    tempsum += 1;
            }
        }
        SaveGame.Save<int>("Lv" + levelIndex, tempsum);
        // PlayerPrefs.SetInt("Lv" + levelIndex, tempsum);
        score.text = "completed:" + tempsum;
    }
}