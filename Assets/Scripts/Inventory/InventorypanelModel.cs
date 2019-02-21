using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

/// <summary>
/// 背包数据控制
/// </summary>
public class InventorypanelModel : MonoBehaviour {

	void Awake () {
		
	}

    /// <summary>
    /// 通过JSon文件名获取List对象
    /// </summary>
    public List<InventoryItem> GetJsonList(string fileName)
    {
        return JsonTools.LoadJsonFile<InventoryItem>(fileName);
    }
}
