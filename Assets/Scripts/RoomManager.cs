﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Collections;
using System.Linq;

public enum RoomType
{
    Loot,
    Boss,
    Trap,
    Normal,

}

public class RoomManager{

    private static RoomManager instance;
    private MapSetting mapsetting;

    public static RoomManager Instace
    {
        get
        {
            if (instance == null)
                instance = new RoomManager();
            return instance;
        }
    }

    private RoomManager()
    {
        InitData();
    }
    
    public void InitData()
    {
        this.mapsetting = (MapSetting)Resources.Load("Map Setting");
        //Debug.Log(mapsetting == null);
    }

    public List<RoomNode> SetRoomType(ref List<RoomNode> rooms,System.Random seed,AnimationCurve curve)
    {
        rooms= rooms.OrderBy(room=>room.Size).ToList<RoomNode>();
        rooms[0].type = RoomType.Loot;
        rooms[rooms.Count - 1].type = RoomType.Boss;
        for(int i=1;i< rooms.Count-1;i++)
        {
            rooms[i].type = (RoomType)seed.Next((int)RoomType.Trap,Enum.GetNames(typeof(RoomType)).Length);
        }
        return rooms;
    }

    //public static List<RoomNode> SortRoomByArea( List<RoomNode> rooms)
    //{
    //    return rooms.OrderBy();
    //}

    public void SetRoomContent(RoomType roomType,RoomNode room,Map map,System.Random seed,List<Vector2Int> doors,MapSetting mapSetting)
    {
        SetRoomBorder(room,map);
        SetDoors(doors,map);
        switch(roomType)
        {
            case RoomType.Boss:
                SetBossRoom(room,map,seed);
                break;
            case RoomType.Loot:
                SetLootRoom(room,map,seed,mapsetting);
                break;
            case RoomType.Trap:
                SetTrapRoom(room, map,seed);
                break;
            case RoomType.Normal:
                SetNormalRoom(room, map,seed);
                break;
            default:
                break;
        }

    }

    public void SetRoomContentByCoordinate(Map map,System.Random seed,Vector2Int pos,float difficulty)
    {
        
    }
    
    private void SetLootRoom(RoomNode room,Map map, System.Random seed,MapSetting mapSetting)
    {
        int tool = (int)TileType.Tool;
        SetFloors(room,map,seed);
        for (int y = room.bottomLeft.y + 1; y < room.topRight.y; y++)
        {
            for (int x = room.bottomLeft.x + 1; x < room.topRight.x; x++)
            {
                map.mapMatrix[x, y] = (seed.Next(0,100)<mapsetting.lootRoomGadgetPercentage )?tool : map.mapMatrix[x,y];
            }
        }
    }

    private void SetBossRoom(RoomNode room, Map map, System.Random seed)
    {

    }
    private void SetNormalRoom(RoomNode room, Map map, System.Random seed)
    {
        //float n=curve.keys.Sum(key=>key.value);
        //Debug.Log(n);
        //int floorType = seed.Next((int)TileType.Floor_1,(int)TileType.Floor_3+1);
        //for(int y=room.bottomLeft.y+1;y<room.topRight.y;y++)
        //{
        //    for (int x = room.bottomLeft.x+1; x < room.topRight.x; x++)
        //    {
        //        map.mapMatrix[x, y] = floorType;
        //    }
        //}
        int floorType = (int)TileType.Floor;
        SetFloors(room,map,seed);
        Vector2Int pos = new Vector2Int(seed.Next(room.bottomLeft.x+1,room.topRight.x-1), seed.Next(room.bottomLeft.y + 1, room.topRight.y - 1));
        //BoxFillTiles(room,map,seed,(TileType)seed.Next((int)TileType.Obstacle_1,(int)TileType.Obstacle_3+1),mapsetting,pos);
        SetObstales(room,map,seed,seed.Next((int)TileType.Obstacle_1, (int)TileType.Obstacle_3 + 1),mapsetting,floorType);
        SetEnemies(room, map, seed, mapsetting);
    }

