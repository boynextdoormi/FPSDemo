using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBarPanelController : MonoBehaviour {
    public static ToolBarPanelController Instance;


    private ToolBarPanelModel m_ToolBarPanelModel;
    private ToolBarPanelView m_ToolBarPanelView;

    private List<GameObject> slotList = null;  //工具栏Slot集合
    private GameObject currentActive = null;  //当前激活的物品

    void Awake()
    {
        Instance = this;
    }

	void Start () {

        Init();
        CreatAllSlot();
	}

    private void Init()
    {
        m_ToolBarPanelModel = gameObject.GetComponent<ToolBarPanelModel>();
        m_ToolBarPanelView = gameObject.GetComponent<ToolBarPanelView>();

        slotList = new List<GameObject>();
    }

    /// <summary>
    /// 生成工具栏所有物品槽
    /// </summary>
    private void CreatAllSlot()
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject slot = GameObject.Instantiate<GameObject>(m_ToolBarPanelView.Prefab_ToolBarSlot,m_ToolBarPanelView.Grid_Transform);
            //slot.transform.Find("Key").GetComponent<Text>().text = (i+1).ToString();
            //slot.name = m_ToolBarPanelView.Prefab_ToolBarSlot.name + i;
            slot.GetComponent<ToolBarSlotController>().Init(m_ToolBarPanelView.Prefab_ToolBarSlot.name + i, i + 1);
            slotList.Add(slot);
        }
    }

    /// <summary>
    /// 存储当前激活物品槽和激活物品
    /// </summary>
    private void SaveActiveSlot(GameObject activeSlot)
    {
        
        if (currentActive != null && currentActive != activeSlot)
        {
            currentActive.GetComponent<ToolBarSlotController>().Normal();
            currentActive = null;
        }
        currentActive = activeSlot;
    }

    public void SaveActiveSlotKey(int keyNum)
    {
        if (currentActive != null && currentActive != slotList[keyNum])
        {
            currentActive.GetComponent<ToolBarSlotController>().Normal();
            currentActive = null;
        }
        currentActive = slotList[keyNum];
        currentActive.GetComponent<ToolBarSlotController>().SlotClick();
    }

}
