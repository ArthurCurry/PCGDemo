using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direciton
{
    Horizontal,
    Vertical
}


public class BinarSpacePartitioner {
    /// <summary>
    /// 迭代分割空间的次数
    /// </summary>
    public int irerationTimes;
    private Queue<RoomNode> generatedQueuq;
    public List<RoomNode> allRooms;
    public List<RoomNode> leafNodes;
    private RoomNode rootNode;
    private System.Random seed;
    public BinarSpacePartitioner(int spaceWidth,int spaceHeight,System.Random seed)
    {
        this.seed = seed;
        this.rootNode = new RoomNode(null,new Vector2Int(0,0),new Vector2Int(spaceWidth,spaceHeight));
    }


    public void SliceRoomN(RoomNode curRoomToSlice,List<RoomNode> roomsToReturn,int minRoomWidth,int minRoomHeight )
    {
        PartitionLine line = GetPartitionLine(curRoomToSlice.bottomLeft,curRoomToSlice.topRight,minRoomWidth,minRoomHeight);

    }

    private void AddRoomsToCollections(RoomNode room)
    {

    }
    /// <summary>
    /// 随机确定分割的边界位置
    /// </summary>
    /// <param name="bottomLeft"></param>
    /// <param name="topRight"></param>
    /// <param name="minRoomWidth"></param>
    /// <param name="minRoomHeight"></param>
    /// <returns></returns>
    private PartitionLine GetPartitionLine(Vector2Int bottomLeft,Vector2Int topRight, int minRoomWidth, int minRoomHeight)
    {
        Direciton direction;
        bool ifDivideByWidth = (topRight.x - bottomLeft.x) >= (2 * minRoomWidth);
        bool ifDivideByHeight = (topRight.y - bottomLeft.y) >= (2 * minRoomHeight);
        if (ifDivideByHeight && ifDivideByWidth)
        {
            direction = (Direciton)seed.Next(0, 2);
        }
        else if (ifDivideByWidth)
        {
            direction = Direciton.Vertical;

        }
        else if (ifDivideByHeight)
        {
            direction = Direciton.Horizontal;
        }
        else
            return null;

        return new PartitionLine(direction,GetPartitionCoordinates(direction, bottomLeft,topRight,minRoomWidth,minRoomHeight));
    }

    private Vector2Int GetPartitionCoordinates(Direciton direction, Vector2Int bottomLeft, Vector2Int topRight, int minRoomWidth, int minRoomHeight)
    {
        Vector2Int coordinates = Vector2Int.zero;
        if(direction==Direciton.Horizontal)
        {
            coordinates = new Vector2Int(0,seed.Next(bottomLeft.y+minRoomHeight,topRight.y-minRoomHeight));
        }
        if(direction==Direciton.Vertical)
        {
            coordinates = new Vector2Int(seed.Next(bottomLeft.x+minRoomWidth,topRight.x-minRoomWidth),0);
        }
        return coordinates;
    }
}

public class PartitionLine
{
    public Direciton direction;
    public Vector2Int coordinates;

    public PartitionLine(Direciton direction, Vector2Int coordinates)
    {
        this.direction = direction;
        this.coordinates = coordinates;
    }


}
