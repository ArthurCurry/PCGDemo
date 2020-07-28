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
        tileData.gameObject = this.gameobject;
        base.GetTileData(position, tilemap, ref tileData);
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
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
        go.transform.position = position + new Vector3(.5f,.5f);
        return base.StartUp(position, tilemap, go);
    }
}
