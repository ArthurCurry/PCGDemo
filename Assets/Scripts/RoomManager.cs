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

    public static RoomManager Instace
    {
        get
        {
            if (instance == null)
                instance = new RoomManager();
            return instance;
        }
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
        SetDoors(doors,map);
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

    private void FloodFillObstacles(RoomNode room,Map map,System.Random seed,TileType tileType,Vector2Int pos,int maxBolckNum)
    {
        Queue<Vector2Int> tileToEvaluate = new Queue<Vector2Int>();
        tileToEvaluate.Enqueue(pos);
        map.mapMatrix[pos.x, pos.y] =(int) tileType;
        //map.mapMatrix[pos.x, pos.y] = (int)tileType;
        int curNum = 1;
        int wall = (int)TileType.Wall;
        int door = (int)TileType.Door;
        int self = (int)tileType;
        int totalNum = seed.Next(1, maxBolckNum + 1);
        while (curNum <= totalNum)
        {
            Vector2Int curPos = tileToEvaluate.Dequeue();
            for (int x = curPos.x - 1; x <= curPos.x + 1; x++)
            {
                for (int y = curPos.y - 1; y <= curPos.y + 1; y++)
                {
                    if (x != curPos.x && y != curPos.y)
                    {
                        if (map.mapMatrix[x, y] != wall && map.mapMatrix[x, y] != door && map.mapMatrix[x, y] != self)
                        {
                            tileToEvaluate.Enqueue(curPos);
                            map.mapMatrix[x, y] = (int)tileType;
                            totalNum += 1;
                        }
                    }
                }
            }
        }
    }
}

public class RoomGenerator
{

    private System.Random seed;

    public RoomGenerator(System.Random seed)
    {

    }

}
