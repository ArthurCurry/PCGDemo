using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu()]
public class ObstacleTile :TileBase {

    public Sprite spriteA;
    public Sprite spriteB;
    public Tile.ColliderType type;

    private int hp = 100;

    public MapSetting mapsetting;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = mapsetting.trapActivated ? spriteA : spriteB;
        tileData.colliderType = type;
        base.GetTileData(position, tilemap, ref tileData);
    }


    private void OnEnable()
    {
        Debug.Log("enabled");

    }

    private void OnDisable()
    {
        //Debug.Log("disbled");
    }

    
}
