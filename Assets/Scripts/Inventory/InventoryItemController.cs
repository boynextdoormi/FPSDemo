using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 背包内Item物品控制器
/// </summary>
public class InventoryItemController : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler {
    private RectTransform m_RectTransform;
    private CanvasGroup m_CanvasGroup;

    private Image m_Image;       //物品图标
    private Text m_Text;         //物品数量
    private int id;              //自身ID
    private bool isDrag = false; //自身拖拽状态
    public bool inInventory =true;//当前物品是否在背包内，T在背包F在合成图谱内
    private int num = 0;          //物品数量

    private Transform parent;    //物品拖拽过程中的父物体
    private Transform selfParent;//物品自身父物体

    public int Num               //通过属性更新UI(Set方法内部)
    {
        get { return num; } 
        set { num = value; m_Text.text = num.ToString(); } 
    }
    public int Id{ get { return id; } set { id = value; } }
    public bool InInventory
    {
        get { return inInventory; }
        set { 
            inInventory = value;
            m_RectTransform.localPosition = Vector3.zero;
            ResetSpriteSize(m_RectTransform,85,85);
        }
    }

	
	void Awake () {
        FindInit();
	}
    void Update()
    {
        //拖拽状态时，按下鼠标右键进行拆分
        if (Input.GetMouseButtonDown(1) && isDrag)
        {
            BreakMaterials();
        }
    }

    /// <summary>
    /// 查找相关的初始化
    /// </summary>
    private void FindInit()
    {
        m_RectTransform = gameObject.GetComponent<RectTransform>();
        m_CanvasGroup = gameObject.GetComponent<CanvasGroup>();

        m_Image = gameObject.GetComponent<Image>();
        m_Text = m_RectTransform.Find("Num").GetComponent<Text>();

        gameObject.name = "InventoryItem";
        //parent = m_RectTransform.parent.parent.parent.parent;
        parent = GameObject.Find("Canvas").GetComponent<Transform>();
    }

    /// <summary>
    /// 传递数据，初始化背包Item
    /// </summary>
    public void InitItem(int id,string name,int num)
    {
        this.id = id;
        m_Image.sprite = Resources.Load<Sprite>("Item/"+name);
        this.num = num;
        m_Text.text = num.ToString();
    }


    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
        selfParent = m_RectTransform.parent;
        //Debug.Log("开始拖拽");
        m_RectTransform.SetParent(parent);//调整物体UI层数，使之不被遮挡
        m_CanvasGroup.blocksRaycasts = false;//让物体忽略射线，能让射线穿透获取下面的物体
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        //Debug.Log("拖拽中");
        Vector3 pos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(m_RectTransform,eventData.position,eventData.enterEventCamera,out pos);
        m_RectTransform.position = pos;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("拖拽结束");
        GameObject target = eventData.pointerEnter;

        ItemDrag(target);

        //通用重置代码
        m_CanvasGroup.blocksRaycasts = true;//重新让物体接受射线，能被再次选中
        m_RectTransform.localPosition = Vector3.zero;//重置位置，注意position与localposition
        isDrag = false;
    }

    private void ResetSpriteSize(RectTransform rectTransform,float width,float height)
    {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,width);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    /// <summary>
    /// 材料拆分
    /// </summary>
    private void BreakMaterials()
    {
        //将当前拖拽中的物品复制一份，B
        GameObject tempB =GameObject.Instantiate<GameObject>(gameObject);
        RectTransform tempTransform = tempB.GetComponent<RectTransform>();
        //重置B的属性
        tempTransform.SetParent(selfParent);
        tempTransform.localPosition = Vector3.zero;
        tempTransform.localScale = Vector3.one;
        //数量拆分
        int tempCount = num;  //总数量
        int tempNumB = tempCount / 2;
        int tempNumA = tempCount - tempNumB;
        //数量更新
        tempB.GetComponent<InventoryItemController>().Num = tempNumB;
        Num = tempNumA;
        //恢复射线检测
        tempB.GetComponent<CanvasGroup>().blocksRaycasts = true;
        //物品Id值初始化
        tempB.GetComponent<InventoryItemController>().Id = Id;
    }

    /// <summary>
    /// 合成材料合并
    /// </summary>
    private void MergMaterials(InventoryItemController target) 
    {
        target.Num += Num;//数量和并
        GameObject.Destroy(gameObject);
    }


    /// <summary>
    /// 物品拖拽逻辑
    /// </summary>
    private void ItemDrag(GameObject target)
    {
        //正常拖拽逻辑
        if (target != null)
        {
            #region 空的物品槽&&非物品槽
            //拖拽到了空的物品槽位置
            if (target.tag == "InventorySlot")
            {
                m_RectTransform.SetParent(target.transform);//重设父物体
                ResetSpriteSize(m_RectTransform, 85, 85);
                InInventory = true;
            }
            //拖拽到了非物品槽位置
            if (target.tag != "InventorySlot")
            {
                m_RectTransform.SetParent(selfParent);
            }
            #endregion

            #region 物品位置交换
            if (target.tag == "InventoryItem")
            {
                InventoryItemController iic =target.GetComponent<InventoryItemController>();

                //只有背包内的物品能交换位置
                if (inInventory && iic.InInventory)
                {
                    //背包内同ID物品合并
                    if (id == iic.Id)
                    {
                        //合并材料
                        MergMaterials(iic);
                    }
                    else
                    {
                        Transform tempTarget = target.GetComponent<Transform>();
                        m_RectTransform.SetParent(tempTarget.parent);
                        tempTarget.SetParent(selfParent);
                        tempTarget.localPosition = Vector3.zero;
                    }
                }
                else
                {
                    //背包与图谱同ID物品合并
                    if (id == iic.Id && iic.InInventory)
                    {
                        //合并材料
                        MergMaterials(iic);
                    }
                }
            }
            #endregion

            #region 拖拽到了图谱槽位置
            if (target.tag == "CraftingSlot")
            {
                //当前物品槽可以接受物品
                if (target.GetComponent<CraftingSlotController>().IsOpen)
                {
                    //判断物品与图谱槽物品是否相同
                    if (id == target.GetComponent<CraftingSlotController>().Id)
                    {
                        m_RectTransform.SetParent(target.transform);//重设父物体
                        ResetSpriteSize(m_RectTransform, 70, 62);
                        inInventory = false;
                        //通知合成面板C层   子C层->父C层 ->合成C层
                        //CraftingpanelController.Instance.DragMaterialsItem(gameObject);
                        InventorypanelController.Instance.SendDragMaterialsItem(gameObject);
                    }
                    else //与图谱槽不同
                    {
                        m_RectTransform.SetParent(selfParent);
                    }

                }
                else//当前物品槽不能接受物品
                {
                    m_RectTransform.SetParent(selfParent);
                }

            }
            #endregion
        }
        else//拖拽到非UI区域
        {
            m_RectTransform.SetParent(selfParent);
        }
    }
}
