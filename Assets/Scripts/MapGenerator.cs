using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum MapType
{
    PerlinNoise,
    Binary,
    Random
}

public class MapGenerator : MonoBehaviour {


    public Map map;
    [HideInInspector]
    public string seed;
    public bool useRandomSeed;
    public bool preViewMap;
    public MapSetting mapSetting;
    public Tilemap tilemap;
    public static List<RoomNode> rooms;
    public BinarySpacePartitioner bsp;
    //[HideInInspector]
    //public float percentage;
    //[Range(0, 10)]
    //public int minBlockNum;

    private void Awake()
    {
        EventDispatcher.GenerateRoom = new EventDispatcher.GenerateRoomInProcess(GenerateRoomByStep);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    #region 柏林噪声方式
    public Map GenerateNoiseMap(int width,int height)
    {
        map = new Map(width, height);
        //float mapDepth = 0f;
        if (useRandomSeed||seed == null)
            seed = Time.time.ToString();
        System.Random random = new System.Random(seed.GetHashCode());
        for(int x=0;x<width;x++)
        {
            for (int y = 0; y <height; y++)
            {
                float xCord = x / MapUnit.unitScale * random.Next(0, 100);
                float yCord = y / MapUnit.unitScale * random.Next(0, 100);
                //float xCord = x / MapUnit.unitScale;
                //float yCord = y / MapUnit.unitScale;

                map.mapMatrix[x, y] = Mathf.PerlinNoise(xCord, yCord)*2-1;
                if (map.mapMatrix[x, y] < mapSetting.percentage / 100f)
                {
                    map.mapMatrix[x, y] = 0;
                }
            }
        }

        //Debug.Log();
        return map;
    }
    public Map GenerateNoiseMap()
    {
        return GenerateNoiseMap(mapSetting.width,mapSetting.height);
        
    }
    #endregion

    private void DrawMapInEditor(Map map)
    {
        if(map!=null)
        {
            for (int x = 0; x <map.width; x++)
            {
                for (int y = 0; y < map.height; y++)
                {
                    Vector3 position = new Vector3(x*MapUnit.unitScale,y*MapUnit.unitScale,0);
                    Gizmos.color = Color.Lerp(Color.white,Color.black,map.mapMatrix[x,y]/10);
                    if (map.mapMatrix[x, y] == (int)TileType.Door)
                        Gizmos.color = Color.red;
                    //Debug.Log(map.mapMatrix[x, y]);
                    Gizmos.DrawCube(position,Vector3.one*MapUnit.unitScale);
                }
            }
        }
        Gizmos.color = Color.red;
        foreach(RoomNode room in rooms)
        {
            foreach(Vector2Int node in room.Path)
            {
                Gizmos.DrawCube(new Vector3(node.x* MapUnit.unitScale, node.y* MapUnit.unitScale, 0), Vector3.one * MapUnit.unitScale);
            }
        }
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if(preViewMap)
            DrawMapInEditor(map);
#endif
        //DrawMapInEditor(GenerateRandomMap(50,50));
    }

    #region 逐方块随机方式
    public Map GenerateRandomMap(int mapWidth,int mapHeight)
    {
        map = new Map(mapWidth, mapHeight);
        if (useRandomSeed|| seed == null)
            seed = Time.time.ToString();
        System.Random random = new System.Random(seed.GetHashCode());
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {

                map.mapMatrix[x, y] = (random.Next(0, 100) < mapSetting.percentage) ? 0 : 1;
            }
        }
        return map;
    }
    public Map GenerateRandomMap()
    {
        return GenerateRandomMap(mapSetting.width,mapSetting.height);
    }
    #endregion
    //private void SmoothMap(int num)
    //{

    //}

    private void OnValidate()
    {
        
    }

