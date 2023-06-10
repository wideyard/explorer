using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PieceType
{
    Grid, TL, TR, T1, T2, BL, BR, B1, B2, L1, L2, R1, R2, C1, C2
}

public class Puzzle_Pool : MonoBehaviour
{
    public static Puzzle_Pool Instance;
    public float maxX, minX, maxY, minY;
    [SerializeField]
    GameObject grid, TL, TR, T1, T2, BL, BR, B1, B2, L1, L2, R1, R2, C1, C2;
    [SerializeField]
    Transform GridPanel,BornPanel;
    List<GameObject> gridList=new List<GameObject>();
    //�����
    Dictionary<string, List<GameObject>> PieceDic = new Dictionary<string, List<GameObject>>();
    //���ڻ���
    List<GameObject> recyclePiece = new List<GameObject>();

    [SerializeField]
    Vector2 textureStartPos, offsetX, offsetY;
    Vector2 tempV2,size;
    Texture2D tex;
    private void Awake()
    {
        Instance = this;
    }

    /// ������Ƭ
    /// <param name="row">��</param>
    /// <param name="col">��</param>
    /// <param name="t">ͼƬ</param>
    /// <param name="interval">���</param>
    public void CreatePuzzle(int row, int col,Texture2D t,float interval)
    {
        tex = t;
        tempV2 = textureStartPos;
        int totalGridNum = row * col;
        size = new Vector2(col * interval, row * interval);
        GameObject g = null;
        //���ɶ�ά����
        for (int i = 0; i < totalGridNum; i++)
        {
            // gridList.Add(Instantiate(grid,puzzlePanel));

            g=GetPiece(PieceType.Grid, i);
            g.transform.SetParent(GridPanel);
            // g.transform.SetParent(puzzlePanel);
            gridList.Add(g);
            recyclePiece.Add(g);
        }
        StartCoroutine(IECreate(row, col));
    }



    /// �������ݿ��������Ƭ Э��
    /// <param name="row">��</param>
    /// <param name="col">��</param>
    /// <returns></returns>
    IEnumerator IECreate(int row,int col)
    {
        yield return new WaitForSeconds(0.1f);
        CreatePieces(row, col);
    }


