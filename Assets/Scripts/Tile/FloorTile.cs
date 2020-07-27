﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu()]
public class FloorTile : TileBase {

    public Object[] allFloors;
    public Sprite sprite;
    public string spritePath;
    public string seedCode;
    private System.Random seed;
    private string targetPath;
    


    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        sprite = (Sprite)allFloors[seed.Next(0, allFloors.Length)];
        //Debug.Log(sprite.name);
        tileData.sprite = this.sprite;
        base.GetTileData(position, tilemap, ref tileData);
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
    }

    private void OnEnable()
    {
        if (seedCode != null)
            seed = new System.Random(seedCode.GetHashCode());
        else
            seed = new System.Random(Time.time.GetHashCode());
        targetPath = "Sprites/" + spritePath;
        allFloors = Resources.LoadAll(targetPath,typeof(Sprite));
        //foreach(Object ob in allFloors)
        //{
        //    Debug.Log(ob.name+" "+ob.GetType());
        //}

    }

    //private List<Sprite> ConvertT2DToSprite(Object[] t2ds)
    //{

    //    foreach(Object t2d in t2ds)
    //    {
    //        Texture2D temp = (Texture2D)t2d;
    //        Sprite sp = Sprite.Create(t2d,);
    //    }
    //}
}