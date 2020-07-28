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
    public Sprite bottomRight;
    public Sprite bottomLeft;
    public Sprite turningLeftT;
    public Sprite turningRightT;
    public Sprite defaultSprite;
    public Sprite sprite;
    public Tile.ColliderType collider;
    public MapSetting mapSetting;

    public string seedCode;
    private System.Random seed;


    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        if (tilemap.GetTile(new Vector3Int(tilemap.size.x - 1, tilemap.size.y - 1, position.z)) != null)
            JudgeSurroundings(position, tilemap);
        tileData.sprite = sprite;
        tileData.colliderType = collider;
        base.GetTileData(position, tilemap, ref tileData);
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {

        //Debug.Log("refreshed" + tilemap.size);
        base.RefreshTile(position, tilemap);
    }

    private void OnEnable()
    {
        if (seedCode != null)
            this.seed = new System.Random(seedCode.GetHashCode());
        else
            this.seed = new System.Random(Time.time.GetHashCode());
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
    private void JudgeSurroundings(Vector3Int position,ITilemap tilemap)
    {
        //bool changeSelf=false;
        sprite = defaultSprite;
        //以下四种是上下左右四方向墙
        if(position.x > 0&&!tilemap.GetTile(position+Vector3Int.left).GetType().Equals(typeof(BackgroundTile)))
        {
            sprite = wallRight[seed.Next(0, wallRight.Count)];
            //changeSelf = true;
        }
        if (position.x < tilemap.size.x-1&&!tilemap.GetTile(position + Vector3Int.right).GetType().Equals(typeof(BackgroundTile)))
        {
            sprite = wallLeft[seed.Next(0, wallRight.Count)];
            //changeSelf = true;
        }
        if (position.y < tilemap.size.y-1&&!tilemap.GetTile(position + Vector3Int.up).GetType().Equals(typeof(BackgroundTile)))
        {
            sprite = wallDown[seed.Next(0, wallRight.Count)];
            //changeSelf = true;
        }
        if (position.y > 0&&!tilemap.GetTile(position + Vector3Int.down).GetType().Equals(typeof(BackgroundTile)))
        {
            sprite = wallUp[seed.Next(0, wallRight.Count)];
            //changeSelf = true;
        }
        //右下，左下，左上，右上的四个角落
        if (position.y < tilemap.size.y-1&&position.x<tilemap.size.x-1 && tilemap.GetTile(position + Vector3Int.right).GetType().Equals(typeof(BackgroundTile))
                && !tilemap.GetTile(position + Vector3Int.right + Vector3Int.up).GetType().Equals(typeof(BackgroundTile))
                && tilemap.GetTile(position + Vector3Int.up).GetType().Equals(typeof(BackgroundTile)))
        {
            sprite = bottomLeft;
            //changeSelf = true;
        }
        if (position.y < tilemap.size.y - 1 && position.x >0&& tilemap.GetTile(position + Vector3Int.left).GetType().Equals(typeof(BackgroundTile))
        && !tilemap.GetTile(position + Vector3Int.left + Vector3Int.up).GetType().Equals(typeof(BackgroundTile))
        && tilemap.GetTile(position + Vector3Int.up).GetType().Equals(typeof(BackgroundTile)))
        {
            sprite = bottomRight;
            //changeSelf = true;
        }
        if (position.y >0 && position.x < tilemap.size.x - 1 && tilemap.GetTile(position + Vector3Int.right).GetType().Equals(typeof(BackgroundTile))
        && !tilemap.GetTile(position + Vector3Int.right + Vector3Int.down).GetType().Equals(typeof(BackgroundTile))
        && tilemap.GetTile(position + Vector3Int.down).GetType().Equals(typeof(BackgroundTile)))
        {
            sprite = wallLeft[seed.Next(0, wallRight.Count)];
            //changeSelf = true;
        }
        if (position.y > 0 && position.x >0 && tilemap.GetTile(position + Vector3Int.left).GetType().Equals(typeof(BackgroundTile))
  && !tilemap.GetTile(position + Vector3Int.left + Vector3Int.down).GetType().Equals(typeof(BackgroundTile))
  && tilemap.GetTile(position + Vector3Int.down).GetType().Equals(typeof(BackgroundTile)))
        {
            sprite = wallRight[seed.Next(0, wallRight.Count)];
            //changeSelf = true;
        }


        if (position.x < tilemap.size.x - 1 && position.x > 0 && position.y > 0 && position.y < tilemap.size.y - 1)
        {
            //两种拐角处的外墙
            if ( !tilemap.GetTile(position + Vector3Int.right).GetType().Equals(typeof(BackgroundTile))
                && !tilemap.GetTile(position + Vector3Int.right + Vector3Int.up).GetType().Equals(typeof(BackgroundTile))
                && !tilemap.GetTile(position + Vector3Int.up).GetType().Equals(typeof(BackgroundTile)))
            {
                sprite = turningLeftT;
                return;
            }
            if (!tilemap.GetTile(position + Vector3Int.left + Vector3Int.up).GetType().Equals(typeof(BackgroundTile))
            && !tilemap.GetTile(position + Vector3Int.left).GetType().Equals(typeof(BackgroundTile))
            && !tilemap.GetTile(position + Vector3Int.up).GetType().Equals(typeof(BackgroundTile)))
            {
                sprite = turningRightT;
                return ;
            }

        }
    }
}
