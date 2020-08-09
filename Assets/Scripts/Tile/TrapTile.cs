using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu()]
public class TrapTile :TileBase  {

    public List<GameObject> traps;
    public GameObject trap;
    private System.Random seed;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        InitTileData();
        tileData.gameObject = trap;
        base.GetTileData(position, tilemap, ref tileData);
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (go != null)
            go.transform.position += new Vector3(.5f, .5f, 0);
        return base.StartUp(position, tilemap, go);
    }

    private void InitTileData()
    {
        seed = new System.Random(GameManager.mapSetting.seed.GetHashCode());
        trap = traps[seed.Next(0, traps.Count)];
    }
    
}
