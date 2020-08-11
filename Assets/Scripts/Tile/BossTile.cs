using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu()]
public class BossTile : TileBase {

    public List<GameObject> allBoss;
    public MapSetting mapsetting;
    public Sprite sprite;
    private GameObject currentBoss;
    private System.Random seed;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        this.seed = new System.Random(mapsetting.seed.GetHashCode());

        currentBoss = allBoss[seed.Next(0, allBoss.Count)];
        tileData.gameObject = currentBoss;
        tileData.sprite = this.sprite;
        base.GetTileData(position, tilemap, ref tileData);
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if(go!=null)
        {
            go.transform.position += new Vector3(.5f, .5f, 0);
        }
        return base.StartUp(position, tilemap, go);
    }
}
