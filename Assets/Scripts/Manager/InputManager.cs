using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    private bool inventoryState = false;


    void Start()
    {
        InventorypanelController.Instance.UIPanelHide(); 
    }

	void Update () {
        InventoryPanelKey();
        ToolBarPanelKey();

	}
    
    /// <summary>
    /// 背包按键检测
    /// </summary>
    private void InventoryPanelKey()
    {
        if (Input.GetKeyDown(GameConst.InventoryPanelKey))
        {
            if (inventoryState)
            {
                //Debug.Log("隐藏背包");
                inventoryState = false;
                InventorypanelController.Instance.UIPanelHide();
            }
            else
            {
                //Debug.Log("显示背包");
                inventoryState = true;
                InventorypanelController.Instance.UIPanelShow();
            }
        }
    }

    /// <summary>
    /// 工具栏按键检测
    /// </summary>
    private void ToolBarPanelKey()
    {
        ToolBarKey(GameConst.ToolBarPanelKey_1,0);
        ToolBarKey(GameConst.ToolBarPanelKey_2,1);
        ToolBarKey(GameConst.ToolBarPanelKey_3,2);
        ToolBarKey(GameConst.ToolBarPanelKey_4,3);
        ToolBarKey(GameConst.ToolBarPanelKey_5,4);
        ToolBarKey(GameConst.ToolBarPanelKey_6,5);
        ToolBarKey(GameConst.ToolBarPanelKey_7,6);
        ToolBarKey(GameConst.ToolBarPanelKey_8,7);
        
    }

    /// <summary>
    /// 工具栏--单个按键检测
    /// </summary>
    private void ToolBarKey(KeyCode keyCode,int keyNum)
    {
        if (Input.GetKeyDown(keyCode))
        {
            ToolBarPanelController.Instance.SaveActiveSlotKey(keyNum);
        }
    }

}
