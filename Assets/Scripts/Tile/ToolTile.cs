using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu()]
public class ToolTile:TileBase{

    public GameObject gadget;
    private System.Random seed;
    public string seedCode;
    public Dictionary<Vector3Int, GameObject> tools;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {

        base.GetTileData(position, tilemap, ref tileData);
    }

    private void OnEnable()
    {
        
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
    }


}
