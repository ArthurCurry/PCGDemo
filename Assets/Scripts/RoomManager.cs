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

    public void SetRoomContent(RoomType roomType,RoomNode room,Map map,System.Random seed)
    {
        switch(roomType)
        {
            case RoomType.Boss:
                SetBossRoom(room,map,seed);
                break;
            case RoomType.Loot:
                SetLootRoom(room,map,seed);
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
    
    private void SetLootRoom(RoomNode room,Map map, System.Random seed)
    {

    }

    private void SetBossRoom(RoomNode room, Map map, System.Random seed)
    {

    }
    private void SetNormalRoom(RoomNode room, Map map, System.Random seed)
    {

    }

    private void SetTrapRoom(RoomNode room, Map map, System.Random seed)
    {

    }
}
