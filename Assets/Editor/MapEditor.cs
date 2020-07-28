using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.Tilemaps;


[CustomEditor(typeof(MapGenerator))]
public class MapEditor:Editor{

    MapGenerator mapGenerator;
    //public MapSetting mapSetting;

    public int mapWidth;
    public int mapHeight;
    private delegate Map GenerateMap();
    private MapType generationType;
    private int percentage;
    GenerateMap generateMethod;
    Vector2Int pos = Vector2Int.zero;

    private void OnEnable()
    {
        mapGenerator = (MapGenerator)target;
        
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //EditorGUILayout.LabelField("宽度");
        //mapWidth = EditorGUILayout.IntSlider(mapWidth, 10, 200);

        //EditorGUILayout.LabelField("长度");
        //mapHeight = EditorGUILayout.IntSlider(mapHeight, 10, 200);
        mapGenerator.seed = mapGenerator.mapSetting.seed;

        

        using (var check =  new EditorGUI.ChangeCheckScope())
        {
            generationType = (MapType)EditorGUILayout.EnumPopup("生成地图类型", generationType);
            Editor editor = CreateEditor(mapGenerator.mapSetting);
            editor.OnInspectorGUI();
            if (check.changed||GUILayout.Button("预览地图"))
            {
                generateMethod = new GenerateMap(GenerateMapType(generationType));
                generateMethod();
                //Debug.Log(generationType);
            }
        }
        if(GUILayout.Button("铺瓦片"))
        {
            mapGenerator.GenerateBinaryMap();
        }
        if(GUILayout.Button("获取瓦片资源"))
        {
            TileManager.Instance.InitData();
            RoomManager.Instace.InitData();
        }
        pos=EditorGUILayout.Vector2IntField("坐标",pos);
        if(GUILayout.Button("击打障碍物 hp 20/次"))
        {
            Debug.Log("button down");
            Vector3Int targetPos = new Vector3Int(pos.x, pos.y, 0);
            ObstacleTile tile = (ObstacleTile)mapGenerator.tilemap.GetTile(targetPos);
            tile.hps[targetPos] -= 20;
            //mapGenerator.tilemap.RefreshAllTiles();
            mapGenerator.tilemap.RefreshTile(targetPos);
        }
        if(GUILayout.Button("刷新地图"))
        {
            mapGenerator.tilemap.RefreshAllTiles();
        }
    }

    GenerateMap GenerateMapType(MapType type)
    {
        GenerateMap action;
        switch(type)
        {
            case MapType.Random:
                action = mapGenerator.GenerateRandomMap;
                break;
            case MapType.PerlinNoise:
                action = mapGenerator.GenerateNoiseMap;
                break;
            case MapType.Binary:
                action = mapGenerator.GenerateBinaryMap;
                break;
            default:
                action = null;
                break;
        }
        return action;
    }

    
}
