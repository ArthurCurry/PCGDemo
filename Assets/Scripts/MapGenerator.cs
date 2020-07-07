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

    List<Map> maps = new List<Map>();
    public string seed;
    public bool useRandomSeed;
    [Range(0,100)]
    public float percentage;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Map GenerateMap(int width,int height)
    {
        Map map = new Map(width, height);
        if (useRandomSeed)
            seed = Time.time.ToString();
        System.Random random = new System.Random(seed.GetHashCode());
        for(int x=0;x<width;x++)
        {
            for (int y = 0; y <height; y++)
            {
                float xCord = x / MapUnit.unitScale*random.Next(0,100);
                float yCord = y / MapUnit.unitScale * random.Next(0, 100);

                map.mapMatrix[x, y] = Mathf.PerlinNoise(xCord, yCord);
                if(map.mapMatrix[x,y]<percentage/100f)
                {
                    map.mapMatrix[x,y] = 0;
                }
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
        DrawMapInEditor(GenerateMap(50, 50));
    }
}
