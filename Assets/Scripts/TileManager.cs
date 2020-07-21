using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public enum TileType
{

    Border,
    Corridor,
    Floor,
    Obstacle,
    Trap,
    Monster,
    Floor_1,
    Floor_2,
    Floor_3,
    Obstacle_1,
    Obstacle_2,
    Obstacle_3,
    Trap_1,
    Trap_2,
    Trap_3,
    Monster_1,
    Monster_2,
    Monster_3,
    Door,
    Wall
}


public class TileManager {

    private static TileManager instance;
    public Tilemap tilemap;
    public Dictionary<TileType, TileBase> tiles;
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


    public void LayTiles(Map map,Tilemap tilemap,System.Random seed)
    {
        tilemap.ClearAllTiles();
        for(int y=0;y<map.height;y++)
        {
            for(int x=0;x<map.width;x++)
            {
                LaySingleTile((TileType)map.mapMatrix[x, y], new Vector2Int(x, y), tilemap,seed);
            }
        }
    }
    /// <summary>
    /// 测试用方法
    /// </summary>
    public void Test()
    {
        foreach(Tile tile in tiles.Values)
        {
            Debug.Log(tile.name);
        }
    }

    private void LaySingleTile(TileType tileType,Vector2Int tilePos,Tilemap tilemap,System.Random seed)
    {
        tilemap.SetTile(new Vector3Int(tilePos.x,tilePos.y,0),tiles[tileType]);
    }

    public void InitData()
    {
        if (tiles == null)
            tiles = new Dictionary<TileType, TileBase>();
        else
            tiles.Clear();
        foreach (TileType tileName in Enum.GetValues(typeof(TileType)))
        {
            TileBase tile = (TileBase)Resources.Load(tilePath+tileName.ToString());
            tiles.Add(tileName,tile);
        }
    }

}