    private void SetTrapRoom(RoomNode room, Map map, System.Random seed)
    {
        //float n = curve.keys.Sum(key => key.value);
        //int floorType = seed.Next((int)TileType.Floor_1, (int)TileType.Floor_3 + 1);
        //for (int y = room.bottomLeft.y+1; y < room.topRight.y; y++)
        //{
        //    for (int x = room.bottomLeft.x+1; x <room.topRight.x; x++)
        //    {
        //        map.mapMatrix[x, y] = floorType;
        //    }
        //}
        SetFloors(room,map,seed);
        Vector2Int pos = new Vector2Int(seed.Next(room.bottomLeft.x + 1, room.topRight.x), seed.Next(room.bottomLeft.y + 1, room.topRight.y));
        FloodFillTiles(room, map, seed, (TileType)seed.Next((int)TileType.Trap_1, (int)TileType.Trap_3 + 1), pos, mapsetting);
#if UNITY_EDITOR

        //Debug.Log("tested");
#endif
    }

    //private void SetRoomMatrix()
    //{
    //
    //}

    private void SetRoomBorder(RoomNode room,Map map)
    {
        for (int y = room.bottomLeft.y; y <= room.topRight.y; y++)
        {
            if(y==room.topRight.y)
            {
                for(int x=room.bottomLeft.x+1;x<room.topRight.x;x++)
                {
                    map.mapMatrix[x, y] = (int)TileType.Wall_Up;
                }
            }
            if (y == room.bottomLeft.y)
            {
                for (int x = room.bottomLeft.x+1; x < room.topRight.x; x++)
                {
                    map.mapMatrix[x, y] = (int)TileType.Wall_Down;
                }
            }
            map.mapMatrix[room.bottomLeft.x, y] = (int)TileType.Wall_Left;
            map.mapMatrix[room.topRight.x, y] = (int)TileType.Wall_Right;
        }
    }

    public void SetDoors(List<Vector2Int> doors,Map map)
    {
        foreach(Vector2Int door in doors)
        {
            map.mapMatrix[door.x, door.y] =(int) TileType.Door;
        }
    }

    public void SetFloors(RoomNode room,Map map,System.Random seed)
    {
        int floorType =(int)TileType.Floor;
        for (int y = room.bottomLeft.y + 1; y < room.topRight.y; y++)
        {
            for (int x = room.bottomLeft.x + 1; x < room.topRight.x; x++)
            {
                map.mapMatrix[x, y] = floorType;
            }
        }
    }

    public void SetGadgets(RoomNode room,Map map,System.Random seed)
    {

    }

    private void FloodFillTiles(RoomNode room,Map map,System.Random seed,TileType tileType,Vector2Int pos,MapSetting mapSetting)
    {
        Queue<Vector2Int> tileToEvaluate = new Queue<Vector2Int>();
        tileToEvaluate.Enqueue(pos);
        map.mapMatrix[pos.x, pos.y] =(int) tileType;
        //map.mapMatrix[pos.x, pos.y] = (int)tileType;
        int curNum = 1;
        string wall = TileType.Wall.ToString();
        int door = (int)TileType.Door;
        int self = (int)tileType;
        int maxBlockNum = room.Size*mapSetting.trapPercentage/100;
        int totalNum = seed.Next(1, maxBlockNum + 1);
        while(curNum<totalNum)
        {
            Vector2Int curPos = tileToEvaluate.Dequeue();
            if(map.mapMatrix[curPos.x-1,curPos.y]!=door&&!((TileType)map.mapMatrix[curPos.x - 1, curPos.y]).ToString() .Contains( wall))
            {
                tileToEvaluate.Enqueue(new Vector2Int(curPos.x-1,curPos.y));
                map.mapMatrix[curPos.x - 1, curPos.y] = self;
                curNum++;
            }
            if (map.mapMatrix[curPos.x, curPos.y + 1] != door && !((TileType)map.mapMatrix[curPos.x, curPos.y+1]).ToString().Contains(wall))
            {
                map.mapMatrix[curPos.x, curPos.y + 1] = self;
                curNum++;
                tileToEvaluate.Enqueue(new Vector2Int(curPos.x, curPos.y + 1));
            }
            if (map.mapMatrix[curPos.x + 1, curPos.y] != door && !((TileType)map.mapMatrix[curPos.x + 1, curPos.y]).ToString().Contains(wall))
            {
                map.mapMatrix[curPos.x + 1, curPos.y] = self;
                curNum++;
                tileToEvaluate.Enqueue(new Vector2Int(curPos.x + 1, curPos.y));
            }
            if (map.mapMatrix[curPos.x , curPos.y-1] != door && !((TileType)map.mapMatrix[curPos.x , curPos.y-1]).ToString().Contains(wall))
            {
                map.mapMatrix[curPos.x, curPos.y-1] = self;
                curNum++;
                tileToEvaluate.Enqueue(new Vector2Int(curPos.x, curPos.y - 1));
            }

        }
    }

