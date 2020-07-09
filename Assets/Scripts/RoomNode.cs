using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNode {

    public RoomNode parent;
    public RoomNode leftChild;
    public RoomNode rightChild;

    public Vector2Int bottomLeft;
    public Vector2Int topRight;

    public RoomNode(RoomNode parent, Vector2Int bottomLeft, Vector2Int topRight)
    {
        this.parent = parent;
        this.bottomLeft = bottomLeft;
        this.topRight = topRight;
    }
}
