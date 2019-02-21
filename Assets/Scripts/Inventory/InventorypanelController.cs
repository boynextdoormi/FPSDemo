using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背包模块总控制器
/// </summary>

public class InventorypanelController : MonoBehaviour,IUIPanelShowHide {

    public static InventorypanelController Instance;

    //持有V和M对象
    private InventorypanelView m_InventorypanelView;
    private InventorypanelModel m_InventorypanelModel;

    private int slotNum =27;
    private List<GameObject> slotList = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }
	void Start () {

        m_InventorypanelView = gameObject.GetComponent<InventorypanelView>();
        m_InventorypanelModel = gameObject.GetComponent<InventorypanelModel>();
        
        CreatAllSlot();
        CreatAllItem();
        
	}
    /// <summary>
    /// 生成全部物品槽
    /// </summary>
    private void CreatAllSlot()
    {
        for (int i = 0; i < slotNum; i++)
        {
            GameObject tempSlot= GameObject.Instantiate<GameObject>(m_InventorypanelView.Prefab_Slot,m_InventorypanelView.GridTransform);
            tempSlot.name = "InventorySlot_" + i;
            slotList.Add(tempSlot);
        }
    }
    /// <summary>
    /// 生成全部物品
    /// </summary>
    private void CreatAllItem()
    {
        List<InventoryItem> tempList = m_InventorypanelModel.GetJsonList("InventoryJsonData");

        for (int i = 0; i < tempList.Count; i++)
        {
            GameObject temp = GameObject.Instantiate<GameObject>(m_InventorypanelView.Prefab_Item, slotList[i].GetComponent<Transform>());
            temp.GetComponent<InventoryItemController>().InitItem(tempList[i].ItemId, tempList[i].ItemName, tempList[i].ItemNum);
        }
    }

    /// <summary>
    /// 往背包内放入材料
    /// </summary>
    public void AddItems(List<GameObject> itemList)
    {
        int tempIndex = 0;
        for (int i = 0; i < slotList.Count; i++)
        {
            Transform tempTransfrom = slotList[i].transform.Find("InventoryItem");
            //空的物品槽
            if (tempTransfrom == null && tempIndex <itemList.Count)
            {
                itemList[tempIndex].transform.SetParent(slotList[i].transform);
                itemList[tempIndex].GetComponent<InventoryItemController>().InInventory = true;
                tempIndex++;
            }
        }
    }

    /// <summary>
    /// 通过背包父C层实现背包子C层与合成父C层进行交互
    /// </summary>
    /// <param name="item"></param>
    public void SendDragMaterialsItem(GameObject item)
    {
        //通知合成面板C层
        CraftingpanelController.Instance.DragMaterialsItem(item);
    }



    public void UIPanelShow()
    {
        gameObject.SetActive(true);
    }

    public void UIPanelHide()
    {
        gameObject.SetActive(false);
    }
}
