using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

/// <summary>
/// 合成模块数据控制
/// </summary>
public class CraftingpanelModel : MonoBehaviour {

    private Dictionary<int, CraftingMapItem> mapItemDic;

	void Awake () {
        mapItemDic = LoadMapContents("CraftingMapJsonData");
	}


    public string[] GetTabsIconName()
    {
        string[] names = new string[] {  "Icon_House","Icon_Weapon"};
        return names;
    }

    /// <summary>
    /// 通过Json文件名加载文件
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public List<List<CraftingCenterItem>> ByNameGetJsonData(string name)
    {
        List<List<CraftingCenterItem>> temp = new List<List<CraftingCenterItem>>();
        string jsonStr = Resources.Load<TextAsset>("JsonData/" + name).text;
        JsonData jsonData = JsonMapper.ToObject(jsonStr);
        for (int i = 0; i < jsonData.Count; i++)
        {
            List<CraftingCenterItem> tempList = new List<CraftingCenterItem>();
            JsonData jd = jsonData[i]["Type"];
                for (int j = 0; j < jd.Count; j++)
			{
                tempList.Add(JsonMapper.ToObject<CraftingCenterItem>(jd[j].ToJson()));
                //tempList.Add(jd[j]["ItemName"].ToString());
			}
                temp.Add(tempList);
        }
        return temp;
    }

    /// <summary>
    /// 加载合成图谱Json数据
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private Dictionary<int,CraftingMapItem> LoadMapContents(string name)
    {
        Dictionary<int, CraftingMapItem> temp = new Dictionary<int,CraftingMapItem>();   //1.构造数据容器
        string jsonStr = Resources.Load<TextAsset>("JsonData/" + name).text;//2.加载Json文件，返回string数据
        JsonData jsonData = JsonMapper.ToObject(jsonStr);          //3.将string数据转化为JsonData(JsonMapper)
        for (int i = 0; i < jsonData.Count; i++)
        {
            //取临时数据
            int mapId = int.Parse(jsonData[i]["MapId"].ToString());
            string tempStr = jsonData[i]["MapContents"].ToString();
            string[] mapContents = tempStr.Split(',');
            int mapCount = int.Parse(jsonData[i]["MaterialsCount"].ToString());
            string mapName = jsonData[i]["MapName"].ToString();
            //构造对象
            CraftingMapItem item = new CraftingMapItem(mapId, mapCount, mapContents, mapName);
            temp.Add(mapId,item);
        }
        return temp;
    }

    /// <summary>
    /// 通过id获取对应的合成图谱
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public CraftingMapItem GetItemById(int id)
    {
        CraftingMapItem temp = null;
        mapItemDic.TryGetValue(id,out temp);
        return temp;
    }

}
