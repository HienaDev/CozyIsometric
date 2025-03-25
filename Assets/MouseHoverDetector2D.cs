using System.Collections.Generic;
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

        if(testPathFinding)
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

        // Check if the raycast hit something
        if (hit.collider != null)
        {
            // Get the GameObject that was hit
            GameObject hoveredObject = hit.collider.gameObject.transform.parent.gameObject;
            // Get the side that was hit
            TAG_Sides.Side side = hit.collider.gameObject.GetComponent<TAG_Sides>().side;
            SpriteRenderer sr = hoveredObject.GetComponentInParent<SpriteRenderer>();

            if (sr != null)
            {
                if(side == TAG_Sides.Side.Left)
                {
                    placeable.transform.position = hoveredObject.transform.position + new Vector3(-16f, -8f, 0f);
                }
                else if (side == TAG_Sides.Side.Right)
                {
                    placeable.transform.position = hoveredObject.transform.position + new Vector3(16f, -8f, 0f);
                }
                else if (side == TAG_Sides.Side.Top)
                {
                    placeable.transform.position = hoveredObject.transform.position + new Vector3(0f, 16f, 0f);
                }
                //placeable.transform.position = hoveredObject.transform.position + new Vector3(0f, 16f, 0f);
                placeable.GetComponentInParent<SpriteRenderer>().sortingOrder = hoveredObject.GetComponentInParent<SpriteRenderer>().sortingOrder + 1;
                sr.color = Color.red;
            }

            if (lastSeenObject != null)
                if (lastSeenObject != hoveredObject)
                {
                    lastSeenObject.GetComponentInParent<SpriteRenderer>().color = Color.white;
                    Debug.Log("last seen different");
                }

            lastSeenObject = hoveredObject;
            lastSeenSide = side;
            // Output the name of the object to the console
            Debug.Log("Hovering over: " + hoveredObject.name);
        }
        else
        {
            if (lastSeenObject != null)
            {
                lastSeenObject.GetComponentInParent<SpriteRenderer>().color = Color.white;
                lastSeenObject = null;
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            if (lastSeenObject != null)
            {
                Debug.Log("Clicked on: " + lastSeenObject.name);

                if (lastSeenSide == TAG_Sides.Side.Left)
                {
                    placeable.transform.position = lastSeenObject.transform.position + new Vector3(-16f, -8f, -0.008f);
                }
                else if (lastSeenSide == TAG_Sides.Side.Right)
                {
                    placeable.transform.position = lastSeenObject.transform.position + new Vector3(16f, -8f, -0.008f);
                }
                else if (lastSeenSide == TAG_Sides.Side.Top)
                {
                    placeable.transform.position = lastSeenObject.transform.position + new Vector3(0f, 16f, -0.008f);
                }

                placeable.GetComponentInParent<SpriteRenderer>().sortingOrder = lastSeenObject.GetComponentInParent<SpriteRenderer>().sortingOrder + 1;
                IsometricTile placeableTile = placeable.GetComponent<IsometricTile>();
                placeableTile.ToggleSideColliders(true);
                
                if(placeableTile.isWalkable)
                {
                    if(lastSeenSide == TAG_Sides.Side.Top)
                        placeableTile.type = IsometricTile.Type.Wall;
                    else
                    {
                        if(lastSeenObject.GetComponent<IsometricTile>().type == IsometricTile.Type.Floor)
                            placeableTile.type = IsometricTile.Type.Floor;
                    }
                }
                else
                { 
                    // Could be something else
                    placeableTile.type = IsometricTile.Type.Wall;
                }

                    lastSeenObject = null;

                placeable = Instantiate(placeableTest, tileParent);
                placeable.GetComponent<IsometricTile>().ToggleSideColliders(false);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (lastSeenObject != null)
            {

                Destroy(lastSeenObject);
                lastSeenObject = null;
            }
        }
    }
}