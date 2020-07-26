using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu()]
public class WallTile : TileBase {

    public Sprite sprite;
    public Object[] allSprites;
    private System.Random seed;
    public string seedCode;
    private string targetPath;

    [SerializeField]
    private TileType type;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        if(allSprites!=null)
        {
            sprite = (Sprite)allSprites[seed.Next(0,allSprites.Length)];
        }
        tileData.sprite = this.sprite;
        base.GetTileData(position, tilemap, ref tileData);
    }


    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
    }

    private void OnEnable()
    {
        if(type.ToString().Contains("Wall"))
        {
            targetPath = "Sprites/Wall/"+type.ToString().Split('_')[1];
            allSprites = Resources.LoadAll(targetPath,typeof(Sprite));
        }
        if (seedCode != null)
            seed = new System.Random(seedCode.GetHashCode());
        else
            seed = new System.Random(Time.time.GetHashCode());
    }
}
