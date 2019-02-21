using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//资源加载工具类
public  sealed class ResourcesTools {

    /// <summary>
    /// 加载文件夹资源
    /// </summary>
    public static Dictionary<string, Sprite> LoadFloderAssets(string floderName, Dictionary<string, Sprite> dic)
    {
        Sprite[] tempSprite = Resources.LoadAll<Sprite>(floderName);
         for (int i = 0; i < tempSprite.Length; i++)
         {
             dic.Add(tempSprite[i].name, tempSprite[i]);
         }
         return dic;
    }


    /// <summary>
    /// 通过名字获取资源
    /// </summary>
    public static Sprite GetAssets(string fileName, Dictionary<string, Sprite> dic)
    {
        Sprite temp = null;
        dic.TryGetValue(fileName, out temp);
        return temp;
    }


}
