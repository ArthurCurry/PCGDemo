using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direciton
{
    Horizontal,
    Vertical
}


public class BinarySpacePartitioner {
    /// <summary>
    /// 迭代分割空间的次数
    /// </summary>
    private int iterationTimes;
    private Queue<RoomNode> roomsToSlice;
    public List<RoomNode> allNodes;
    public List<RoomNode> leafNodes;
    public List<PartitionLine> passages;
    private RoomNode rootNode;
    private System.Random seed;
    public BinarySpacePartitioner(int spaceWidth,int spaceHeight,System.Random seed,int iterationTimes)
    {
        this.seed = seed;
        this.rootNode = new RoomNode(null,new Vector2Int(0,0),new Vector2Int(spaceWidth-1,spaceHeight-1));
        roomsToSlice = new Queue<RoomNode>();
        leafNodes=new List<RoomNode>();
        allNodes = new List<RoomNode>();
        passages = new List<PartitionLine>();
        this.iterationTimes = iterationTimes;
    }

    public List<RoomNode> SliceMap(int minRoomWidth,int minRoomHeight,int lineWidth)
    {
        //ClearCollections();
        roomsToSlice.Enqueue(rootNode);
        allNodes.Add(rootNode);
        int i = 0;
        while(i<iterationTimes&&roomsToSlice.Count>0)
        {
            i++;
            RoomNode curNode = roomsToSlice.Dequeue();
            SliceRoom(curNode,allNodes,roomsToSlice,minRoomWidth,minRoomHeight,lineWidth);
        }
        //foreach (RoomNode room in leafNodes)
        //{
        //    Debug.Log(room.bottomLeft + " " + room.topRight);
        //}
        //Debug.Log(i+" "+allNodes.Count+"  "+leafNodes.Count+" "+passages.Count);

        //foreach(PartitionLine line in passages)
        //{
        //    Debug.Log(line.direction);
        //}
        //foreach(RoomNode room in allNodes)
        //{
        //    Debug.Log(room.bottomLeft+" "+room.topRight);
        //}
        return leafNodes;
    }

    public void SliceRoom(RoomNode curRoomToSlice,List<RoomNode> roomsToReturn,Queue<RoomNode> queue,int minRoomWidth,int minRoomHeight,int lineWidth )
    {
        PartitionLine line = GetPartitionLine(curRoomToSlice.bottomLeft,curRoomToSlice.topRight,minRoomWidth,minRoomHeight);
        RoomNode left, right;
        passages.Add(line);
        if (line != null)
        {
            if (line.direction == Direciton.Horizontal)
            {
                left = new RoomNode(curRoomToSlice, new Vector2Int(curRoomToSlice.bottomLeft.x, line.coordinates.y+lineWidth), curRoomToSlice.topRight);
                right = new RoomNode(curRoomToSlice, curRoomToSlice.bottomLeft, new Vector2Int(curRoomToSlice.topRight.x, line.coordinates.y- lineWidth));
            }
            else /*if (line.direction == Direciton.Vertical)*/
            {
                left = new RoomNode(curRoomToSlice, curRoomToSlice.bottomLeft, new Vector2Int(line.coordinates.x- lineWidth, curRoomToSlice.topRight.y));
                right = new RoomNode(curRoomToSlice, new Vector2Int(line.coordinates.x+ lineWidth, curRoomToSlice.bottomLeft.y), curRoomToSlice.topRight);
            }

            AddRoomsToCollections(left, queue, roomsToReturn);
            AddRoomsToCollections(right, queue, roomsToReturn);

        }
        else
        {
            leafNodes.Add(curRoomToSlice);
            return;
        }

    }

    private void AddRoomsToCollections(RoomNode room,Queue<RoomNode> queue,List<RoomNode> list)
    {
        queue.Enqueue(room);
        list.Add(room);
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
        bool ifDivideByWidth = (topRight.x - bottomLeft.x)+1 >(2 * minRoomWidth);
        bool ifDivideByHeight = (topRight.y - bottomLeft.y)+1 > (2 * minRoomHeight);
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

    private void ClearCollections()
    {
        roomsToSlice.Clear();
        leafNodes.Clear();
        allNodes.Clear();
        passages.Clear();
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