    #region 二元空间分割方式
    public Map GenerateBinaryMap(int mapWidth,int mapHeight)
    {
        //TileManager.Instance.Test();
#if UNITY_STANDALONE
        tilemap.ClearAllTiles();
#endif
        map = new Map(mapWidth,mapHeight);
        if (useRandomSeed||seed==null)
            seed = Time.time.ToString();
        System.Random random = new System.Random(seed.GetHashCode());
        BinarySpacePartitioner bsp = new BinarySpacePartitioner(mapWidth,mapHeight,random,mapSetting.BSPIterationTimes);
        rooms=bsp.SliceMap(mapSetting.minRoomWidth,mapSetting.minRoomHeight,mapSetting.passageWidth,mapSetting.corridorWidth);
        RoomManager.Instance.SetRoomsType(ref rooms,random,mapSetting.RoomTypePercentage);
        foreach (RoomNode room in rooms)
        {
            //for(int y=room.bottomLeft.y;y<=room.topRight.y;y++)
            //{
            //    for(int x=room.bottomLeft.x;x<=room.topRight.x;x++)
            //    {
            //        if (x == 0 || x == mapWidth - 1 || y == 0 || y == mapHeight - 1)
            //            map.mapMatrix[x, y] = (float)TileType.Border;
            //        //else
            //        //    map.mapMatrix[x, y] = (float)TileType.Floor_1;
            //    }
            //}
            //Debug.Log(rooms.IndexOf(room) + "  " + room.type + " " + room.bottomLeft);

            RoomManager.Instance.SetRoomContent(room.type, room, map, random,mapSetting);

        }
        //foreach (Corridor corridor in bsp.corridors)
        //{
        //    foreach (Vector2Int pos in corridor.coordinates)
        //    {
        //        map.mapMatrix[pos.x, pos.y] = (float)TileType.Corridor;
        //    }
        //}
        TileManager.Instance.LayTiles(map,tilemap,random);
        return map; 
    }
    public Map GenerateBinaryMap()
    {
        return GenerateBinaryMap(mapSetting.width,mapSetting.height);
    }
    #endregion



    private void SetRoomType(RoomNode room,RoomType type)
    {

    }


    public void GenerateRoomWithCoordinates(List<Vector2Int> coordinates,Map map,System.Random seed,MapSetting mapSetting,Tilemap tilemap)
    {
        RoomNode room = FindRoomWithCoordinates(coordinates);
        if (room!=null&&!room.isTiled)
        {
            RoomManager.Instance.SetRoomContent(room.type, room, map, seed, mapSetting);
            TileManager.Instance.LayTilsInRoom(map, room, tilemap, seed);
        }
    }

    public void GenerateRoomByStep(List<Vector2Int> coordinates)
    {
        GenerateRoomWithCoordinates(coordinates,map,new System.Random(seed.GetHashCode()),mapSetting,tilemap);
    }

    public static RoomNode FindRoomWithCoordinates(List<Vector2Int> coordinates)
    {
        if(rooms!=null)
        {
            foreach(RoomNode room in rooms)
            {
                if (room.ContainsCoordinates(coordinates))
                    return room;
            }
        }
        return null;
    }

    public RoomNode InitMapFrameWork(System.Random seed,MapSetting mapSetting)
    {
        tilemap.ClearAllTiles();
        map = new Map(mapSetting.width,mapSetting.height);
        TileManager.Instance.LayTiles(map, tilemap, seed);
        bsp = new BinarySpacePartitioner(mapSetting.width,mapSetting.height,seed,mapSetting.BSPIterationTimes);
        rooms = bsp.SliceMap(mapSetting.minRoomWidth,mapSetting.minRoomHeight,mapSetting.passageWidth,mapSetting.corridorWidth);
        RoomManager.Instance.SetRoomsType(ref rooms,seed);
        
        RoomNode startRoom = rooms[seed.Next(0, rooms.Count)];
        while(startRoom.type.Equals(RoomType.Boss))
        {
            startRoom = rooms[seed.Next(0, rooms.Count)];
        }
        RoomManager.Instance.SetRoomContent(startRoom.type,startRoom,map,seed,mapSetting);
        TileManager.Instance.LayTilsInRoom(map, startRoom, tilemap, seed);
        return startRoom;
    }

    public void ClearMap()
    {
        tilemap.ClearAllTiles();
    }

}
