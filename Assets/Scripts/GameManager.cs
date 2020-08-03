using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class GameManager : MonoBehaviour {

    private static Dictionary<Type,List<Action>> actions=new Dictionary<Type, List<Action>>();
    private MapGenerator mapGenerator;
    public GameObject playerPrefab;
    private GameObject player;
    private MapSetting mapSetting;
    private System.Random seed;
    private RoomNode startRoom;

    public MapGenerator generator
    {
        get
        {
            return mapGenerator;
        }
    }

    public static Queue<GameObject> playerProjectiles = new Queue<GameObject>();

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start () {
        InitializeGame();
        //foreach(List<Action> action in actions.Values)
        //{
        //    Debug.Log( action.Count);
        //}
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            mapGenerator.tilemap.RefreshAllTiles();
            Debug.Log("r");
        }
	}

    public static void RegisterInitialization(Type type,Action action)
    {
        if(!actions.ContainsKey(type))
        {
            actions.Add(type,new List<Action>());
        }
        actions[type].Add(action);
    }

    private void InitializeGame()
    {
        mapGenerator = this.transform.GetComponent<MapGenerator>();
        mapSetting = mapGenerator.mapSetting;
        seed = new System.Random(mapSetting.seed.GetHashCode());
        //mapGenerator.GenerateBinaryMap();
        startRoom=mapGenerator.InitMapFrameWork(seed,mapSetting);
        InitPlayerInRoom(startRoom);
        foreach(Action action in actions[typeof(CameraController)])
        {
            action();
        }
    }


    private void UpdateMapWithGamingProcess()
    {

    }

    private void InitPlayerInRoom(RoomNode room)
    {
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            player = GameObject.Instantiate(playerPrefab);
        }
        else
            player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = new Vector3((room.bottomLeft.x + room.topRight.x) / 2, (room.bottomLeft.y + room.topRight.y) / 2, 0);
    }
    
    

}

public static class ExtensionClass
{
    public static Vector2Int ConvertToVector2Int(this Vector3Int vector3Int)
    {
        return new Vector2Int(vector3Int.x, vector3Int.y);
    }
}
