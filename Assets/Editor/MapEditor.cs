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

       

        using (var check =  new EditorGUI.ChangeCheckScope())
        {
            Editor editor = CreateEditor(mapGenerator.mapSetting);
            editor.OnInspectorGUI();
            if (check.changed)
            {
                mapGenerator.GenerateNoiseMap();
            }
        }


    }

    GenerateMap GenerateMapType(int type,GenerateMap action)
    {
        switch(type)
        {
            case (int)MapType.Random:
                action = mapGenerator.GenerateRandomMap;
                break;
            case (int)MapType.PerlinNoise:
                action = mapGenerator.GenerateNoiseMap;
                break;
            case (int)MapType.Binary:
                action = mapGenerator.GenerateBinaryMap;
                break;
            default:
                action = null;
                break;
        }
        return action;
    }

    
}
