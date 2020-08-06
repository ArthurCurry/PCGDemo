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
    public Sprite topRight;
    public Sprite topLeft;
    public Sprite turningLeftT;
    public Sprite turningRightT;
    public Sprite defaultSprite;
    public Sprite sprite;
    public Tile.ColliderType collider;
    public Color color;
    [Range(0,100)]
    public int decorationPercentage;

    public List<GameObject> frontDecorations;
    public List<GameObject> sideDecorations;
    public List<GameObject> horizontalTraps;
    public List<GameObject> verticalTraps;
    public MapSetting mapsetting;


    public string seedCode;
    private System.Random seed;
    private GameObject gameObject;
    private Vector3 offset;
    private Vector3 scale;


    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        //Debug.Log(Time.time + "get");

        if (tilemap.GetTile(new Vector3Int(tilemap.size.x - 1, tilemap.size.y - 1, position.z)) != null)
            JudgeSurroundings(position, tilemap);
        tileData.sprite = sprite;
        //tileData.sprite.col = this.color;
        tileData.colliderType = collider;
        if (gameObject != null)
        {
            //gameObject.transform.position += offset;
            tileData.gameObject = gameObject;
        }
        base.GetTileData(position, tilemap, ref tileData);
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {

        //Debug.Log("refreshed" + tilemap.size);
        base.RefreshTile(position, tilemap);
    }

    private void OnEnable()
    {
        this.seed = new System.Random(mapsetting.seed.GetHashCode());

    }


    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        //Debug.Log(Time.time + "start");
        if (go != null)
        {
            JudgeGameobject(go,position);


        }
        //tilemap.RefreshTile(position);
        return base.StartUp(position,tilemap,go);
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
        if (position.x < tilemap.size.x - 1 && position.x > 0 && position.y > 0 && position.y < tilemap.size.y - 1)
        {
            //两种拐角处的外墙
            if (!tilemap.GetTile(position + Vector3Int.right).GetType().Equals(typeof(BackgroundTile))
                //&& !tilemap.GetTile(position + Vector3Int.right + Vector3Int.up).GetType().Equals(typeof(BackgroundTile))
                && !tilemap.GetTile(position + Vector3Int.up).GetType().Equals(typeof(BackgroundTile)))
            {
                sprite = turningLeftT;
                return;
            }
            if (/*!tilemap.GetTile(position + Vector3Int.left + Vector3Int.up).GetType().Equals(typeof(BackgroundTile))*/
            /*&& */!tilemap.GetTile(position + Vector3Int.left).GetType().Equals(typeof(BackgroundTile))
            && !tilemap.GetTile(position + Vector3Int.up).GetType().Equals(typeof(BackgroundTile)))
            {
                sprite = turningRightT;
                return;
            }

        }
        int percentage = seed.Next(0,100);
        gameObject = null;
        offset = Vector3.zero;
        scale = new Vector3(1, 1, 1);
        //以下四种是上下左右四方向墙
        if (position.x > 0&&!tilemap.GetTile(position+Vector3Int.left).GetType().Equals(typeof(BackgroundTile)))
        {
            sprite = wallRight[seed.Next(0, wallRight.Count)];
            if (percentage <= mapsetting.wallTrapPercentage)
            {
                gameObject = horizontalTraps[seed.Next(horizontalTraps.Count)];
                //scale = new Vector3(-1, 1, 1);
                //offset = new Vector3(0, 0.5f, 0);
            }
            else if(percentage<=decorationPercentage)
            {
                gameObject = sideDecorations[seed.Next(0, sideDecorations.Count)];
                //offset = Vector3.left;

            }
            if (gameObject != null)
            {
                //gameObject.transform.localScale = new Vector3(-gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
            }

            //changeSelf = true;
        }
        if (position.x < tilemap.size.x-1&&!tilemap.GetTile(position + Vector3Int.right).GetType().Equals(typeof(BackgroundTile)))
        {
            sprite = wallLeft[seed.Next(0, wallRight.Count)];
            if (percentage <= mapsetting.wallTrapPercentage)
            {
                gameObject = horizontalTraps[seed.Next(horizontalTraps.Count)];
                //offset = Vector3.right * 0.5f;
            }
            else if (percentage <= decorationPercentage)
            {
                gameObject = sideDecorations[seed.Next(0, sideDecorations.Count)];
                //offset = Vector3.right * 0.5f;

            }
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
            if (percentage <= mapsetting.wallTrapPercentage)
            {
                gameObject = verticalTraps[seed.Next(horizontalTraps.Count)];
                //offset = new Vector3(0.5f, 0, 0);
            }
            else if(percentage<=decorationPercentage)
            {
                gameObject = frontDecorations[seed.Next(0, frontDecorations.Count)];
                //offset = new Vector3(0.5f, 0.5f, 0);
                //temp.transform.position += Vector3.down * 0.5f;
            }

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
            sprite = topLeft;
            //changeSelf = true;
        }
        if (position.y > 0 && position.x >0 && tilemap.GetTile(position + Vector3Int.left).GetType().Equals(typeof(BackgroundTile))
  && !tilemap.GetTile(position + Vector3Int.left + Vector3Int.down).GetType().Equals(typeof(BackgroundTile))
  && tilemap.GetTile(position + Vector3Int.down).GetType().Equals(typeof(BackgroundTile)))
        {
            sprite = topRight;
            //changeSelf = true;
        }

        
        
    }

    private void JudgeGameobject(GameObject go,Vector3Int position)
    {
        //Debug.Log(gameObject == null);
        if (go.name.Contains("flamethrower"))
        {
            if (go.name.Contains("vertical"))
                go.transform.position += new Vector3(.5f, 0, 0);
            else
                go.transform.position += new Vector3(2,.5f,0);
        }
        if (go.name.Contains("flag"))
        {
            go.transform.position += new Vector3(0.5f, 0.5f, 0);
        }
    }
}
