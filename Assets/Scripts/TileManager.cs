using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileType
{
    public static string Border = "border";
    public static string Corridor = "corridor";
    public static string Floor = "floor";
    public static string Basic = "basic";
}

public class TileManager {

    private TileManager instance;

    public TileManager Instance
    {
        get
        {
            if(instance==null)
            {
                instance = new TileManager();
            }
            return instance;
        }
    }


}


