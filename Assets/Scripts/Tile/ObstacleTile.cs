using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu()]
public class ObstacleTile :TileBase {

    public Sprite spriteA;
    public Sprite spriteB;
    public Tile.ColliderType type;
    public Color color;

    public int hp = 100;

    public MapSetting mapsetting;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        tileData.sprite = spriteA ;
        tileData.colliderType = type;
        //tileData.color = color;

    }


    private void OnEnable()
    {
        //Debug.Log("enabled");

    }

    private void OnDisable()
    {
        //Debug.Log("disbled");
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
        Debug.Log("refreshed" + " " + position);
    }
}
