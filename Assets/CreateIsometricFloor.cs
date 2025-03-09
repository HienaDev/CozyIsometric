using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateIsometricFloor : MonoBehaviour
{
    [SerializeField] private int floorWidth = 15;
    [SerializeField] private int floorHeight = 4;
    [SerializeField] private GameObject floorTile;
    // sort: 10000 - y
    // Start is called before the first frame update
    void Start()
    {
        CreateFloor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateFloor()
    {
        for (int y = 0; y < floorHeight; y++)
        {
            for (int x = 0; x < floorWidth; x++)
            {
                GameObject tile = Instantiate(floorTile);
                if(y % 2 == 0)
                {
                    tile.transform.position = new Vector3(x * 32, y * 8, y);
                }
                else
                {
                    tile.transform.position = new Vector3(x * 32 + 16, y * 8, y);
                }
                tile.name = "Tile_" + x + "_" + y;  
                tile.transform.SetParent(this.transform);
                tile.GetComponent<SpriteRenderer>().sortingOrder = 10000 - y;
            }
        }

        //BoxCollider2D collider = this.gameObject.AddComponent<BoxCollider2D>();

        //collider.size = new Vector2((floorWidth + 1) * 32, (floorHeight + 1) * 8);
        //collider.offset = new Vector2((floorWidth) * 16 - 8, (floorHeight) * 4);

        BoxCollider collider = this.gameObject.AddComponent<BoxCollider>();
        collider.size = new Vector3((floorWidth + 1) * 32, (floorHeight + 1) * 8, 0f);
        collider.center = new Vector3((floorWidth) * 16 - 8, (floorHeight) * 4, 0f);
    }
}
