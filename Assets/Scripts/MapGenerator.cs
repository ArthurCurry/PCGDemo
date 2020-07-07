using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{

}


public class MapGenerator : MonoBehaviour {

    List<Map> maps = new List<Map>();
    public string seed;
    public bool useRandomSeed;
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


        //Debug.Log();
        return map;
    }


}
