using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private static Dictionary<Type,List<Action>> actions=new Dictionary<Type, List<Action>>();
    private MapGenerator mapGenerator;

    public MapGenerator generator
    {
        get
        {
            return mapGenerator;
        }
    }


	// Use this for initialization
	void Start () {
        InitializeGame();
        foreach(List<Action> action in actions.Values)
        {
            Debug.Log( action.Count);
        }
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
        mapGenerator.GenerateBinaryMap();
        foreach(Action action in actions[typeof(CameraController)])
        {
            action();
        }
    }

}
