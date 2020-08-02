﻿using System.Collections;
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

    public List<Corridor> corridors=new List<Corridor>();

    public RoomType type;

    public int nodeIndex;

    public bool isTiled;

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

    public int Size
    {
        get
        {
            return (Width*Height);
        }
    }
    
    public RoomNode(RoomNode parent, Vector2Int bottomLeft, Vector2Int topRight,int nodeIndex=0)
    {
        this.parent = parent;
        this.bottomLeft = bottomLeft;
        this.topRight = topRight;
        bottomRight = new Vector2Int(topRight.x,bottomLeft.y);
        topLeft = new Vector2Int( bottomLeft.x, topRight.y);
        this.nodeIndex = nodeIndex;
        this.isTiled = false;
    }

    public bool ContainsCoordinate(List<Vector2Int> coordinates)
    {
        foreach (Vector2Int coordinate in coordinates)
        {
            if (coordinate.x <= topRight.x && coordinate.x >= bottomLeft.x && coordinate.y <= topRight.y && coordinate
                .y >= bottomLeft.y)
            {
                return true;
            }
        }
        return false;
    }


}
