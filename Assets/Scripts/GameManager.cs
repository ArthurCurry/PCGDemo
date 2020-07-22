using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private MapGenerator mapGenerator;

	// Use this for initialization
	void Start () {
        mapGenerator = this.transform.GetComponent<MapGenerator>();
        mapGenerator.GenerateBinaryMap();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            mapGenerator.tilemap.RefreshAllTiles();
            Debug.Log("r");
        }
	}
}
