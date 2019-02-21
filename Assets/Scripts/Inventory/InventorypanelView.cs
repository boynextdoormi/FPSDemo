using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背包界面控制
/// </summary>
public class InventorypanelView : MonoBehaviour {

    private Transform m_Transform;
    private Transform grid_Transform;

    private GameObject prefab_Slot;
    private GameObject prefab_Item;

    public Transform Transform { get { return m_Transform; } }
    public Transform GridTransform { get { return grid_Transform; } }
    public GameObject Prefab_Slot { get { return prefab_Slot; } }
    public GameObject Prefab_Item { get { return prefab_Item; } }

	void Awake () {
        m_Transform = gameObject.GetComponent<Transform>();
        grid_Transform = m_Transform.Find("Background/Grid").GetComponent<Transform>();

        prefab_Slot = Resources.Load<GameObject>("InventorySlot");
        prefab_Item = Resources.Load<GameObject>("InventoryItem");
	}

	void Update () {
		
	}
}
