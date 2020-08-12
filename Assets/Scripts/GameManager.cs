using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;


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
    public GameObject nextLevelButton;
    public GameObject exitButton;
    public GameObject bossDeadUI;
    public GameObject restartMenu;
    public Text bossDebuffTipsUI;


    public  Text seedInputFiled;
    public  UIManager uiManager;
    public static int levelBossHp;
    public static int levelBossDp;
    public  List<GameObject> specialPotions;

    public int levelNum = 0;

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
        EventDispatcher.OnBossDead = ActivateBossDeadUI;
        EventDispatcher.OnPlayerDead = ActivateRestartMenu;
        //DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start () {
        uiManager = new UIManager(playerdisplay, bossdisplay);
        mapGenerator = this.transform.GetComponent<MapGenerator>();
        mapSetting = mapGenerator.mapSetting;
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //mapGenerator.tilemap.RefreshAllTiles();
            //Debug.Log("r");
            DiffultyAdjuster.EvaluatePotionType();
        }
        uiManager.UpdateCharacterUI();
        uiManager.UpdateTextWithGivenString(bossDebuffTipsUI,EventDispatcher.bossDebuffTips);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ActivateRestartMenu();
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
        levelNum = 0;
        seed = new System.Random(mapSetting.seed.GetHashCode());
        //mapGenerator.GenerateBinaryMap();
        startRoom=mapGenerator.InitMapFrameWork(seed,mapSetting);
        InitPlayerInRoom(startRoom);
        levelBossHp = player.GetComponent<PlayerController>().lifePoint;
        levelBossDp = levelBossHp;
        foreach(Action action in actions[typeof(CameraController)])
        {
            action();
        }
    }

    private void GenerateBossAttribute()
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
        levelNum += 1;
    }
    
    public void InitGame(GameObject inputUI)
    {
        mapSetting.seed = uiManager.GetSeedInputFiled(seedInputFiled);
        inputUI.SetActive(false);
        InitializeGame();
        uiManager.ActiveUI();

    }

#if UNITY_EDITOR
    [MenuItem("Tools/重新开始游戏")]
    public static void RestartGame()
    {
        //TileManager.Instance.tilemap.ClearAllTiles();
        TileManager.Instance.tilemap.ClearAllTiles();
        
        //TileManager.Instance.InitData();
        //SceneManager.UnloadSceneAsync("Main");
    }
#endif

    public void Restart(GameObject inputfiled)
    {
        Time.timeScale = 1;
        TileManager.Instance.tilemap.ClearAllTiles();
        foreach (Transform go in mapGenerator.tilemap.gameObject.GetComponentInChildren<Transform>(true))
        {
            Destroy(go.gameObject);
        }
        foreach(Queue<GameObject> gos in ProjectileLauncher.projectiles.Values)
        {
            foreach(GameObject go in gos)
            {
                go.SetActive(false);
            }
        }
        Destroy(player);
        //ProjectileLauncher.projectiles.Clear();
        inputfiled.SetActive(true);
    }

    public void SetUIActive(GameObject self)
    {
        self.SetActive(!self.activeSelf);
    }

    private void ActivateRestartMenu()
    {
        Time.timeScale = (Time.timeScale>0)?0:1;
        restartMenu.SetActive(!restartMenu.activeSelf);
    }

    private void ActivateBossDeadUI()
    {
        ActivateUIs(bossDeadUI);
        EventDispatcher.bossDebuffTips = "BossDebuffs";
    }

    private void ActivateUIs(params GameObject[] uis)
    {
        foreach (GameObject ui in uis)
        {
            ui.SetActive(true);
        }
        foreach (Text text in bossdisplay.GetComponentsInChildren<Text>())
        {
            text.text = "";
            //Debug.Log(text.gameObject.name);

        }
    }

    public void InitNextLevel(GameObject self)
    {
        foreach (Transform go in mapGenerator.tilemap.gameObject.GetComponentInChildren<Transform>(true))
        {
            //Debug.Log(go.name);
            Destroy(go.gameObject);
        }
        Debug.Log(mapSetting.seed.GetHashCode());
        seed = new System.Random(mapSetting.seed.GetHashCode()*(levelNum+1));
        //Debug.Log(seed);
        //mapGenerator.GenerateBinaryMap();
        startRoom = mapGenerator.InitMapFrameWork(seed, mapSetting);
        InitPlayerInRoom(startRoom);
        levelBossHp = player.GetComponent<PlayerController>().lifePoint;
        levelBossDp = levelBossHp;
        foreach (Action action in actions[typeof(CameraController)])
        {
            action();
        }
        uiManager.ActiveUI();
            self.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}

public static class ExtensionClass
{
    public static Vector2Int ConvertToVector2Int(this Vector3Int vector3Int)
    {
        return new Vector2Int(vector3Int.x, vector3Int.y);
    }
}

[Serializable]
public class UIManager
{
    public delegate void UpdateText(params Text[] targets);
    public static Dictionary<string, UpdateText> uiUpdateActions = new Dictionary<string, UpdateText>();
    public GameObject exit;

    private GameObject playerdisplay;
    private Text playerHP;
    private Text playerDP;
    private Text playerLifeNum;

    private GameObject bossdisplay;
    private Text bossHP;
    private Text bossDP;


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
                else if (text.gameObject.name.Contains("dp"))
                    playerDP = text;
                else if (text.gameObject.name.Contains("life"))
                    playerLifeNum = text;
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
                    bossDP = text;
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
                pair.Value(playerHP,playerDP,playerLifeNum);
            }
            else if(pair.Key.Contains("Boss"))
            {
                pair.Value(bossHP, bossDP);
            }
        }
    }

    public static void ActiveUI(params GameObject[] uis)
    {
        foreach(GameObject ui in uis)
        {
            ui.SetActive(true);
        }
    }

    public void ActiveUI()
    {
        playerdisplay.SetActive(true);
        bossdisplay.SetActive(true);

    }

    public string GetSeedInputFiled(Text seedInputField)
    {
        return seedInputField.text;
    }

    
    public void UpdateTextWithGivenString(Text target,string text)
    {
        if (!target.text.Equals(text))
            target.text = text;
    }
}
