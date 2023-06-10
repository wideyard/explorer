using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using BayatGames.SaveGameFree;

public class FirstMenu : MonoBehaviour
{
    public GameObject detailsPanel;
    public Image Img;
    public Sprite[] pic;
    float timer;
    float isChange = 0;

    void Start()
    {
        int r = Random.Range(0,21);
        Img.sprite = pic[r];
    }


     // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if((int)timer>(int)isChange && (int)timer % 3 == 0)
        {
            int r = Random.Range(0,21);
            Img.sprite = pic[r];
            isChange = timer;
        }
    }
    public void New_Start()
    {
        for(int i=1;i<=4;i++){
            if (SaveGame.Exists("Lv" + i))
                SaveGame.Delete("Lv" + i);
                // PlayerPrefs.DeleteKey("Lv" + i);
            // if (PlayerPrefs.HasKey("Lv" + i))
            //     PlayerPrefs.DeleteKey("Lv" + i);
        }
        for(int i = 1; i<=4;i++)
        {
            for(int j = 1;j<=4;j++)
            {
                if (SaveGame.Exists("Lv" + i + "in" + j))
                    SaveGame.Delete("Lv" + i + "in" + j);
                // if (PlayerPrefs.HasKey("Lv" + i + "in" + j))
                //     PlayerPrefs.DeleteKey("Lv" + i + "in" + j);
            }
            
        }
        SceneManager.LoadScene("level_selection");
    }

    public void Start_with_history()
    {
        SceneManager.LoadScene("level_selection");
    }

    public void details()
    {
        detailsPanel.SetActive(true);
    }
}
