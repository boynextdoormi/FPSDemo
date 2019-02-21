using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 合成模块单个选项卡控制
/// </summary>
public class CraftingTabItemController : MonoBehaviour {

    private Transform m_Transform;
    private Button m_Button;
    private GameObject m_ButtonBG;
    private Image m_Icon;

    private int index = -1;
	void Awake () {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Button = gameObject.GetComponent<Button>();
        m_ButtonBG = m_Transform.Find("Center_BG").gameObject;
        m_Icon = m_Transform.Find("Icon").GetComponent<Image>();
        m_Button.onClick.AddListener(ButtonOnClick);
        
    }

    /// <summary>
    /// 初始化Item
    /// </summary>
    /// <param name="index"></param>
    public void InitItem(int index,Sprite icon)
    {
        this.index = index;
        gameObject.name = "Tab"+index;
        m_Icon.sprite = icon;
    }

    /// <summary>
    /// 选项卡默认状态
    /// </summary>
    public void NormalTab()
    {
        if (m_ButtonBG.activeSelf == false)
        {
             m_ButtonBG.SetActive(true);
        }
    }

    /// <summary>
    /// 选项卡激活状态
    /// </summary>
    public void ActiveTab()
    {
        m_ButtonBG.SetActive(false);
    }

    private void ButtonOnClick()
    {
        SendMessageUpwards("ResetTabsAndContents", index);
    }
}
