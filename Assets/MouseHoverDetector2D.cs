using UnityEngine;
using static TAG_Sides;

public class MouseHoverDetector2D : MonoBehaviour
{
    private Camera mainCamera;

    private GameObject lastSeenObject;
    private Side lastSeenSide;

    [SerializeField] private GameObject tableTest;
    private GameObject table;

    [SerializeField] private Transform tileParent;

    private bool active = true;

    void Start()
    {
        // Get the main camera in the scene
        mainCamera = Camera.main;

        table = Instantiate(tableTest, tileParent);
        
        table.GetComponent<IsometricTile>().ToggleSideColliders(false);
    }

    public void ToggleActive(bool toggle)
    {
        active = toggle;
        table.SetActive(toggle);
    }

    void Update()
    {
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
                    table.transform.position = hoveredObject.transform.position + new Vector3(-16f, -8f, 0f);
                }
                else if (side == TAG_Sides.Side.Right)
                {
                    table.transform.position = hoveredObject.transform.position + new Vector3(16f, -8f, 0f);
                }
                else if (side == TAG_Sides.Side.Top)
                {
                    table.transform.position = hoveredObject.transform.position + new Vector3(0f, 16f, 0f);
                }
                //table.transform.position = hoveredObject.transform.position + new Vector3(0f, 16f, 0f);
                table.GetComponentInParent<SpriteRenderer>().sortingOrder = hoveredObject.GetComponentInParent<SpriteRenderer>().sortingOrder + 1;
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
                    table.transform.position = lastSeenObject.transform.position + new Vector3(-16f, -8f, -1f);
                }
                else if (lastSeenSide == TAG_Sides.Side.Right)
                {
                    table.transform.position = lastSeenObject.transform.position + new Vector3(16f, -8f, -1f);
                }
                else if (lastSeenSide == TAG_Sides.Side.Top)
                {
                    table.transform.position = lastSeenObject.transform.position + new Vector3(0f, 16f, -1f);
                }

                table.GetComponentInParent<SpriteRenderer>().sortingOrder = lastSeenObject.GetComponentInParent<SpriteRenderer>().sortingOrder + 1;
                table.GetComponent<IsometricTile>().ToggleSideColliders(true);
                lastSeenObject = null;

                table = Instantiate(tableTest, tileParent);
                table.GetComponent<IsometricTile>().ToggleSideColliders(false);
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