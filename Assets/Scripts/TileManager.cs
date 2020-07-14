using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public enum TileType
{
    Border=0,
    Floor,
    Corridor,
    Floor_1,
    Floor_2,
    Floor_3
}

public class TileManager {

    private static TileManager instance;
    private Tilemap tilemap;
    private Dictionary<TileType, Tile> tiles;
    private string tilePath = "Prefabs/Tiles/";

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
    
    public void Test()
    {
        
    }

    private void LaySingleTile(TileType tileType,int x,int y,Tilemap tilemap)
    {

    }

    private void InitData()
    {
        foreach(TileType tileName in Enum.GetValues(typeof(TileType)))
        {
            Tile tile = (Tile)Resources.Load(tilePath+tileName.ToString());
            if(tile!=null)
            {
                tiles.Add(tileName,tile);
            }
        }

    }

}


