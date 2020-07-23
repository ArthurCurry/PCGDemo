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
    public Dictionary<Vector3Int, int> hps=new Dictionary<Vector3Int, int>();

    public int hp = 100;

    public MapSetting mapsetting;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        if(!hps.ContainsKey(position))
            hps.Add(position, this.hp);
        base.GetTileData(position, tilemap, ref tileData);
        tileData.sprite = (hps[position]>0)?spriteA:spriteB ;
        tileData.colliderType = (hps[position] > 0)?Tile.ColliderType.Sprite:Tile.ColliderType.None;
        //tileData.color = color;
        //Debug.Log("dataget");
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
        //Debug.Log("refreshed" + "  " + position + hps[position]);
        base.RefreshTile(position, tilemap);
    }

    private void OnDestroy()
    {
        
    }

    public void UpdateSelf(Vector3Int position,int hp)
    {
        
    }

    
}
