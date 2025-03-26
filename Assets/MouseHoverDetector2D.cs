using Map;
using System;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;
using static TAG_Sides;

public class MouseHoverDetector2D : MonoBehaviour
{
    private Camera mainCamera;

    private GameObject lastSeenObject;
    private Side lastSeenSide;

    [SerializeField] private GameObject placeableTest;
    private GameObject placeable;

    [SerializeField] private Transform tileParent;

    private bool active = true;

    [SerializeField] private bool testPathFinding = false;

    [SerializeField] private CreateIsometricFloor createIsometricFloor;

    private List<IsometricTile> lastPath;
    private Pathfinding pathfinding;

    private int count = 0;

    void Start()
    {
        // Get the main camera in the scene
        mainCamera = Camera.main;

        placeable = Instantiate(placeableTest, tileParent);

        placeable.GetComponent<IsometricTile>().ToggleSideColliders(false);





        pathfinding = new Pathfinding();
    }

    public void ToggleActive(bool toggle)
    {
        active = toggle;
        placeable.SetActive(toggle);
    }

    public void TestPathFinding()
    {
        // Convert the mouse position to a world point in 2D space
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // Perform a 2D raycast at the mouse position
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        // Check if the raycast hit something
        if (hit.collider != null)
        {
            // Get the GameObject that was hit
            GameObject hoveredObject = hit.collider.gameObject.transform.parent.gameObject;
            // Get the side that was hit
            TAG_Sides.Side side = hit.collider.gameObject.GetComponent<TAG_Sides>().side;
            SpriteRenderer sr = hoveredObject.GetComponentInParent<SpriteRenderer>();

            lastSeenObject = hoveredObject;
            lastSeenSide = side;
            // Output the name of the object to the console
            //Debug.Log("Hovering over: " + hoveredObject.name);
        }
        else
        {
            if (lastSeenObject != null)
            {
                lastSeenObject.GetComponentInParent<SpriteRenderer>().color = Color.white;
                lastSeenObject = null;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (lastSeenObject != null)
            {
                Debug.Log(lastSeenObject.GetComponent<IsometricTile>().pos);
                Debug.Log(lastSeenObject.name);
                if (lastPath != null)
                {
                    foreach (IsometricTile isometricTile in lastPath)
                    {
                        if (isometricTile != null)
                            isometricTile.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
                List<IsometricTile> path = pathfinding.FindPath(createIsometricFloor.startingTile, lastSeenObject.GetComponent<IsometricTile>(), createIsometricFloor.map);
                foreach (IsometricTile isometricTile in path)
                {
                    isometricTile.GetComponent<SpriteRenderer>().color = Color.blue;
                }



                lastPath = path;

            }
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {
            testPathFinding = !testPathFinding;
        }

        if (testPathFinding)
        {
            TestPathFinding();
            return;
        }

        if (!active)
            return;


        // Convert the mouse position to a world point in 2D space
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // Perform a 2D raycast at the mouse position
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        ShowBlockIndication(hit);

        PlaceBlock();

        RemoveBlock();


    }

    private void RemoveBlock()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (lastSeenObject != null)
            {
                IsometricTile lastIsometricTile = lastSeenObject.GetComponent<IsometricTile>();

                foreach (IsometricTile blockingTile in lastIsometricTile.tilesImBlocking)
                {
                    if (blockingTile != null)
                    {

                        blockingTile.tilesBlockingMe.Remove(lastIsometricTile);



                        if (blockingTile.tilesBlockingMe.Count <= 0)
                        {
                            Debug.Log("No tiles blocking me, im walkable");
                            if (blockingTile.pos.z == 0)
                                blockingTile.isWalkable = true;
                        }
                        else
                        {
                            Debug.Log("Tiles blocking " + blockingTile + ": " + blockingTile.tilesBlockingMe.Count);
                        }


                    }
                }


                createIsometricFloor.map.RemoveNode(lastIsometricTile.pos);
                Destroy(lastSeenObject, 0.1f);
                lastSeenObject = null;
            }
        }
    }

    private void PlaceBlock()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (lastSeenObject != null)
            {
                Debug.Log("Clicked on: " + lastSeenObject.name);
                Debug.Log("placeable position: " + placeable.transform.position);
                Debug.Log("last seen object position: " + lastSeenObject.transform.localPosition);

                // Recolor it to full white
                placeable.GetComponent<SpriteRenderer>().color = Color.white;


                IsometricTile placeableTile = placeable.GetComponent<IsometricTile>();
                placeableTile.ToggleSideColliders(true);

                IsometricTile lastSeenTile = lastSeenObject.GetComponent<IsometricTile>();



                // If it wasnt on top, it was either left or right, we assign its position based on the clicked block
                // we make the 2 blocks direclty under it unwalkable, so if they are 3 away they dont get blocked
                // giving npcs a height of 2

                if (lastSeenSide == TAG_Sides.Side.Top)
                {
                    placeableTile.pos = new Vector3Int(lastSeenTile.pos.x, lastSeenTile.pos.y, lastSeenTile.pos.z + 1);
                }
                else if (lastSeenSide == TAG_Sides.Side.Left)
                {
                    placeableTile.pos = new Vector3Int(lastSeenTile.pos.x, lastSeenTile.pos.y - 1, lastSeenTile.pos.z);
                }
                else if (lastSeenSide == TAG_Sides.Side.Right)
                {
                    placeableTile.pos = new Vector3Int(lastSeenTile.pos.x + 1, lastSeenTile.pos.y, lastSeenTile.pos.z);
                }

                Vector3Int positionToCheck;

                // If the tile is a floor or wall, we check if it has anything above it
                // if it doesnt, it becomes walkable
                if (placeableTile.type == IsometricTile.Type.Floor || placeableTile.type == IsometricTile.Type.Wall)
                {

                    if (lastSeenTile.pos.z == 0)
                    {
                        bool hasSomethingAbove = false;

                        positionToCheck = new Vector3Int(placeableTile.pos.x, placeableTile.pos.y, placeableTile.pos.z + 1);
                        if (createIsometricFloor.map.CheckNode(positionToCheck))
                        {
                            createIsometricFloor.map.GetNode(positionToCheck).tilesImBlocking.Add(placeableTile);
                            placeableTile.tilesBlockingMe.Add(createIsometricFloor.map.GetNode(positionToCheck));
                            hasSomethingAbove = true;
                        }


                        positionToCheck = new Vector3Int(placeableTile.pos.x, placeableTile.pos.y, placeableTile.pos.z + 2);
                        if (createIsometricFloor.map.CheckNode(positionToCheck))
                        {

                            hasSomethingAbove = true;


                            createIsometricFloor.map.GetNode(positionToCheck).tilesImBlocking.Add(placeableTile);
                            placeableTile.tilesBlockingMe.Add(createIsometricFloor.map.GetNode(positionToCheck));
                        }

                        if (!hasSomethingAbove)
                        {
                            placeableTile.isWalkable = true;
                        }

                    }
                }

                // Then check if there is anything undere this tile by 2, and block it if there is
                bool hasSomethingUnder = false;

                positionToCheck = new Vector3Int(placeableTile.pos.x, placeableTile.pos.y, placeableTile.pos.z - 1);
                if (createIsometricFloor.map.CheckNode(positionToCheck))
                {
                    hasSomethingUnder = true;
                    createIsometricFloor.map.GetNode(positionToCheck).isWalkable = false;
                    placeableTile.tilesImBlocking.Add(createIsometricFloor.map.GetNode(positionToCheck));
                    createIsometricFloor.map.GetNode(positionToCheck).tilesBlockingMe.Add(placeableTile);
                }

                positionToCheck = new Vector3Int(placeableTile.pos.x, placeableTile.pos.y, placeableTile.pos.z - 2);
                if (createIsometricFloor.map.CheckNode(positionToCheck))
                {
                    if (!hasSomethingUnder)
                    {
                        
                    }
                    createIsometricFloor.map.GetNode(positionToCheck).isWalkable = false;
                    placeableTile.tilesImBlocking.Add(createIsometricFloor.map.GetNode(positionToCheck));
                    createIsometricFloor.map.GetNode(positionToCheck).tilesBlockingMe.Add(placeableTile);
                }



                createIsometricFloor.map.AddNode(placeableTile.pos, placeableTile);
                Debug.Log("Added node");

                placeable.name += count;
                count++;

                lastSeenObject = null;

                placeable = Instantiate(placeableTest, tileParent);
                placeable.GetComponent<IsometricTile>().ToggleSideColliders(false);


            }
        }
    }

