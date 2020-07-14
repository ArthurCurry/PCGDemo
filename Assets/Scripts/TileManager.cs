using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class TileType
{
    public static string Border = "border";
    public static string Corridor = "corridor";
    public static string Floor = "floor";
    public static string Basic = "basic";
}

public class TileManager {

    private static TileManager instance;
    private Tilemap tilemap;

    public static TileManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TileManager();
            }
            return instance;
        }
    }

    private TileManager()
    {
        InitData();
    }


    public void LayTiles(Map map,List<RoomNode> rooms,List<Vector2Int> corridors,Tilemap tilemap,System.Random seed)
    {
        foreach(RoomNode room in rooms)
        {
            for(int y =room.bottomLeft.y;y<=room.topRight.y;y++)
            {
                for(int x=room.bottomLeft.x;x<=room.topRight.x;x++)
                {
                    
                }
            }
        }
    }

    private void LaySingleTile(TileType tileType,int x,int y,Tilemap tilemap)
    {

    }

    private void InitData()
    {

    }

}


