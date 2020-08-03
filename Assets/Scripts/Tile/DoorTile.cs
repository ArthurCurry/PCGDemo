using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu()]
public class DoorTile : TileBase {

    public Sprite sprite;
    public GameObject gameobject;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = this.sprite;
        if (JudgeCorridorNums(position, tilemap)<2)
        {
            tileData.gameObject = this.gameobject;
            //SetNeighbourCordsOfDoor(this.gameobject.GetComponent<Door>(),position);
        }
        else
            tileData.gameObject = null;
        base.GetTileData(position, tilemap, ref tileData);
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        RefreshNeighbourTile(position,tilemap);
        base.RefreshTile(position, tilemap);
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        //if(!tilemap.GetTile(position+Vector3Int.down).GetType().Equals(typeof(BackgroundTile)))
        //{
        //    go.GetComponent<BoxCollider2D>().size= new Vector2(1,3);
        //}
        //else if(!tilemap.GetTile(position + Vector3Int.left).GetType().Equals(typeof(BackgroundTile)))
        //{
        //    go.GetComponent<BoxCollider2D>().size = new Vector2(3, 1);
        //}
        //GameObject.Instantiate(gameobject, position+new Vector3(.5f,.5f,position.z),go.transform.rotation);
        if(go!=null)
            go.transform.position = position + new Vector3(.5f,.5f);
        return base.StartUp(position, tilemap, go);
    }

    private int JudgeCorridorNums(Vector3Int position,ITilemap tilemap)
    {
        int num = 0;
        if (tilemap.GetTile(position + Vector3Int.left) is DoorTile)
        {
            num += 1;
        }
        if (tilemap.GetTile(position + Vector3Int.right) is DoorTile)
        {
            num += 1;
        }
        if (tilemap.GetTile(position + Vector3Int.up) is DoorTile)
        {
            num += 1;
        }
        if (tilemap.GetTile(position + Vector3Int.down) is DoorTile)
        {
            num += 1;
        }
        return num;
    }

    private void RefreshNeighbourTile(Vector3Int position, ITilemap tileMap)
    {
        if (tileMap.GetTile(position + Vector3Int.left + Vector3Int.up) is BackgroundTile)
        {
            tileMap.RefreshTile(position + Vector3Int.left + Vector3Int.up);
        }
        if (tileMap.GetTile(position + Vector3Int.left + Vector3Int.down) is BackgroundTile)
        {
            tileMap.RefreshTile(position + Vector3Int.left + Vector3Int.down);
        }
        if (tileMap.GetTile(position + Vector3Int.left) is BackgroundTile)
        {
            tileMap.RefreshTile(position + Vector3Int.left);
        }
        if (tileMap.GetTile(position + Vector3Int.right + Vector3Int.up) is BackgroundTile)
        {
            tileMap.RefreshTile(position + Vector3Int.right + Vector3Int.up);
        }
        if (tileMap.GetTile(position + Vector3Int.right + Vector3Int.down) is BackgroundTile)
        {
            tileMap.RefreshTile(position + Vector3Int.right + Vector3Int.down);
        }
        if (tileMap.GetTile(position + Vector3Int.right) is BackgroundTile)
        {
            tileMap.RefreshTile(position + Vector3Int.right);
        }
        if (tileMap.GetTile(position + Vector3Int.up) is BackgroundTile)
        {
            tileMap.RefreshTile(position + Vector3Int.up);
        }
        if (tileMap.GetTile(position + Vector3Int.down) is BackgroundTile)
        {
            tileMap.RefreshTile(position + Vector3Int.down);
        }
    }
}
