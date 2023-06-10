using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using BayatGames.SaveGameFree;

public class SingleLevel : MonoBehaviour
{
    private int currentStarsNum = 0;
    public int levelIndex;

    // input pictures
    public int level;
    public int index;
    public int sum;

    public GameObject infoPanel;


    void Start()
    {
        ChangButtonColor();
    }

    void Update()
    {
        ChangButtonColor();
    }

    public void BackButton()
    {
        SceneManager.LoadScene("level_selection");
    }

    public void restart()
    {
        if (SaveGame.Exists ("Lv" + levelIndex))
            SaveGame.Delete("Lv" + levelIndex);
        for(int i = 1; i<=sum;i++)
        {
            if (SaveGame.Exists("Lv" + level + "in" + i))
                SaveGame.Delete("Lv" + level + "in" + i);
        }

        // if (PlayerPrefs.HasKey("Lv" + levelIndex))
        //     PlayerPrefs.DeleteKey("Lv" + levelIndex);
        // for(int i = 1; i<=sum;i++)
        // {
        //     if (PlayerPrefs.HasKey("Lv" + level + "in" + i))
        //         PlayerPrefs.DeleteKey("Lv" + level + "in" + i);
        // }
        
    }

    public void PressCountryButton(int _levelNum)
    {
        currentStarsNum = _levelNum;

        SaveGame.Save<int>("level", level);
        SaveGame.Save<int>("index", index);
        // PlayerPrefs.SetInt("level", level);
        // PlayerPrefs.SetInt("index", index);

        SceneManager.LoadScene("puzzle");

        //BackButton();
        //MARKER Each level has saved their own stars number
        //CORE PLayerPrefs.getInt("KEY", "VALUE"); We can use the KEY to find Our VALUE

        // Debug.Log(PlayerPrefs.GetInt("Lv" + levelIndex, _levelNum));
        // Debug.Log(SaveGame.Load<int>("Lv" + levelIndex));
    }

    public void ChangButtonColor()
    {
        string strtmp = "text" + index;
        if(transform.Find(strtmp))
        {
            TextMeshProUGUI obj = transform.Find(strtmp).GetComponent<TextMeshProUGUI>();
             // 保存一个 integer 数据类型:

            // if (PlayerPrefs.GetInt("Lv" + level + "in" + index) == 7)
            if (SaveGame.Exists("Lv" + level + "in" + index))
            {
                if(SaveGame.Load<int>("Lv" + level + "in" + index) == 7)
                    obj.color=new Color32(242, 122, 25, 255);
            }
            else
            {
                obj.color=new Color32(255, 255, 255, 255);
            }
        }
    }
}
