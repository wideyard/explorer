using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Windows.Forms;
using UnityEngine.SceneManagement;

using BayatGames.SaveGameFree;

public class puzzle_pictures : MonoBehaviour
{
    //单例模式
    public static puzzle_pictures Instance;

    // input pictures
    public int levelIndex;
    public int level;
    public int index;

    public GameObject StartPanel;
    public GameObject EndPanel;

    bool gameStart;
    float timer;
    public Drawer drawer;

    //新的图片分辨率
    [HideInInspector]
    float newW, newH;

    //碎片间隔
    public float intervalSize = 170;

    //图片
    [SerializeField]
    RawImage rawImage;

    //规定大小
    [SerializeField]
    Vector2 basicScale; 

    //计时器
    [SerializeField]
    public TextMeshProUGUI timeText;
    [SerializeField]
    public TextMeshProUGUI EndTimeText;

    // 图片集
    [SerializeField]
    public Texture2D[] pics;

    int[] puzzle; //碎片格子

    private void Awake()
    {
        Instance = this;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (gameStart)
        {
            timer += Time.deltaTime;
            timeText.text = ((int)timer).ToString();
        }

    }

    //读取图片
    [UnityEngine.ContextMenu("LoadPicture")]
    public void LoadPicture()
    {
        // level = PlayerPrefs.GetInt("level");
        // index = PlayerPrefs.GetInt("index");

        level = SaveGame.Load<int>("level");
        index = SaveGame.Load<int>("index");

        int tmpnum = (level-1)*4 + index-1;
        Debug.Log(tmpnum);
        Texture2D tex2d = pics[tmpnum];
        rawImage.texture = tex2d;

        PictureScaleResize(tex2d);
        rawImage.color = new Color(1, 1, 1, 0.5f);

        newW = (int)(newW / intervalSize);
        newH = (int)(newH / intervalSize);
        if (newW % 2 != 0)
        {
            newW += 1;
        }
        if( newH % 2 != 0 )
        {
            newH += 1;
        }
        //自动生成碎片
        Puzzle_Pool.Instance.CreatePuzzle((int)newH,(int)newW,tex2d,intervalSize);
        // img.GetComponent<RectTransform>().sizeDelta = new Vector2(newW*intervalSize,newH*intervalSize);
        rawImage.GetComponent<RectTransform>().sizeDelta = new Vector2(newW * intervalSize, newH * intervalSize);
        InitPuzzleGrid();
        StartPanel.SetActive(false);
        drawer.InitScript();
        timer = 0;
        gameStart = true;

    }

    //调整大小 同比例缩放
    void PictureScaleResize(Texture2D tex)
    {
        bool isWidth = tex.width > tex.height ? true:false;
        float longSide = isWidth == true ? tex.width :tex.height;

        bool expand;
        float ratio;
        if (isWidth)
        {
            expand = longSide > basicScale.x ? false:true;
            ratio = expand==true ? longSide/basicScale.x : basicScale.x /longSide;
        }
        else
        {
            expand = longSide > basicScale.y ? false:true;
            ratio = expand==true ? longSide/basicScale.y : basicScale.y /longSide;
        }
        
        newW = expand==true ? tex.width/ratio : tex.width*ratio;
        newH = expand==true ? tex.height/ratio : tex.height*ratio;

        //伪正方形
        if(newH > basicScale.y)
        {
            ratio = basicScale.y / newH;
            newW = newW*ratio;
            newH = newH*ratio;
        }

        newW = (int)newW;
        newH = (int)newH;
    }

    /// <summary>
    /// 初始化格子
    /// </summary>
    void InitPuzzleGrid()
    {
        puzzle = null;
        puzzle = new int[(int)(newW * newH)];
        Debug.Log(puzzle.Length);
        for (int i = 0; i < puzzle.Length; i++)
        {
            puzzle[i] = 99999;
        }
    }

    /// <summary>
    /// 检测该格子下是否有碎片
    /// </summary>
    /// <param name="gridID"></param>
    public bool CheckHavePiece(int gridID)
    {
        if (puzzle.Length <= gridID)
        {
            Debug.LogError("检查格子碎片时超出索引");
            return false;
        }
        if (puzzle[gridID] != 99999)
        {
            return true;
        }     
        return false;
    }

    /// <summary>
    /// 网格填进碎片
    /// </summary>
    /// <param name="gridID">网格ID</param>
    /// <param name="pieceID">碎片ID</param>
    public void SetPiece(int gridID, int pieceID)
    {
        puzzle[gridID] = pieceID;
    }

    /// <summary>
    /// 取出碎片
    /// </summary>
    /// <param name="gridID">网格ID</param>
    public void OutPiece(int gridID)
    {
        puzzle[gridID] = 99999;
    }

    /// <summary>
    /// 检测是否完成拼图
    /// </summary>
    /// <returns></returns>
    public bool IsFinish()
    {
        for (int i = 0; i < puzzle.Length - 1; i++)
        {
            if (puzzle[i] >= puzzle[i + 1])
            {
                return false;
            }
        }

        // if(PlayerPrefs.GetInt("Lv" + level + "in" + index) != 7)
        // {
        //     PlayerPrefs.SetInt("Lv" + level + "in" + index, 7);
        //     PlayerPrefs.Save();
        // }
        if(SaveGame.Exists("Lv" + level + "in" + index) )
        {
            if(SaveGame.Load<int>("Lv" + level + "in" + index) != 7)
                SaveGame.Save<int>("Lv" + level + "in" + index, 7);
        }


        Debug.Log("拼图完成");
        EndPanel.SetActive(true);
        Puzzle_Pool.Instance.Recycle();
        EndTimeText.text = "time used:"+ timeText.text+ "s";
        gameStart = false;
        timer = 0;
        return true;
    }

    public void parseTimer()
    {
        if (gameStart == true)
        {
            gameStart = false;
        }
        else{
            gameStart = true;
        }
    }


    // 返回上级菜单
    public void back()
    {
        level = SaveGame.Load<int>("level");
        string backmenu = "level_0" + level;
        SceneManager.LoadScene(backmenu);
    }


}
