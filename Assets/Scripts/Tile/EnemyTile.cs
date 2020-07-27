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
        enemy = (GameObject)enemies[seed.Next(0, enemies.Length)];
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
        foreach (Object ob in enemies)
        {
            //Debug.Log(ob.name);
        }
    }
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (go != null)
            go.transform.position = position + new Vector3(.5f, .5f);
        return base.StartUp(position, tilemap, go);
    }

    /// <summary>
    /// 判断周围和自己相同类型瓦片的个数，四邻域
    /// </summary>
    /// <param name="tilemap"></param>
    /// <param name="position"></param>
    private void JudgeSorroundings(ITilemap tilemap, Vector3Int position)
    {
        TileBase right = tilemap.GetTile(position + Vector3Int.right);
        TileBase left = tilemap.GetTile(position + Vector3Int.left);
        TileBase up = tilemap.GetTile(position + Vector3Int.up);
        TileBase down = tilemap.GetTile(position + Vector3Int.down);
        EnemyTile self = (EnemyTile)tilemap.GetTile(position);

        if (right is FloorTile && right.name.Contains(TileType.Floor.ToString()))
        {
            FloorTile temp = right as FloorTile;
            self.sprite = temp.sprite;
        }
        if (left is FloorTile && left.name.Contains(TileType.Floor.ToString()))
        {
            FloorTile temp = left as FloorTile;
            self.sprite = temp.sprite;
        }
        if (up is FloorTile && up.name.Contains(TileType.Floor.ToString()))
        {
            FloorTile temp = up as FloorTile;
            self.sprite = temp.sprite;
        }
        if (down is FloorTile && down.name.Contains(TileType.Floor.ToString()))
        {
            FloorTile temp = down as FloorTile;
            self.sprite = temp.sprite;
        }
    }

}
