using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingContentItemController : MonoBehaviour {

    private Transform m_Transform;

    private Button m_Button;
    private Text m_Text;
    private GameObject m_BG;

    private int id;
    private string name;

    public int Id{ get { return id; } }


	void Awake () {
        m_Transform = gameObject.GetComponent<Transform>();

        m_Button = gameObject.GetComponent<Button>();
        m_Text = m_Transform.Find("Text").GetComponent<Text>();
        m_BG = m_Transform.Find("BG").gameObject;

        m_BG.SetActive(false);
        m_Button.onClick.AddListener(ItemButtonClick);
	}

    public void Init(CraftingCenterItem item)
    {
        this.id = item.ItemId;
        this.name = item.ItemName;
        gameObject.name = "Item" + id;
        m_Text.text = " " + name;
    }

    /// <summary>
    /// 默认状态
    /// </summary>
    public void NormalItem()
    {
        m_BG.SetActive(false);
    }
    /// <summary>
    /// 激活状态
    /// </summary>
    public void ActiveItem()
    {
        m_BG.SetActive(true);
    }

    private void ItemButtonClick()
    {
        SendMessageUpwards("ResetItemState", this);
    }
}
