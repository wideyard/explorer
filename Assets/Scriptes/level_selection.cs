using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using BayatGames.SaveGameFree;

public class level_selection : MonoBehaviour
{
    [SerializeField] private bool unlocked;//Default value is false;
    public Image unlockImage;

    // // Start is called before the first frame update
    // void Start()
    // {

    // }

    // Update is called once per frame
    private void Update()
    {
        UpdateLevelImage();
        UpdateLevelStatus();
    }

    private void UpdateLevelStatus()
    {
        int levelIndex = int.Parse(gameObject.name);
        if(SaveGame.Exists("Lv" + levelIndex))
        // if(PlayerPrefs.GetInt("Lv" + levelIndex) == 4)
        {
            if(SaveGame.Load<int>("Lv" + levelIndex) == 4)
                unlocked = true;
        }
    }

    private void UpdateLevelImage()
    {
        if(!unlocked)
        {
            unlockImage.gameObject.SetActive(true);
        }
        else
        {
            unlockImage.gameObject.SetActive(false);
        }
    }

    public void PressSelection(string _LevelName)//When we press this level, we can move to the corresponding Scene to play
    {
        SceneManager.LoadScene(_LevelName);
    }

    public void back_home()
    {
        SceneManager.LoadScene("first_menu");
    }
}
