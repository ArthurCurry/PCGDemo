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
}

public class RoomGenerator
{

    private System.Random seed;

    public RoomGenerator(System.Random seed)
    {

    }

}
