using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu()]
public class ToolTile:TileBase{

    public GameObject gadget;
    private System.Random seed;
    public string seedCode;
    public Object[] tools;
    public Sprite sprite;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = this.sprite;
        base.GetTileData(position, tilemap, ref tileData);
        gadget = (GameObject)tools[seed.Next(0, tools.Length)];
        Debug.Log(gadget.name+" "+position);
    }

    private void OnEnable()
    {
        tools = Resources.LoadAll("Tools") ;
        seed = new System.Random( seedCode.GetHashCode());

    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
    }

    public void SetTool(System.Random seed)
    {
        this.seed = seed;
    }
}
