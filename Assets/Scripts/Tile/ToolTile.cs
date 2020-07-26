using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu()]
public class ToolTile:TileBase{

    public GameObject gadget;
    private System.Random seed;
    public string seedCode;
    public Object[] tools;
    public Sprite sprite;
    public Dictionary<Vector3Int, GameObject> gadgets = new Dictionary<Vector3Int, GameObject>();

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        JudgeSorroundings(tilemap,position);
        tileData.sprite = this.sprite;
        gadget = (GameObject)tools[seed.Next(0, tools.Length)];
        tileData.gameObject = gadget;
        base.GetTileData(position, tilemap, ref tileData);
        //if(tilemap.GetTile(position + Vector3Int.right)!=null)
        //    Debug.Log(tilemap.GetTile(position + Vector3Int.right).name);
        //if(!gadgets.ContainsKey(position))
        //    gadgets.Add(position,tileData.gameObject);
        //Debug.Log(gadget.name+" "+position);
        //Debug.Log("getted");
    }

    private void OnEnable()
    {
        tools = Resources.LoadAll("Tools") ;
        seed = new System.Random( seedCode.GetHashCode());

    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        //DestroyImmediate(gadgets[position]);
        //Debug.Log("refreshed");
        base.RefreshTile(position, tilemap);
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (go != null)
            go.transform.position = position + new Vector3(.5f, .5f);
        return base.StartUp(position, tilemap, go);
    }

    private void OnDisable()
    {
        //Destroy(gadget);
    }

    

    public void SetTool(System.Random seed)
    {
        this.seed = seed;
    }

    /// <summary>
    /// 根据周边的floor地块种类自动变更自身精灵
    /// </summary>
    /// <param name="tilemap"></param>
    /// <param name="position"></param>
    private void JudgeSorroundings(ITilemap tilemap,Vector3Int position)
    {
        TileBase right = tilemap.GetTile(position + Vector3Int.right);
        TileBase left = tilemap.GetTile(position + Vector3Int.left);
        TileBase up = tilemap.GetTile(position + Vector3Int.up);
        TileBase down = tilemap.GetTile(position + Vector3Int.down);
        ToolTile self = (ToolTile)tilemap.GetTile(position);

        if (right is FloorTile && right.name.Contains(TileType.Floor.ToString()))
        {
            FloorTile temp = right as FloorTile;
            self.sprite = temp.sprite;
            return;
        }
        else if (left is FloorTile && left.name.Contains(TileType.Floor.ToString()))
        {
            FloorTile temp = left as FloorTile;
            self.sprite = temp.sprite;
            return;

        }
        else if (up is FloorTile && up.name.Contains(TileType.Floor.ToString()))
        {
            FloorTile temp = up as FloorTile;
            self.sprite = temp.sprite;
            return;

        }
        else if (down is FloorTile && down.name.Contains(TileType.Floor.ToString()))
        {
            FloorTile temp = down as FloorTile;
            self.sprite = temp.sprite;
            return;
        }

    }
}
