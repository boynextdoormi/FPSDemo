using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 合成模块总控制
/// </summary>
public class CraftingpanelController : MonoBehaviour {

    public static CraftingpanelController Instance;

    private Transform m_Transform;

    private CraftingpanelModel m_CraftingpanelModel;
    private CraftingpanelView m_CraftingpanelView;
    private CraftingController m_CraftingController;

    private int tabsNum = 2;
    private int slotsNum = 25;
    private int currentIndex = -1;
    private int materialsCount = 0;         //物品合成需要的材料数.Json
    private int dragMaterialsCount = 0;     //合成图谱槽纯在的材料数.拖拽

    private List<GameObject> slotList;
    private List<GameObject> tabList;
    private List<GameObject> contentsList;
    private List<GameObject> materialsList; //管理拖拽放入的材料

    void Awake()
    {
        Instance = this;
    }
	void Start () {
        Init();
        CreatAllTabs();
        CreatAllContents();
        ResetTabsAndContents(0);

        CreatAllSlots();
	}

    private void Init()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_CraftingpanelModel = gameObject.GetComponent<CraftingpanelModel>();
        m_CraftingpanelView = gameObject.GetComponent<CraftingpanelView>();
        m_CraftingController = m_Transform.Find("Right").GetComponent<CraftingController>();

        slotList = new List<GameObject>();
        tabList = new List<GameObject>();
        contentsList = new List<GameObject>();
        materialsList = new List<GameObject>();

        m_CraftingController.Prefab_InventoryItem = m_CraftingpanelView.Prefab_InventoryItem;
    }
    /// <summary>
    /// 生成全部选项卡
    /// </summary>
    private void CreatAllTabs()
    {
        for (int i = 0; i < tabsNum; i++)
        {
            GameObject go = GameObject.Instantiate<GameObject>(m_CraftingpanelView.Prefab_TabsItem,m_CraftingpanelView.Tabs_Transform);
            Sprite temp = m_CraftingpanelView.ByNameGetSprite(m_CraftingpanelModel.GetTabsIconName()[i]);
            go.GetComponent<CraftingTabItemController>().InitItem(i, temp);
            tabList.Add(go);
        }
    }
    /// <summary>
    /// 生成全部类容
    /// </summary>
    public void CreatAllContents()
    {
        List<List<CraftingCenterItem>> tempList = m_CraftingpanelModel.ByNameGetJsonData("CraftingContentsJsonData");

        for (int i = 0; i < tabsNum; i++)
        {
            GameObject go = GameObject.Instantiate<GameObject>(m_CraftingpanelView.Prefab_Content, m_CraftingpanelView.Contents_Transform);
            go.GetComponent<CraftingContentController>().InitContent(i,m_CraftingpanelView.Prefab_ContentItem,tempList[i]);
            contentsList.Add(go);
        }

    }

    /// <summary>
    /// 重置选项卡和正文区域
    /// </summary>
    public void ResetTabsAndContents(int index)
    {
        if (currentIndex == index) return;
        Debug.Log("Tab:"+index);
        for (int i = 0; i < tabList.Count; i++)
        {
            tabList[i].GetComponent<CraftingTabItemController>().NormalTab();
            contentsList[i].SetActive(false);
        }
        tabList[index].GetComponent<CraftingTabItemController>().ActiveTab();
        contentsList[index].SetActive(true);
        currentIndex = index;
    }

    /// <summary>
    /// 生成所有合成图谱槽
    /// </summary>
    private void CreatAllSlots()
    {
        for (int i = 0; i < slotsNum; i++)
        {
            GameObject go = GameObject.Instantiate<GameObject>(m_CraftingpanelView.Prefab_Slot, m_CraftingpanelView.Center_Transform);
            go.name = "Slot" + i;
            slotList.Add(go);
        }
    }


    /// <summary>
    /// 图谱槽数据填充
    /// </summary>
    private void CreatSlotContents(int id)
    {
        CraftingMapItem temp = m_CraftingpanelModel.GetItemById(id);
        if (temp !=null)
        {

            // 清空上一次的图谱类容
            ResetSlotContents();
            //把图谱内的材料放入背包中
            ResetMaterials();

            //填充图谱
            for (int j = 0; j < temp.MapContents.Length; j++)
            {
                if (temp.MapContents[j] != "0")
                {
                    Sprite sp = m_CraftingpanelView.ByNameGetMaterialSprite(temp.MapContents[j]);
                    slotList[j].GetComponent<CraftingSlotController>().Init(sp,temp.MapContents[j]);
                }

            }
            //最终合成物品展示
            m_CraftingController.Init(temp.MapId, temp.MapName);
            materialsCount = temp.MaterialsCount;
        }
    }

    /// <summary>
    /// 重置图谱类容
    /// </summary>
    private void ResetSlotContents()
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            slotList[i].GetComponent<CraftingSlotController>().Reset();
        }
    }

    /// <summary>
    /// 重置图谱内的材料
    /// </summary>
    private void ResetMaterials()
    {
        List<GameObject> materialsList = new List<GameObject>();
        for (int i = 0; i < slotList.Count; i++)
        {
            Transform tempTransform = slotList[i].transform.Find("InventoryItem");
            if (tempTransform != null)
            {
                materialsList.Add(tempTransform.gameObject);
            }
        }

        //Debug.Log("收集到的材料数:"+materialsList.Count);
        InventorypanelController.Instance.AddItems(materialsList);
    }

    /// <summary>
    /// 对拖入合成物品槽中的物品进行管理
    /// </summary>
    public void DragMaterialsItem(GameObject item)
    {
        materialsList.Add(item);
        dragMaterialsCount++;
        Debug.Log("当前物品合成需要的材料数量:"+materialsCount+",拖拽放入的材料数："+dragMaterialsCount);
        //激活合成按钮
        if (materialsCount==dragMaterialsCount)
        {
            m_CraftingController.ActiveButton();
        }
    }

    /// <summary>
    /// 合成完毕
    /// </summary>
    private void CraftingOK()
    {
        for (int i = 0; i < materialsList.Count; i++)
        {
            InventoryItemController iic = materialsList[i].GetComponent<InventoryItemController>();
            if (iic.Num == 1)
            {
                GameObject.Destroy(materialsList[i]);
            }
            else
            {
                iic.Num--;
            }
        }
        StartCoroutine("ResetMap");
    }

    private IEnumerator ResetMap()
    {
        yield return new WaitForSeconds(2.0f);
        ResetMaterials();
        dragMaterialsCount = 0;
        materialsList.Clear();
    }
}
