using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MapGenerator))]
public class MapEditor:Editor{

    MapGenerator mapGenerator;

    public int mapWidth;
    public int mapHeight;

    private void OnEnable()
    {
        mapGenerator = (MapGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("宽度");
        mapWidth = EditorGUILayout.IntSlider(mapWidth, 10, 200);

        EditorGUILayout.LabelField("长度");
        mapHeight = EditorGUILayout.IntSlider(mapHeight, 10, 200);

        if (GUILayout.Button("生成地图"))
        {
            mapGenerator.GenerateNoiseMap(mapWidth,mapHeight);
        }
    }


}
