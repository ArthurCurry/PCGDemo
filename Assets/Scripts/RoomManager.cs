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
    //Cave
}

public class RoomManager{

    private static RoomManager instance;
    private MapSetting mapsetting;

    public static RoomManager Instance
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

    public List<RoomNode> SetRoomsType(ref List<RoomNode> rooms,System.Random seed,AnimationCurve curve=null)
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

    public void SetRoomContent(RoomType roomType,RoomNode room,Map map,System.Random seed,MapSetting mapSetting)
    {
        //SetDoors(doors, map);
        room.FindPath();
        switch (roomType)
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
            //case RoomType.Cave:
            //    SetCaveRoom(room,map, mapsetting, seed);
            //    break;
            default:
                break;
        }
        SetRoomCorridors(room, map);
        foreach(Vector2Int door in room.DoorWays)
        {
            Debug.Log(door);
        }
        //SetRoomBorder(room, map);
    }

    private void SetCaveRoom(RoomNode room, Map map,MapSetting mapsetting, System.Random seed)
    {
        //SetFloors(room, map, seed, mapsetting, (int)TileType.Obstacle_1);
        //BoxFillFloors(room, map, seed, mapsetting);
        //for(int x =room.bottomLeft.x;x<=room.topRight.x;x++)
        //{
        //    for (int y = room.bottomLeft.y; y <= room.topRight.y; y++)
        //    {
        //        if(map.mapMatrix[x,y].Equals((int)TileType.Border)&&GetSurroundingSelves(x,y,map)<=2)
        //        {
        //            map.mapMatrix[x, y] = (int)TileType.Floor;
        //        }
        //    }
        //}
        SetFloors(room, map, seed);


    }

    public void SetRoomContentByCoordinate(Map map,System.Random seed,Vector2Int pos,float difficulty)
    {
        
    }
    
    private void SetRoomCorridors(RoomNode room,Map map)
    {
        foreach(Corridor corridor in room.corridors)
        {
            foreach(Vector2Int pos in corridor.coordinates)
            {
                map.mapMatrix[pos.x, pos.y] = (int)TileType.Corridor;
            }
        }
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
        //for (int i = 0; i < mapsetting.trapBlockNum; i++)
        //{
        //    Vector2Int pos = new Vector2Int(seed.Next(room.bottomLeft.x + 1, room.topRight.x), seed.Next(room.bottomLeft.y + 1, room.topRight.y));
        //    FloodFillTiles(room, map, seed, TileType.Trap, pos, mapsetting);
        //}

        SetHollows((int)TileType.Border, mapsetting, map, seed, room);
        
        SetEnemies(room,map,seed,mapsetting);
#if UNITY_EDITOR

        //Debug.Log("tested");
#endif
    }

    //private void SetRoomMatrix()
    //{
    //
    //}

    //private void SetRoomBorder(RoomNode room,Map map)
    //{
    //    for (int y = room.bottomLeft.y; y <= room.topRight.y; y++)
    //    {
    //        if(y==room.topRight.y)
    //        {
    //            for(int x=room.bottomLeft.x+1;x<room.topRight.x;x++)
    //            {
    //                map.mapMatrix[x, y] = (int)TileType.Wall_Up;
    //            }
    //        }
    //        if (y == room.bottomLeft.y)
    //        {
    //            for (int x = room.bottomLeft.x+1; x < room.topRight.x; x++)
    //            {
    //                map.mapMatrix[x, y] = (int)TileType.Wall_Down;
    //            }
    //        }
    //        map.mapMatrix[room.bottomLeft.x, y] = (int)TileType.Wall_Left;
    //        map.mapMatrix[room.topRight.x, y] = (int)TileType.Wall_Right;
    //    }
    //}

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
        for (int y = room.bottomLeft.y ; y <=room.topRight.y; y++)
        {
            for (int x = room.bottomLeft.x ; x <=room.topRight.x; x++)
            {
                map.mapMatrix[x, y] = floorType;
            }
        }
    }

    public void BoxFillFloors(RoomNode room,Map map,System.Random seed,MapSetting mapsetting)
    {
        int boxWidth = room.Width / 2;
        int boxHeigh = room.Height / 2;
        int floorNum=0;
        for(int n=0;n<room.corridors.Count;n++)
        {
            Vector2Int pos=Vector2Int.zero;
            if(room.corridors[n].direction.Equals(Direction.Horizontal))
            {
                if (room.corridors[n].coordinates[0].x < room.bottomLeft.x)
                    pos = new Vector2Int(room.bottomLeft.x, room.corridors[n].coordinates[0].y);
                else
                    pos = new Vector2Int(room.topRight.x, room.corridors[n].coordinates[0].y);
            }
            else if (room.corridors[n].direction.Equals(Direction.Vertical))
            {
                if (room.corridors[n].coordinates[0].x < room.bottomLeft.x)
                    pos = new Vector2Int(room.corridors[n].coordinates[0].x, room.bottomLeft.y);
                else
                    pos = new Vector2Int(room.corridors[n].coordinates[0].x, room.topRight.y);
            }
            BoxFill(room, map, (int)TileType.Floor, pos, boxWidth, boxHeigh);
        }
        for(int i =room.corridors.Count;i<=mapsetting.caveNum;i++)
        {
            Vector2Int pos = new Vector2Int(seed.Next(room.bottomLeft.x, room.topRight.x + 1), seed.Next(room.bottomLeft.y, room.topRight.y + 1));
            BoxFill(room, map, (int)TileType.Floor, pos, boxWidth, boxHeigh);
        }
    }

    public void BoxFill(RoomNode room,Map map,int tileType,Vector2Int centre,int width,int height)
    {
        for (int x = centre.x - width / 2; x <= centre.x + width / 2; x++)
        {
            for (int y = centre.y - height / 2; y <= centre.y + height / 2; y++)
            {
                if (room.ContainsCoordinate(x, y))
                {
                    if (map.mapMatrix[x, y] != tileType)
                    {
                        map.mapMatrix[x, y] = tileType;
                        //floorNum++;
                    }
                }
            }
        }
    }

    private void SetHollows(int type,MapSetting mapSetting,Map map,System.Random seed,RoomNode room)
    {
        int hollowNums = seed.Next(mapsetting.caveNum/2,mapsetting.caveNum);
        for(int i =0;i<=hollowNums;i++)
        {
            Vector2Int pos = new Vector2Int(seed.Next(room.bottomLeft.x+2, room.topRight.x ), seed.Next(room.bottomLeft.y+2, room.topRight.y ));
            int sizeX = seed.Next(mapsetting.minHollowSize.x, mapSetting.maxHollowSize.x);
            int sizeY = seed.Next(mapsetting.minHollowSize.y, mapSetting.maxHollowSize.y);

            
            for(int x=pos.x;x<pos.x+sizeX;x++)
            {
                for(int y=pos.y;y<pos.y+sizeY;y++)
                {
                    if(room.ContainsCoordinate(x,y)&&!room.Path.Contains(new Vector2Int(x,y)))
                    {
                        map.mapMatrix[x, y] = type;

                    }
                }
            }
        }
    }

    private void FillCaveRoomFloor(RoomNode room,Map map,MapSetting mapSetting)
    {
        System.Random seed = new System.Random(mapSetting.seed.GetHashCode());
        int width = room.Width / 2;
        int height = room.Height / 2;
    }

    private void FloodFillTiles(RoomNode room,Map map,System.Random seed,TileType tileType,Vector2Int pos,MapSetting mapSetting)
    {
        Queue<Vector2Int> tileToEvaluate = new Queue<Vector2Int>();
        tileToEvaluate.Enqueue(pos);
        map.mapMatrix[pos.x, pos.y] =(int) tileType;
        //map.mapMatrix[pos.x, pos.y] = (int)tileType;
        int curNum = 1;
        string wall = TileType.Wall.ToString();
        string floor = TileType.Floor.ToString();
        int door = (int)TileType.Door;
        int self = (int)tileType;
        int maxBlockNum = room.Size*mapSetting.trapPercentage/100;
        int totalNum = seed.Next(1, maxBlockNum + 1);
        while(curNum<totalNum)
        {
            Vector2Int curPos = tileToEvaluate.Dequeue();
            if(map.mapMatrix[curPos.x-1,curPos.y]!=door&&((TileType)map.mapMatrix[curPos.x - 1, curPos.y]).ToString() .Contains( floor))
            {
                tileToEvaluate.Enqueue(new Vector2Int(curPos.x-1,curPos.y));
                map.mapMatrix[curPos.x - 1, curPos.y] = self;
                curNum++;
            }
            if (map.mapMatrix[curPos.x, curPos.y + 1] != door && ((TileType)map.mapMatrix[curPos.x, curPos.y+1]).ToString().Contains(floor))
            {
                map.mapMatrix[curPos.x, curPos.y + 1] = self;
                curNum++;
                tileToEvaluate.Enqueue(new Vector2Int(curPos.x, curPos.y + 1));
            }
            if (map.mapMatrix[curPos.x + 1, curPos.y] != door && ((TileType)map.mapMatrix[curPos.x + 1, curPos.y]).ToString().Contains(floor))
            {
                map.mapMatrix[curPos.x + 1, curPos.y] = self;
                curNum++;
                tileToEvaluate.Enqueue(new Vector2Int(curPos.x + 1, curPos.y));
            }
            if (map.mapMatrix[curPos.x , curPos.y-1] != door && ((TileType)map.mapMatrix[curPos.x , curPos.y-1]).ToString().Contains(floor))
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

    public int GetSurroundingType(int x, int y, Map map,int tileType)
    {
        //float type = map.mapMatrix[x, y];
        int surroundingNum = 0;
        if (map.mapMatrix[x + 1, y].Equals(tileType))
            surroundingNum += 1;
        if (map.mapMatrix[x - 1, y].Equals(tileType))
            surroundingNum += 1;
        if (map.mapMatrix[x, y + 1].Equals(tileType))
            surroundingNum += 1;
        if (map.mapMatrix[x, y - 1].Equals(tileType))
            surroundingNum += 1;
        if (map.mapMatrix[x-1, y - 1].Equals(tileType))
            surroundingNum += 1;
        if (map.mapMatrix[x+1, y - 1].Equals(tileType))
            surroundingNum += 1;
        if (map.mapMatrix[x+1, y + 1].Equals(tileType))
            surroundingNum += 1;
        if (map.mapMatrix[x-1, y + 1].Equals(tileType))
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
