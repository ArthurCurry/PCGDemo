using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map {

    public int width;
    public int height;

    private int[,] mapMatrix;

    public Map(int width,int height)
    {
        this.width = width;
        this.height = height;
        mapMatrix = new int[width, height];
    }

}
