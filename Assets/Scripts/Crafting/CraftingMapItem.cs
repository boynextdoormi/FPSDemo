using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingMapItem{

    private int mapId;
    private string[] mapContents;
    private int materialsCount;
    private string mapName;

    public int MapId
    {
        get { return mapId; }
        set { mapId = value; }
    }
    public int MaterialsCount
    {
        get { return materialsCount; }
        set { materialsCount = value; }
    }
    public string[] MapContents
    {
        get { return mapContents; }
        set { mapContents = value; }
    }
    public string MapName
    {
        get { return mapName; }
        set { mapName = value; }
    }

    public CraftingMapItem(int mapId,int materialsCount, string[] mapContents, string mapName)
    {
        this.mapId = mapId;
        this.mapName = mapName;
        this.mapContents = mapContents;
        this.materialsCount = materialsCount;
    }

    public override string ToString()
    {
        return string.Format("ID:{0}," + "map:{1}," + "name:{2}," + "count:{3}", this.mapId, this.mapContents.Length, this.mapName, this.materialsCount);
    }

}
