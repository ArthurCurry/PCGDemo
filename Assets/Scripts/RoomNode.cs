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

    public List<Corridor> corridors=new List<Corridor>();
    public List<Vector2> floors = new List<Vector2>();

    public RoomType type;

    public int nodeIndex;

    public bool isTiled;
    private List<Vector2Int> path=new List<Vector2Int>();
    private List<Vector2Int> posNearDoor;

     PathNode[,] nodes ;


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

    public List<Vector2Int> Path
    {
        get
        {
            if(path==null)
            {
                path = FindPath();
            }
            return path;
        }
    }

    public List<Vector2Int> DoorWays
    {
        get
        {
            if (posNearDoor==null)
            {
                posNearDoor = FindDoorWays();
            }
            return posNearDoor;
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
        nodes = new PathNode[Width, Height];
        //FindPath();
        //Debug.Log(path.Count);
    }

    public bool ContainsCoordinates(List<Vector2Int> coordinates)
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

    public bool ContainsCoordinate(int x,int y)
    {
        if (x >= this.bottomLeft.x && x <= this.topRight.x && y >= this.bottomLeft.y && y <= this.topRight.y)
            return true;
        return false;
    }

    private bool ContainsCoordinates(params Vector2Int[] coordinates)
    {
        //Debug.Log(coordinates.Length);
        foreach (Vector2Int coordinate in coordinates)
        {
            //Debug.Log(coordinate);
            if (coordinate.x <= topRight.x && coordinate.x >= bottomLeft.x && coordinate.y <= topRight.y && coordinate
                    .y >= bottomLeft.y)
            {
                //Debug.Log(coordinate);
                return true;
            }
        }
        return false;
    }

    public bool ContainsCorridor(Corridor corridor)
    {
        if(corridor.direction.Equals(Direction.Horizontal))
        {
            if (ContainsCoordinates(corridor.coordinates[0]+Vector2Int.left, corridor.coordinates[corridor.coordinates.Count - 1]+Vector2Int.left, corridor.coordinates[0] + Vector2Int.right, corridor.coordinates[corridor.coordinates.Count - 1] + Vector2Int.right))
                return true;
        }
        if (corridor.direction.Equals(Direction.Vertical))
        {
            if (ContainsCoordinates(corridor.coordinates[0]+Vector2Int.down, corridor.coordinates[corridor.coordinates.Count - 1]+Vector2Int.down, corridor.coordinates[0] + Vector2Int.up, corridor.coordinates[corridor.coordinates.Count - 1] + Vector2Int.up))
                return true;
        }
        return false;
    }

    public List<Vector2Int> FindPath()
    {
        List<Vector2Int> doors =FindDoorWays();
        Debug.Log(doors.Count);
        if(doors.Count==1)
        {
            
        }
        else if(doors.Count==2)
        {
            FindPathBetween(doors[0],doors[1]);
        }
        else if(doors.Count==3)
        {
            FindPathBetween(doors[0], doors[1]);
            FindPathBetween(doors[1], doors[2]);
            FindPathBetween(doors[0], doors[2]);

        }
        return path;
    }

    private List<Vector2Int> FindDoorWays()
    {
        List<Vector2Int> temp= new List<Vector2Int>();
        foreach(Corridor corridor in corridors)
        {
            if(corridor.direction.Equals(Direction.Horizontal))
            {
                if(corridor.coordinates[0].x<bottomLeft.x)
                {
                    temp.Add(new Vector2Int(bottomLeft.x,corridor.coordinates[0].y));
                }
                else if(corridor.coordinates[0].x > topRight.x)
                {
                    temp.Add(new Vector2Int(topRight.x, corridor.coordinates[0].y));

                }
            }
            else if(corridor.direction.Equals(Direction.Vertical))
            {
                if (corridor.coordinates[0].y < bottomLeft.y)
                {
                    temp.Add(new Vector2Int(corridor.coordinates[0].x,bottomLeft.y ));
                }
                else if (corridor.coordinates[0].y > topRight.y)
                {
                    temp.Add(new Vector2Int(corridor.coordinates[0].x, topRight.y));

                }
                
            }
        }
        return temp;
    }

    public void FindPathBetween(Vector2Int start,Vector2Int end)
    {
        //PathNode startPoint = new PathNode(start);
        //PathNode endPoint = new PathNode(end);
        Debug.Log(start + " " + end+" "+nodes.GetLength(0)+" "+nodes.GetLength(1) );
        for(int x=0;x<Width;x++)
        {
            for(int y=0;y<Height;y++)
            {
                nodes[x, y] = new PathNode(start,end,x,y);
            }
        }
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();
        PathNode startNode = nodes[start.x-bottomLeft.x, start.y-bottomLeft.y];
        PathNode endNode = nodes[end.x-bottomLeft.x, end.y-bottomLeft.y];
        openList.Add(startNode);
        //Debug.Log(openList.Count);
        while(openList.Count>0)
        {
            PathNode current = openList[0];
            for(int i =0;i<openList.Count;i++)
            {
                if (openList[i].totalCost < current.totalCost || (openList[i].totalCost == current.totalCost&&openList[i].distanceToEndPoint<current.distanceToEndPoint))
                {
                    current = openList[i];
                }
            }
            //Debug.Log(current.position);
            openList.Remove(current);
            closedList.Add(current);
            if(current==endNode)
            {
                GeneratePath(startNode, endNode);
                return;
            }
            foreach(PathNode node in GetNeighbourNodes(current))
            {
                if (closedList.Contains(node))
                    continue;
                int disToStart = current.distanceToStartPoint + current.GetDistance(node.position);
                if(disToStart<node.distanceToStartPoint||!openList.Contains(node))
                {
                    node.distanceToStartPoint = disToStart;
                    node.distanceToEndPoint = node.GetDistance(end);
                    node.parent = current;
                    if(!openList.Contains(node))
                    {
                        openList.Add(node);
                    }
                }
            }
        }
    }

    private List<PathNode> GetNeighbourNodes(PathNode node)
    {
        List<PathNode> temp = new List<PathNode>();
        if(node.position.x+1<Width)
            temp.Add(nodes[node.position.x + 1, node.position.y]);
        if(node.position.x-1>=0)
            temp.Add(nodes[node.position.x - 1, node.position.y]);
        if(node.position.y+1<Height)
            temp.Add(nodes[node.position.x , node.position.y+1]);
        if(node.position.y-1>=0)
            temp.Add(nodes[node.position.x , node.position.y-1]);
        return temp;
    }

    private void GeneratePath(PathNode start,PathNode end)
    {
        List<Vector2Int> temp = new List<Vector2Int>();
        PathNode node = end;
        while (node.parent != null)
        {
            temp.Add(node.position+bottomLeft);
            node = node.parent;
        }
        temp.Add(start.position+bottomLeft);
        path.AddRange(temp);
    }
}

public class RoomGrid
{
    PathNode[,] nodes;

    public RoomGrid(int width,int height,Vector2Int start,Vector2Int end)
    {

    }
}

public class PathNode
{
    public Vector2Int position;
    public int distanceToStartPoint;
    public int distanceToEndPoint;
    public int totalCost;
    public PathNode parent;

    public int GetDistance(Vector2Int targetPos)
    {
        return Mathf.Abs(targetPos.x - position.x) + Mathf.Abs(targetPos.y-position.y);
    }

    public PathNode(Vector2Int position)
    {
        this.position = position;
    }

    public PathNode(Vector2Int start,Vector2Int end,int x,int y)
    {
        position = new Vector2Int(x, y);
        distanceToEndPoint = GetDistance(end);
        distanceToStartPoint = GetDistance(start);
        totalCost = distanceToStartPoint + distanceToEndPoint;
    }

    
}

