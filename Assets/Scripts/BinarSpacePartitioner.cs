using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Direction
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
    public List<PartitionLine> borders;
    public List<Corridor> corridors;
    public List<Vector2Int> gates;
    private RoomNode rootNode;
    private System.Random seed;
    public BinarySpacePartitioner(int spaceWidth,int spaceHeight,System.Random seed,int iterationTimes)
    {
        this.seed = seed;
        this.rootNode = new RoomNode(null,new Vector2Int(1,1),new Vector2Int(spaceWidth-1,spaceHeight-1));
        roomsToSlice = new Queue<RoomNode>();
        leafNodes=new List<RoomNode>();
        allNodes = new List<RoomNode>();
        borders = new List<PartitionLine>();
        corridors = new List<Corridor>();
        gates=new List<Vector2Int>();
        this.iterationTimes = iterationTimes;
    }

    public List<RoomNode> SliceMap(int minRoomWidth,int minRoomHeight,int lineWidth,int corridorWidth)
    {
        //ClearCollections();
        //roomsToSlice.Enqueue(rootNode);
        //allNodes.Add(rootNode);
        AddRoomToCollections(rootNode,roomsToSlice,allNodes);
        int i = 0;
        while(i<iterationTimes&&roomsToSlice.Count>0)
        {
            i++;
            RoomNode curNode = roomsToSlice.Dequeue();
            SliceRoom(i,curNode,allNodes,roomsToSlice,minRoomWidth,minRoomHeight,lineWidth,corridorWidth);
        }
        //foreach (RoomNode room in leafNodes)
        //{
        //    Debug.Log(room.bottomLeft + " " + room.topRight+"  "+room.nodeIndex);
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

    public void SliceRoom(int curIterationTimes,RoomNode curRoomToSlice,List<RoomNode> roomsToReturn,Queue<RoomNode> queue,int minRoomWidth,int minRoomHeight,int lineWidth,int corridorWidth )
    {

        PartitionLine line = GetPartitionLine(curRoomToSlice,minRoomWidth,minRoomHeight,lineWidth);
        RoomNode left, right;
        borders.Add(line);
        if (line != null)
        {
            if (line.direction == Direction.Horizontal)
            {
                left = new RoomNode(curRoomToSlice, new Vector2Int(curRoomToSlice.bottomLeft.x, line.coordinates.y+lineWidth), curRoomToSlice.topRight);
                right = new RoomNode(curRoomToSlice, curRoomToSlice.bottomLeft, new Vector2Int(curRoomToSlice.topRight.x, line.coordinates.y- lineWidth));
            }
            else /*if (line.direction == Direciton.Vertical)*/
            {
                left = new RoomNode(curRoomToSlice, curRoomToSlice.bottomLeft, new Vector2Int(line.coordinates.x- lineWidth, curRoomToSlice.topRight.y)/*,curRoomToSlice.nodeIndex + 1*/);
                right = new RoomNode(curRoomToSlice, new Vector2Int(line.coordinates.x+ lineWidth, curRoomToSlice.bottomLeft.y), curRoomToSlice.topRight/*, curRoomToSlice.nodeIndex + 2*/);
            }
            curRoomToSlice.leftChild = left;
            curRoomToSlice.rightChild = right;
            AddRoomToCollections(left, queue, roomsToReturn);
            AddRoomToCollections(right, queue, roomsToReturn);
            ConnectNeighborRooms(left,right,line,corridorWidth,minRoomWidth,minRoomHeight,lineWidth);
        }
        else
        {
            leafNodes.Add(curRoomToSlice);
        }
        if (curIterationTimes==iterationTimes-1)
        {
            leafNodes.AddRange(roomsToSlice);
        }
    }

    private void AddRoomToCollections(RoomNode room,Queue<RoomNode> queue,List<RoomNode> list)
    {
        queue.Enqueue(room);
        list.Add(room);
        room.nodeIndex = list.IndexOf(room);
    }
    /// <summary>
    /// 随机确定分割的边界位置
    /// </summary>
    /// <param name="bottomLeft"></param>
    /// <param name="topRight"></param>
    /// <param name="minRoomWidth"></param>
    /// <param name="minRoomHeight"></param>
    /// <returns></returns>
    private PartitionLine GetPartitionLine(RoomNode room, int minRoomWidth, int minRoomHeight,int lineWidth)
    {
        Direction direction;
        bool ifDivideByWidth = room.Width>(2 * minRoomWidth)+lineWidth*2-1;
        bool ifDivideByHeight = room.Height> (2 * minRoomHeight)+lineWidth * 2 - 1;
        if (ifDivideByHeight && ifDivideByWidth)
        {
            direction = (Direction)seed.Next(0, 2);
        }
        else if (ifDivideByWidth)
        {
            direction = Direction.Vertical;

        }
        else if (ifDivideByHeight)
        {
            direction = Direction.Horizontal;
        }
        else
            return null;

        return new PartitionLine(direction,GetPartitionCoordinates(direction, room.bottomLeft,room.topRight,minRoomWidth,minRoomHeight));
    }

    private Vector2Int GetPartitionCoordinates(Direction direction, Vector2Int bottomLeft, Vector2Int topRight, int minRoomWidth, int minRoomHeight)
    {
        Vector2Int coordinates = Vector2Int.zero;
        if(direction==Direction.Horizontal)
        {
            coordinates = new Vector2Int(0,seed.Next(bottomLeft.y+minRoomHeight,topRight.y-minRoomHeight));
        }
        if(direction==Direction.Vertical)
        {
            coordinates = new Vector2Int(seed.Next(bottomLeft.x+minRoomWidth,topRight.x-minRoomWidth),0);
        }
        return coordinates;
    }

    //private void ClearCollections()
    //{
    //    roomsToSlice.Clear();
    //    leafNodes.Clear();
    //    allNodes.Clear();
    //    borders.Clear();
    //}

    //连接相邻房间
    private void ConnectNeighborRooms(RoomNode left, RoomNode right, PartitionLine passage,int corridorWidth, int minRoomWidth, int minRoomHeight,int lineWidth)
    {
        Direction direction = passage.direction;
        List<Vector2Int> temp = new List<Vector2Int>();
        if(direction==Direction.Horizontal)
        {
            int x;
            if (left.Width < 2 * minRoomWidth+2*lineWidth-1)
            {
                x = seed.Next(left.bottomLeft.x + 1 + corridorWidth, right.topRight.x - corridorWidth);
            }
            else
            {
                x = (seed.Next(0, 2) == 0) ? seed.Next(left.bottomLeft.x + 1 + corridorWidth, left.bottomLeft.x +minRoomWidth-corridorWidth) : seed.Next(right.topRight.x - minRoomWidth+corridorWidth, right.topRight.x-corridorWidth);
            }
            for(int y =right.topRight.y+1;y<left.bottomLeft.y;y++)
            {
                for (int xOffset = x - corridorWidth; xOffset <= x + corridorWidth; xOffset++)
                {
                    temp.Add(new Vector2Int(xOffset, y));
                }
            }
            gates.Add(new Vector2Int(x,left.bottomLeft.y));
            gates.Add(new Vector2Int(x, right.topRight.y));

        }
        else if (direction==Direction.Vertical)
        {
            int y;
            if (left.Height < 2 * minRoomHeight + 2 * lineWidth - 1)
            {
                y = seed.Next(left.bottomLeft.y + 1 + corridorWidth, right.topRight.y - corridorWidth);
            }
            else
            {
                y= (seed.Next(0, 2) == 0) ? seed.Next(left.bottomLeft.y+ 1 + corridorWidth, left.bottomLeft.y +minRoomHeight-corridorWidth) : seed.Next(right.topRight.y -minRoomHeight+corridorWidth, right.topRight.y-corridorWidth);
            }
            for(int x=left.topRight.x+1;x<right.bottomLeft.x;x++)
            {
                for (int yOffset = y - corridorWidth; yOffset <= y + corridorWidth; yOffset++)
                {
                    temp.Add(new Vector2Int(x, yOffset));
                }
            }
            gates.Add(new Vector2Int(left.topRight.x,y));
            gates.Add(new Vector2Int(right.bottomLeft.x, y));

        }
        corridors.Add(new Corridor(temp));
    }

    private void ConnectNeighborRooms(List<RoomNode> rooms,int corridorWidth,PartitionLine passage)
    {
        for(int i =0;i<rooms.Count;i++)
        {
            //rooms.OrderByDescending();
        }
    }

}

public class PartitionLine
{
    public Direction direction;
    public Vector2Int coordinates;


    public PartitionLine(Direction direction, Vector2Int coordinates)
    {
        this.direction = direction;
        this.coordinates = coordinates;
    }


}

public class Corridor
{
    public List<Vector2Int> coordinates;

    public Corridor(List<Vector2Int> coordinates)
    {
        this.coordinates = new List<Vector2Int>( coordinates);
    }
}
