using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    private  Transform m_Transform;
    private GameObject m_BuildingPlan;
    private GameObject m_WoodenSpear;

    private GameObject currentWeapon; //当前武器
    private GameObject targetWeapon;  //切换武器
	
	void Start () {
		m_Transform =gameObject.GetComponent<Transform>();
        m_BuildingPlan = m_Transform.Find("PersonCamera/Building Plan").gameObject;
        m_WoodenSpear = m_Transform.Find("PersonCamera/Wooden Spear").gameObject;

        m_WoodenSpear.SetActive(false);
        currentWeapon = m_BuildingPlan;
        targetWeapon = null;
	}
	
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.M))//切换到建造
        {
            targetWeapon = m_BuildingPlan;
            Changed();
        }
        if (Input.GetKeyDown(KeyCode.K))//切换到长矛
        {
            targetWeapon = m_WoodenSpear;
            Changed();
        }
	}

    private void Changed()
    {
        //放下当前武器
        currentWeapon.GetComponent<Animator>().SetTrigger("Holster");
        StartCoroutine("DelayTime");
    }

    IEnumerator DelayTime()
    {
        yield return new WaitForSeconds(1);
         currentWeapon.SetActive(false);
        //目标武器显示
         targetWeapon.SetActive(true);
         currentWeapon = targetWeapon;
    }

}
