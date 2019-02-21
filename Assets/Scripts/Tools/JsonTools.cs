using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
public sealed class JsonTools {

    /// <summary>
    /// 通过文件名加载Json文件
    /// </summary>
    public static List<T> LoadJsonFile<T>(string fileName)
    {
        List<T> tempList = new List<T>();
        string tempJsonStr = Resources.Load<TextAsset>("JsonData/" + fileName).text;
        //解析Json
        JsonData jsondata = JsonMapper.ToObject(tempJsonStr);
        for (int i = 0; i < jsondata.Count; i++)
        {
            T ii = JsonMapper.ToObject<T>(jsondata[i].ToJson());
            tempList.Add(ii);
        }
        return tempList;
    }


}
