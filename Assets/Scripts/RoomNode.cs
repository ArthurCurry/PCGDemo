using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNode {

    public RoomNode parent;
    public RoomNode leftChild;
    public RoomNode rightChild;

    public Vector2Int bottomLeft;
    public Vector2Int topRight;
    public Vector2Int bottomRight;
    public Vector2Int topLeft;



    public int Width
    {
        get
        {
            return (topRight.x - bottomLeft.x+1);
        }
    }

    public int Height
    {
        get
        {
            return (topRight.y-bottomLeft.y+1);
        }
    }
    public RoomNode(RoomNode parent, Vector2Int bottomLeft, Vector2Int topRight)
    {
        this.parent = parent;
        this.bottomLeft = bottomLeft;
        this.topRight = topRight;
        bottomRight = new Vector2Int(topRight.x,bottomLeft.y);
        topLeft = new Vector2Int( bottomLeft.x, topRight.y);

    }

    public void AddChild()
    {

    }
}
