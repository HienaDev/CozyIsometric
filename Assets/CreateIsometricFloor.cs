using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

public class CreateIsometricFloor : MonoBehaviour
{
    [SerializeField] private int floorWidth = 15;
    [SerializeField] private int floorHeight = 4;
    [SerializeField] private GameObject floorTile;
    private List<GameObject> tiles = new List<GameObject>();

    public IsometricTile startingTile;

    public MapManager map;
    // sort: 10000 - y
    // Start is called before the first frame update
    void Start()
    {
        map = new MapManager();
        HabboIsometric();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            map.ClearNodes();

            foreach(GameObject tile in tiles)
            {
                Destroy(tile);
            }

            tiles.Clear();

            HabboIsometric();
        }
    }

    public void CreateFloorWeirdIsomteric()
    {
        for (int y = 0; y < floorHeight; y++)
        {
            for (int x = 0; x < floorWidth; x++)
            {
                GameObject tile = Instantiate(floorTile);
                if (y % 2 == 0)
                {
                    tile.transform.position = new Vector3(x * 32, y * 8, y);
                }
                else
                {
                    tile.transform.position = new Vector3(x * 32 + 16, y * 8, y);
                }
                tile.name = "Tile_" + x + "_" + y;
                tile.transform.SetParent(this.transform);
                tile.GetComponent<SpriteRenderer>().sortingOrder = 10000 - (int)tile.transform.position.y;
            }
        }

        //BoxCollider2D collider = this.gameObject.AddComponent<BoxCollider2D>();

        //collider.size = new Vector2((floorWidth + 1) * 32, (floorHeight + 1) * 8);
        //collider.offset = new Vector2((floorWidth) * 16 - 8, (floorHeight) * 4);

        BoxCollider collider = this.gameObject.AddComponent<BoxCollider>();
        collider.size = new Vector3((floorWidth + 1) * 32, (floorHeight + 1) * 8, 0f);
        collider.center = new Vector3((floorWidth) * 16 - 8, (floorHeight) * 4, 0f);
    }

    public void HabboIsometric()
    {
        for (int y = 0; y < floorHeight; y++)
        {
            for (int x = 0; x < floorWidth; x++)
            {
                GameObject tile = Instantiate(floorTile);

                tile.transform.position = new Vector3(x * 16 + y * 16, x * -8 + y * 8, (x * -8f + y * 8f) / 1000f);

                tile.name = "Tile_" + x + "_" + y;
                tile.transform.SetParent(this.transform);
                tile.GetComponent<SpriteRenderer>().sortingOrder = 10000 - (int)tile.transform.position.y;

                tiles.Add(tile);

                IsometricTile isometricTile = tile.GetComponent<IsometricTile>();

                if (x == 0 && y == 0)
                {
                    startingTile = isometricTile;
                }

                isometricTile.type = IsometricTile.Type.Floor;

                bool isWalkable = Random.Range(0, 100) > 30;


                isometricTile.isWalkable = isWalkable;



                if (!isWalkable)
                {
                    tile.GetComponent<SpriteRenderer>().color = Color.green;
                }

                if (x == 0 && y == 0)
                {

                    isWalkable = true;
                    tile.GetComponent<SpriteRenderer>().color = Color.white;
                }
                isometricTile.pos = new Vector2Int(x, y);

                map.AddNode(new Vector2Int(x, y), isometricTile);
            }
        }

        //BoxCollider2D collider = this.gameObject.AddComponent<BoxCollider2D>();

        //collider.size = new Vector2((floorWidth + 1) * 32, (floorHeight + 1) * 8);
        //collider.offset = new Vector2((floorWidth) * 16 - 8, (floorHeight) * 4);

        BoxCollider collider = this.gameObject.AddComponent<BoxCollider>();
        collider.size = new Vector3((floorWidth + floorHeight) * (16 / Mathf.Sqrt(2f)), (floorWidth + floorHeight) * (16 / Mathf.Sqrt(2f)), 0f);
        collider.center = new Vector3(0f, 0f, 0f);
    }
}
