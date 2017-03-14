using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGen : MonoBehaviour
{
    public int width = 7;
    public int height = 5;

    public GameObject square;

    public  List<GameObject> tiles = new List<GameObject>();
    public Dictionary<Vector2 , GameObject> position = new Dictionary<Vector2, GameObject>();

    // Use this for initialization
    void Awake ()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject go = Instantiate(square, new Vector3(square.transform.position.x + i, square.transform.position.y - j, 0), transform.rotation) as GameObject;
                //tiles.Add(go);
                position.Add(new Vector2(i,j) , go);
            } 
        }

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
