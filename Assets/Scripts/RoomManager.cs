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
    
    public List<RoomNode> SetRoomType(List<RoomNode> rooms,System.Random seed,AnimationCurve curve)
    {
        List<RoomNode> sortedRoom = rooms.OrderBy(room=>room.Size).ToList<RoomNode>();
        sortedRoom[0].type = RoomType.Loot;
        sortedRoom[sortedRoom.Count - 1].type = RoomType.Boss;
        for(int i=1;i<sortedRoom.Count-1;i++)
        {
            sortedRoom[i].type = (RoomType)seed.Next((int)RoomType.Trap,Enum.GetNames(typeof(RoomType)).Length);
        }
        return sortedRoom;
    }

    //public static List<RoomNode> SortRoomByArea( List<RoomNode> rooms)
    //{
    //    return rooms.OrderBy();
    //}

    public void SetRoomContent(RoomType roomType,RoomNode room,Map map)
    {
        switch(roomType)
        {
            case RoomType.Boss:
                SetBossRoom(room,map);
                break;
            case RoomType.Loot:
                SetLootRoom(room,map);
                break;
            case RoomType.Trap:
                SetTrapRoom(room, map);
                break;
            case RoomType.Normal:
                SetNormalRoom(room, map);
                break;
        }
    }
    
    private void SetLootRoom(RoomNode room,Map map)
    {

    }

    private void SetBossRoom(RoomNode room, Map map)
    {

    }
    private void SetNormalRoom(RoomNode room, Map map)
    {

    }

    private void SetTrapRoom(RoomNode room, Map map)
    {

    }
}
