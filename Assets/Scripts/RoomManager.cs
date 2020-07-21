using System;
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

    public void SetRoomContent(RoomType roomType,RoomNode room,Map map,System.Random seed,AnimationCurve curve,List<Vector2Int> doors)
    {
        SetRoomBorder(room,map);
        SetDoors(doors,map);
        switch(roomType)
        {
            case RoomType.Boss:
                SetBossRoom(room,map,seed);
                break;
            case RoomType.Loot:
                SetLootRoom(room,map,seed);
                break;
            case RoomType.Trap:
                SetTrapRoom(room, map,seed,curve);
                break;
            case RoomType.Normal:
                SetNormalRoom(room, map,seed,curve);
                break;
            default:
                break;
        }

    }
    
    private void SetLootRoom(RoomNode room,Map map, System.Random seed)
    {

    }

    private void SetBossRoom(RoomNode room, Map map, System.Random seed)
    {

    }
    private void SetNormalRoom(RoomNode room, Map map, System.Random seed,AnimationCurve curve)
    {
        float n=curve.keys.Sum(key=>key.value);
        Debug.Log(n);
        int floorType = seed.Next((int)TileType.Floor_1,(int)TileType.Floor_3+1);
        for(int y=room.bottomLeft.y+1;y<room.topRight.y;y++)
        {
            for (int x = room.bottomLeft.x+1; x < room.topRight.x; x++)
            {
                map.mapMatrix[x, y] = floorType;
            }
        }
        Vector2Int pos = new Vector2Int(seed.Next(room.bottomLeft.x+1,room.topRight.x-1), seed.Next(room.bottomLeft.y + 1, room.topRight.y - 1));
        BoxFillTiles(room,map,seed,(TileType)seed.Next((int)TileType.Obstacle_1,(int)TileType.Obstacle_3+1),mapsetting,pos);
    }

    private void SetTrapRoom(RoomNode room, Map map, System.Random seed, AnimationCurve curve)
    {
        float n = curve.keys.Sum(key => key.value);
        int floorType = seed.Next((int)TileType.Floor_1, (int)TileType.Floor_3 + 1);
        for (int y = room.bottomLeft.y+1; y < room.topRight.y; y++)
        {
            for (int x = room.bottomLeft.x+1; x <room.topRight.x; x++)
            {
                map.mapMatrix[x, y] = floorType;
            }
        }
        Vector2Int pos = new Vector2Int(seed.Next(room.bottomLeft.x + 1, room.topRight.x), seed.Next(room.bottomLeft.y + 1, room.topRight.y));
        FloodFillTiles(room, map, seed, (TileType)seed.Next((int)TileType.Trap_1, (int)TileType.Trap_3 + 1), pos, mapsetting);
#if UNITY_EDITOR

        Debug.Log("tested");
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
            for (int x = room.bottomLeft.x; x <= room.topRight.x; x++)
            {
                if (y == room.topRight.y || y == room.bottomLeft.y || x == room.bottomLeft.x || x == room.topRight.x)
                    if (map.mapMatrix[x, y] != (int)TileType.Door)
                        map.mapMatrix[x, y] = (int)TileType.Wall;

            }
        }
    }

    public void SetDoors(List<Vector2Int> doors,Map map)
    {
        foreach(Vector2Int door in doors)
        {
            map.mapMatrix[door.x, door.y] =(int) TileType.Door;
        }
    }

    private void FloodFillTiles(RoomNode room,Map map,System.Random seed,TileType tileType,Vector2Int pos,MapSetting mapSetting)
    {
        Queue<Vector2Int> tileToEvaluate = new Queue<Vector2Int>();
        tileToEvaluate.Enqueue(pos);
        map.mapMatrix[pos.x, pos.y] =(int) tileType;
        //map.mapMatrix[pos.x, pos.y] = (int)tileType;
        int curNum = 1;
        int wall = (int)TileType.Wall;
        int door = (int)TileType.Door;
        int self = (int)tileType;
        int maxBlockNum = room.Size*mapSetting.trapPercentage/100;
        int totalNum = seed.Next(1, maxBlockNum + 1);
        while(curNum<totalNum)
        {
            Vector2Int curPos = tileToEvaluate.Dequeue();
            if(map.mapMatrix[curPos.x-1,curPos.y]!=door&& map.mapMatrix[curPos.x - 1, curPos.y] != wall)
            {
                tileToEvaluate.Enqueue(new Vector2Int(curPos.x-1,curPos.y));
                map.mapMatrix[curPos.x - 1, curPos.y] = self;
                curNum++;
            }
            if (map.mapMatrix[curPos.x, curPos.y + 1] != door && map.mapMatrix[curPos.x, curPos.y + 1] != wall)
            {
                map.mapMatrix[curPos.x, curPos.y + 1] = self;
                curNum++;
                tileToEvaluate.Enqueue(new Vector2Int(curPos.x, curPos.y + 1));
            }
            if (map.mapMatrix[curPos.x + 1, curPos.y] != door && map.mapMatrix[curPos.x + 1, curPos.y] != wall)
            {
                map.mapMatrix[curPos.x + 1, curPos.y] = self;
                curNum++;
                tileToEvaluate.Enqueue(new Vector2Int(curPos.x + 1, curPos.y));
            }
            if (map.mapMatrix[curPos.x , curPos.y-1] != door && map.mapMatrix[curPos.x, curPos.y - 1] != wall)
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
}

//public class RoomGenerator
//{

//    private System.Random seed;

//    public RoomGenerator(System.Random seed)
//    {
        
//    }

//}
