using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{

}

public enum MapType
{
    PerlinNoise,
    Binary,
}

public class MapGenerator : MonoBehaviour {


    public Map map;
    public string seed;
    public bool useRandomSeed;
    [Range(0,100)]
    public float percentage;
    [Range(0, 10)]
    public int minBlockNum;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Map GenerateNoiseMap(int width,int height)
    {
        map = new Map(width, height);
        float mapDepth = 0f;
        if (useRandomSeed)
            seed = Time.time.ToString();
        System.Random random = new System.Random(seed.GetHashCode());
        for(int x=0;x<width;x++)
        {
            for (int y = 0; y <height; y++)
            {
                //float xCord = x / MapUnit.unitScale * random.Next(0, 100);
                //float yCord = y / MapUnit.unitScale * random.Next(0, 100);
                float xCord = x / MapUnit.unitScale;
                float yCord = y / MapUnit.unitScale;

                map.mapMatrix[x, y] = Mathf.PerlinNoise(xCord, yCord)*2-1;
                //if(map.mapMatrix[x,y]<percentage/100f)
                //{
                //    map.mapMatrix[x,y] = 0;
                //}
            }
        }

        //Debug.Log();
        return map;
    }

    private void DrawMapInEditor(Map map)
    {
        if(map!=null)
        {
            for (int x = 0; x <map.width; x++)
            {
                for (int y = 0; y < map.height; y++)
                {
                    Vector3 position = new Vector3(x*MapUnit.unitScale,y*MapUnit.unitScale,0);
                    Gizmos.color = Color.Lerp(Color.white,Color.black,map.mapMatrix[x,y]);
                    //Debug.Log(map.mapMatrix[x, y]);
                    Gizmos.DrawCube(position,Vector3.one*MapUnit.unitScale);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        DrawMapInEditor(GenerateNoiseMap(50, 50));
        //DrawMapInEditor(GenerateRandomMap(50,50));
    }

    public Map GenerateRandomMap(int mapWidth,int mapHeight)
    {
        Map map = new Map(mapWidth, mapHeight);
        if (useRandomSeed)
            seed = Time.time.ToString();
        System.Random random = new System.Random(seed.GetHashCode());
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {


                map.mapMatrix[x, y] = (random.Next(0, 100) < percentage) ? 0 : 1;
            }
        }
        return map;
    }

    private void SmoothMap(int num)
    {

    }

    
}
