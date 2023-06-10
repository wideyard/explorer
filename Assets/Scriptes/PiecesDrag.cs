using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PiecesDrag : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    //遮罩的图片
    Image maskImage;
    //鼠标点击偏移
    Vector3 offset;
    //挪动起始点，移动失败会回到原来的位置上
    Vector3 originPos;
    //转换坐标系输出的坐标
    Vector3 outPos;
    RectTransform rectTrans;
    public Transform PieceArea;
    // Start is called before the first frame update
    void Start()
    {
        maskImage = GetComponent<Image>();
        //透明度小于0.1将无法触发射线
        maskImage.alphaHitTestMinimumThreshold = 0.1f;
        //获取最上方父节点的RectTransform
        rectTrans = transform.parent.parent.GetComponent<RectTransform>();
        PieceArea = GameObject.Find("PieceArea").GetComponent<RectTransform>();
    }
 
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTrans, Input.mousePosition
     , eventData.enterEventCamera, out outPos))
        {
            originPos = rectTrans.anchoredPosition;
            rectTrans.SetAsLastSibling();
            offset = (Vector3)rectTrans.anchoredPosition - outPos;

            ///对网格数据的处理
            RaycastHit2D[] hits2d = Physics2D.RaycastAll(eventData.position, Vector2.zero);

            //该网格有一片碎片
            foreach (var h in hits2d)
            {
                if (h.collider.transform.gameObject.layer == 9)
                {
                    int gridID = int.Parse(h.collider.gameObject.name);
                    if (puzzle_pictures.Instance.CheckHavePiece(gridID))
                    {
                        puzzle_pictures.Instance.OutPiece(gridID);
                        Debug.Log("拿出碎片");
                    }
                }
            }
        }
    }

  

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.position.y > 200)
        {
            rectTrans.SetParent(PieceArea);
        }
        //TODO 检查是否移动失败和游戏是否完成
        //如：移动到其他碎片上方，移动至格子上等
        RaycastHit2D[] hits2d = Physics2D.RaycastAll(eventData.position, Vector2.zero);
        if (hits2d.Length > 1)
        {
            //放置碎片
            foreach (var h in hits2d)
            {
                if (h.collider.transform.gameObject.layer == 9)
                {
                    //我们在生成时命名的name可以当索引，当然大家也可以自己做一个类去设置ID
                    int gridID = int.Parse(h.collider.gameObject.name);
                    ///检查这个位置是不是没有其他碎片
                    if (!puzzle_pictures.Instance.CheckHavePiece(gridID))
                    {
                        rectTrans.anchoredPosition = h.collider.transform.GetComponent<RectTransform>().position;
                        puzzle_pictures.Instance.SetPiece(gridID, int.Parse(rectTrans.name));
                        rectTrans.SetAsFirstSibling();
                        Debug.Log(rectTrans.name);
                        if (puzzle_pictures.Instance.IsFinish())
                        {
                            Debug.Log("完成");
                        }
                    }
                    // else//格子已有碎片
                    // {
                    //     rectTrans.anchoredPosition = originPos;
                    //     Debug.Log("放入失败");
                    //     return;
                    // }
                   
                }
            }
            return;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTrans.anchoredPosition = offset + Input.mousePosition;
    }
}
