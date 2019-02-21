using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBarPanelView : MonoBehaviour {

    private Transform m_Transfrom;
    private Transform grid_Transform;

    private GameObject prefab_ToolBarSlot;


    public Transform M_Transfrom { get { return m_Transfrom; } }
    public Transform Grid_Transform { get { return grid_Transform; } }

    public GameObject Prefab_ToolBarSlot { get { return prefab_ToolBarSlot; } }

	void Awake () {
        m_Transfrom = gameObject.GetComponent<Transform>();
        grid_Transform = m_Transfrom.Find("Grid").GetComponent<Transform>();

        prefab_ToolBarSlot = Resources.Load<GameObject>("ToolBarSlot");

        
	}
	
	
}