    void CreatePieces(int row, int col)
    {
        int index=0;
        GameObject pieces = null;
        RectTransform temp;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (CheckCorner(i, j,row,col,ref pieces,index))
                {
                    //���
                }
                else if (CheckSide(i, j,row,col, ref pieces,index))
                {
                    //���
                }
                else
                {
                    //�м���Ƭ
                    if ((i % 2 == 0 && j % 2 == 0) || (i % 2 != 0 && j % 2 != 0))
                    {
                        // pieces = Instantiate(C1);
                        pieces= GetPiece(PieceType.C1, index);

                    }
                    else
                    {
                        // pieces = Instantiate(C2);
                        pieces= GetPiece(PieceType.C2, index);
                    }
                }
                pieces.transform.SetParent(BornPanel);
                //����ά���ӵ�λ�ø�����Ƭ
                pieces.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

                // pieces.transform.SetParent(PiecePanel);
                // pieces.GetComponent<RectTransform>().anchoredPosition = gridList[index].transform.position;
                
                //ÿһ����Ƭ����ͼƬ
                pieces.transform.Find("Mask/texture").GetComponent<RawImage>().texture= tex;
                //���¸�ͼƬ���ô�С
                temp = pieces.transform.Find("Mask/texture").GetComponent<RectTransform>();
                temp.sizeDelta = size;
                //��ͼƬ��ƫ��
                temp.anchoredPosition= tempV2;
                tempV2 += offsetX;
                index++;

                recyclePiece.Add(pieces);
                
            }
            //������ͼƬ��ƫ��
            tempV2.x = textureStartPos.x;
            tempV2 += offsetY;
        }
    }


    /// ����Ƿ�Ϊ��
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <param name="row">��</param>
    /// <param name="col">��</param>
    /// <param name="obj">����</param>
    /// <returns></returns>
    bool CheckSide(int i, int j, int row, int col,ref GameObject obj,int index)
    {
        //�ϱ�
        if (i == 0 && j != 0 && j != col - 1)
        {
            if (j % 2 != 0)
            {
                // obj = Instantiate(T1);
                obj= GetPiece(PieceType.T1, index);

            }
            else
            {
                // obj = Instantiate(T2);
                obj= GetPiece(PieceType.T2, index);

            }
            return true;
        }
        //�±�
        else if (i == row - 1 && j != 0 && j != col - 1)
        {
            if (j % 2 != 0)
            {
                // obj = Instantiate(B1);
                obj= GetPiece(PieceType.B1, index);

            }
            else
            {
                // obj = Instantiate(B2);
                obj= GetPiece(PieceType.B2, index);

            }
            return true;
        }
        else if (j == 0 && i != 0 && j != col - 1)
        {
            if (i % 2 != 0)
            {
                // obj = Instantiate(L1);
                obj= GetPiece(PieceType.L1, index);
            }
            else
            {
                // obj = Instantiate(L2);
                obj= GetPiece(PieceType.L2, index);

            }
            return true;
        }
        else if (j == col - 1 && i != 0 && i != row - 1)
        {
            if (i % 2 != 0)
            {
                obj= GetPiece(PieceType.R1, index);
                // obj = Instantiate(R1);

            }
            else
            {
                obj= GetPiece(PieceType.R2, index);
                // obj = Instantiate(R2);

            }
            return true;
        }

        return false;
    }

    /// ����Ƿ�Ϊ��
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    bool CheckCorner(int i, int j, int row, int col,ref GameObject obj,int index)
    {
        //TL
        if (i == 0 && j == 0)
        {
            obj= GetPiece(PieceType.TL, index);
            // obj = Instantiate(TL);

            return true;
        }
        //TR
        else if (i == 0 && j == col - 1)
        {
            obj= GetPiece(PieceType.TR, index);
            // obj = Instantiate(TR);

            return true;
        }
        //BL
        else if (j == 0 && i == row - 1)
        {
            obj= GetPiece(PieceType.BL, index);
            // obj = Instantiate(BL);

            return true;
        }
        //BR
        else if (j == col - 1 && i == row - 1)
        {
            obj= GetPiece(PieceType.BR,index);
            // obj = Instantiate(BR);
        
            return true;
        }
        return false;
    }

    /// ��������name="type">��Ƭ����</param>
    /// <param name="id">��������</param>
    /// <returns></returns>
    GameObject GetPiece(PieceType type,int id)
    {
        /*
         * 1����ȡ����ת��string
         * 2������Ƿ��и������ƬList
         * 3������List �Ƿ��п��õ���Ƭ
         * 4��������Ƭ
         * 5���������
         */
        string str = type.ToString();
        GameObject o=null;
        if (PieceDic.ContainsKey(str))
        {
            bool find = false;
            foreach( var i in PieceDic[str])
            {
                if (!i.activeSelf)
                {
                    o = i;
                    find=true;
                    break;
                }
            }
            if (!find)
            {
                o = CreatePiece(type);
                PieceDic[str].Add(o);
                //����һ����Ƭ��������list��
            }
        }
        else
        {
            List<GameObject> list = new List<GameObject>();
            //����һ����Ƭ������list��
            o = CreatePiece(type);          
            list.Add(o);            
            PieceDic.Add(str,list);
        }
        o.name = id.ToString();
        o.SetActive(true);
        return o;
    }


    /// �������ʹ���Ԥ����
    /// <param name="type">����</param>
    /// <returns></returns>
    GameObject CreatePiece(PieceType type)
    {
        switch (type)
        {
            case PieceType.Grid:
                return Instantiate(grid);
         
            case PieceType.C1:
                return Instantiate(C1);

            case PieceType.C2:
                return Instantiate(C2);
  
            case PieceType.T1:
                return Instantiate(T1);
  
            case PieceType.T2:
                return Instantiate(T2);
     
            case PieceType.B1:
                return Instantiate(B1);
                
            case PieceType.B2:
                return Instantiate(B2);
   
            case PieceType.R2:
                return Instantiate(R2);
     
            case PieceType.R1:
                return Instantiate(R1);
                
            case PieceType.L1:
                return Instantiate(L1);
  
            case PieceType.L2:
                return Instantiate(L2);
  
            case PieceType.TL:
                return Instantiate(TL);
  
            case PieceType.TR:
                return Instantiate(TR);
    
            case PieceType.BL:
                return Instantiate(BL);

            case PieceType.BR:
                return Instantiate(BR);
            default:
                return null;
        }
    }

    //  ����Ԥ����
    [ContextMenu("Recycle")]
    public void  Recycle()
    {
       foreach(var i in recyclePiece)
        {
            i.SetActive(false);
        }
        gridList.Clear();
        recyclePiece.Clear();
    }
}
