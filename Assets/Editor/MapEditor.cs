using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


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

        generationType = (MapType)EditorGUILayout.EnumPopup("生成地图类型", generationType);
        using (var check =  new EditorGUI.ChangeCheckScope())
        {
            Editor editor = CreateEditor(mapGenerator.mapSetting);
            editor.OnInspectorGUI();
            if (check.changed)
            {
                generateMethod = new GenerateMap(GenerateMapType(generationType));
                generateMethod();
                //Debug.Log(generationType);
            }
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
