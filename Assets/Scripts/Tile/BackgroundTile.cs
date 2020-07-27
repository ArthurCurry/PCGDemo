using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu()]
public class BackgroundTile :TileBase  {

    public List<Sprite> wallRight;
    public List<Sprite> wallLeft;
    public List<Sprite> wallUp;
    public List<Sprite> wallDown;
    public Sprite defaultSprite;
    public Sprite sprite;

    public string seedCode;
    private System.Random seed;


    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {

        base.GetTileData(position, tilemap, ref tileData);
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
    }

    private void OnEnable()
    {
        this.seed = new System.Random(seedCode.GetHashCode());
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        return base.StartUp(position, tilemap, go);
    }

    /// <summary>
    /// 根据周围环境改变自身显示精灵
    /// </summary>
    /// <param name="position"></param>
    /// <param name="tilemap"></param>
    /// <param name="tileData"></param>
    private void JudgeSurroundings(Vector3Int position,ITilemap tilemap, TileData tileData)
    {

    }
}
