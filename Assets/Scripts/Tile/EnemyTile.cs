using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu()]
public class EnemyTile:TileBase {

    public Sprite sprite;
    public GameObject enemy;
    private System.Random seed;
    public string seedCode;
    private Object[] enemies;



    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        JudgeSorroundings(tilemap, position);
        enemy= (GameObject)enemies[seed.Next(0, enemies.Length)];
        tileData.gameObject = enemy;
        tileData.sprite = this.sprite;
        base.GetTileData(position, tilemap, ref tileData);
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
    }

    private void OnEnable()
    {
        enemies = Resources.LoadAll("Prefabs/Enemy");
        seed = new System.Random(seedCode.GetHashCode());
        Debug.Log(enemies.Length);
    }

    private void JudgeSorroundings(ITilemap tilemap, Vector3Int position)
    {
        TileBase right = tilemap.GetTile(position + Vector3Int.right);
        TileBase left = tilemap.GetTile(position + Vector3Int.left);
        TileBase up = tilemap.GetTile(position + Vector3Int.up);
        TileBase down = tilemap.GetTile(position + Vector3Int.down);
        EnemyTile self = (EnemyTile)tilemap.GetTile(position);

        if (right is Tile && right.name.Contains(TileType.Floor.ToString()))
        {
            Tile temp = right as Tile;
            self.sprite = temp.sprite;
        }
        if (left is Tile && left.name.Contains(TileType.Floor.ToString()))
        {
            Tile temp = left as Tile;
            self.sprite = temp.sprite;
        }
        if (up is Tile && up.name.Contains(TileType.Floor.ToString()))
        {
            Tile temp = up as Tile;
            self.sprite = temp.sprite;
        }
        if (down is Tile && down.name.Contains(TileType.Floor.ToString()))
        {
            Tile temp = down as Tile;
            self.sprite = temp.sprite;
        }
    }
}
