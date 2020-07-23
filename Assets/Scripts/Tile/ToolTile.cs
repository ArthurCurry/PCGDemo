using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu()]
public class ToolTile:TileBase{

    public GameObject gadget;
    private System.Random seed;
    public string seedCode;
    public GameObject[] tools;
    public Sprite sprite;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = this.sprite;
        base.GetTileData(position, tilemap, ref tileData);
    }

    private void OnEnable()
    {
        tools = Resources.LoadAll("Prefabs/Tools") as GameObject[];
        Debug.Log(tools.Length);
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
    }


}
