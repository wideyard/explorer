using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    Animation anim;
    bool isDown = false;
    RectTransform rectTran;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        rectTran= GetComponent<RectTransform>();
        InitScript();
    }

    public void DrawerControl()
    {
        if (isDown)
        {
            anim.Play("drawerDown");
        }
        else
        {
            anim.Play("drawerUp");
        }
        isDown = !isDown;
    }
    /// <summary>
    /// 更换新图片时的初始化
    /// </summary>
    public void InitScript()
    {
        isDown = false;
        rectTran.anchoredPosition = new Vector2(0,-545);
    }

}