    private void BoxFillTiles(RoomNode room,Map map, System.Random seed,TileType tileType, MapSetting mapSetting,Vector2Int pos)
    {
        int type = (int)tileType;
        int wall = (int)TileType.Wall;
        int door = (int)TileType.Door;
        int width=seed.Next(1,mapsetting.obstacleBlockNums);
        int height=mapsetting.obstacleBlockNums/width;
        for(int x=pos.x;x<pos.x+width&&x<room.topRight.x;x++)
        {
            for (int y = pos.y; y < pos.y + height&&y<room.topRight.y; y++)
            {
                if (map.mapMatrix[x, y] != door && map.mapMatrix[x, y] != wall)
                {
                    map.mapMatrix[x, y] = type;
                }
            }
        }
    }

    private void SetObstales(RoomNode room,Map map,System.Random seed,int tileType,MapSetting mapSetting,int floor)
    {
        List<Vector2Int> blockPoses = new List<Vector2Int>();
        for(int y=room.bottomLeft.y+1;y<room.topRight.y;y++)
        {
            for (int x = room.bottomLeft.x + 1; x < room.topRight.x; x++)
            {
                map.mapMatrix[x, y] = (seed.Next(0, 100) < mapsetting.obstacleBlockPercentage) ? tileType : map.mapMatrix[x, y];
                if (map.mapMatrix[x, y] == tileType)
                    blockPoses.Add(new Vector2Int(x, y));
            }
        }
        foreach(Vector2Int pos in blockPoses)
        {
            if (GetSurroundingSelves(pos.x, pos.y, map) < mapsetting.minObstacleSize)
                map.mapMatrix[pos.x, pos.y] = floor;
        }
    }

    /// <summary>
    /// 获取周围和自己类型相同瓦片的数量（四邻域）
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public int GetSurroundingSelves(int x,int y,Map map)
    {
        float type = map.mapMatrix[x, y];
        int surroundingNum = 0;
        if (map.mapMatrix[x + 1, y].Equals(type))
            surroundingNum += 1;
        if (map.mapMatrix[x - 1, y].Equals(type))
            surroundingNum += 1;
        if (map.mapMatrix[x, y + 1].Equals(type))
            surroundingNum += 1;
        if (map.mapMatrix[x, y - 1].Equals(type))
            surroundingNum += 1;

        //for(int X=x-1;X<=x+1;X++)
        //{
        //    for (int Y = y - 1; Y <= y + 1; Y++)
        //    {
        //        if(X!=x&&y!=Y)
        //            if (map.mapMatrix[X - 1, Y].Equals(type))
        //                surroundingNum += 1;
        //    }
        //}
        return surroundingNum;
    }

    public void SetEnemies(RoomNode room,Map map,System.Random seed,MapSetting mapSetting )
    {
        int enemy = (int)TileType.Enemy;
        int enemyNum = (room.Size * mapsetting.enemyPercentage / 100 >= mapsetting.maxEnemyNum) ? mapsetting.maxEnemyNum :(room.Width-2)*(room.Height-2)*mapsetting.enemyPercentage/100;
        for(int n=0;n<enemyNum;n++ )
        {
            int x = seed.Next(room.bottomLeft.x + 1, room.topRight.x);
            int y = seed.Next(room.bottomLeft.y+1,room.topRight.y);
            if (((TileType)map.mapMatrix[x, y]).ToString().Contains("Floor"))
                map.mapMatrix[x, y] = enemy;
        }
    }
}

//public class RoomGenerator
//{

//    private System.Random seed;

//    public RoomGenerator(System.Random seed)
//    {
        
//    }

//}
