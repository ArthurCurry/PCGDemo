using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {

    private static Dictionary<Type,List<Action>> actions=new Dictionary<Type, List<Action>>();
    private MapGenerator mapGenerator;
    public GameObject playerPrefab;
    private GameObject player;
    public static MapSetting mapSetting;
    private System.Random seed;
    private RoomNode startRoom;
   
    public GameObject playerdisplay;
    public GameObject bossdisplay;
    private UIManager UIManager;

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
        UIManager.ActiveUI();
        //foreach(List<Action> action in actions.Values)
        //{
        //    Debug.Log( action.Count);
        //}
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //mapGenerator.tilemap.RefreshAllTiles();
            //Debug.Log("r");
            DiffultyAdjuster.EvaluatePotionType();
        }
        UIManager.UpdateCharacterUI();
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
        UIManager = new UIManager(playerdisplay,bossdisplay);
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
        Vector2Int pos = room.Path[seed.Next(0, room.Path.Count)];
        player.transform.position = new Vector3(pos.x,pos.y,0);
    }
    
    

}

public static class ExtensionClass
{
    public static Vector2Int ConvertToVector2Int(this Vector3Int vector3Int)
    {
        return new Vector2Int(vector3Int.x, vector3Int.y);
    }
}

public class UIManager
{
    public delegate void UpdateText(params Text[] targets);
    public static Dictionary<string, UpdateText> uiUpdateActions = new Dictionary<string, UpdateText>();

    private GameObject playerdisplay;
    private Text playerHP;
    private Text playerDP;

    private GameObject bossdisplay;
    private Text bossHP;
    private Text bossAttk;

    public UIManager(GameObject playerDisplay,GameObject bossDisplay)
    {
        this.playerdisplay = playerDisplay;
        this.bossdisplay=bossDisplay;
        foreach(Text text in playerDisplay.GetComponentsInChildren<Text>())
        {
            if (text != null)
            {
                if (text.gameObject.name.Contains("hp"))
                    playerHP = text;
                else
                    playerDP = text;
                //Debug.Log(text.gameObject.name);
            }
        }
        foreach (Text text in bossdisplay.GetComponentsInChildren<Text>())
        {
            if (text != null)
            {
                if (text.gameObject.name.Contains("hp"))
                    bossHP = text;
                else
                    bossAttk = text;
            }
            //Debug.Log(text.gameObject.name);

        }
    }

    public void UpdateCharacterUI()
    {
        foreach(KeyValuePair<string,UpdateText> pair in uiUpdateActions)
        {
            if(pair.Key.Equals("Player"))
            {
                //Debug.Log("s");
                pair.Value(playerHP,playerDP);
            }
        }
    }

    public void ActiveUI()
    {
        playerdisplay.SetActive(true);
        bossdisplay.SetActive(true);

    }

    public static void Inform()
    {

    }

    
}
