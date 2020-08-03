using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    private Animator anim;
    private bool opened = false;
    private GameObject player;
    public List<Vector2Int> neighbourCoordinate;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        SetNeighbourCordsOfDoor(this.GetComponent<Door>(), Vector3Int.FloorToInt(this.transform.position));
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(anim.isPlaying);
        if (player!=null&&Input.GetKeyDown(KeyCode.E)&&(transform.position-player.transform.position).magnitude<=1.5f)
        {
            anim.SetTrigger("play");
            EventDispatcher.GenerateRoom(neighbourCoordinate);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if(player == null)
                player = collision.gameObject;
            Debug.Log("stay");
        }
    }

    public void OnDoorClosed()
    {
        this.gameObject.SetActive(false);
        //this.GetComponent<BoxCollider2D>().enabled = false;
    }

    private void SetNeighbourCordsOfDoor(Door door, Vector3Int position)
    {
        door.neighbourCoordinate = new List<Vector2Int>();
        door.neighbourCoordinate.Add((position + Vector3Int.right).ConvertToVector2Int());
        door.neighbourCoordinate.Add((position + Vector3Int.left).ConvertToVector2Int());
        door.neighbourCoordinate.Add((position + Vector3Int.up).ConvertToVector2Int());
        door.neighbourCoordinate.Add((position + Vector3Int.down).ConvertToVector2Int());

    }
}