    private void ShowBlockIndication(RaycastHit2D hit)
    {
        // Check if the raycast hit something
        if (hit.collider != null)
        {
            // Get the GameObject that was hit
            GameObject hoveredObject = hit.collider.gameObject.transform.parent.gameObject;
            // Get the side that was hit
            TAG_Sides.Side side = hit.collider.gameObject.GetComponent<TAG_Sides>().side;
            SpriteRenderer sr = hoveredObject.GetComponentInParent<SpriteRenderer>();

            // Place above to the right or to the left based on the side seen
            if (sr != null)
            {
                if (lastSeenSide == TAG_Sides.Side.Left)
                {
                    placeable.transform.localPosition = hoveredObject.transform.localPosition + new Vector3(-16f, -8f, -0.008f);
                }
                else if (lastSeenSide == TAG_Sides.Side.Right)
                {
                    placeable.transform.localPosition = hoveredObject.transform.localPosition + new Vector3(16f, -8f, -0.008f);
                }
                else if (lastSeenSide == TAG_Sides.Side.Top)
                {
                    placeable.transform.localPosition = hoveredObject.transform.localPosition + new Vector3(0f, 16f, -0.008f);
                }

                SpriteRenderer placeableSR = placeable.GetComponent<SpriteRenderer>();

                // Sort the sprite always above what it was placed on
                placeableSR.sortingOrder = hoveredObject.GetComponentInParent<SpriteRenderer>().sortingOrder + 9;

                // Make the sprite half transparent to indicate that it can be placed
                placeableSR.color = new Color(1, 1, 1, 0.5f);
            }

            if (lastSeenObject != null)
                if (lastSeenObject != hoveredObject)
                {
                    Debug.Log("last seen different");
                }

            // Update last seen
            lastSeenObject = hoveredObject;
            lastSeenSide = side;
            // Output the name of the object to the console
            Debug.Log("Hovering over: " + hoveredObject.name);
        }
    }
}