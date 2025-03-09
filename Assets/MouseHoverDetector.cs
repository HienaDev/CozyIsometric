using UnityEngine;

public class MouseHoverDetector : MonoBehaviour
{
    private Camera mainCamera;

    private GameObject lastSeenObject;

    void Start()
    {
        // Get the main camera in the scene
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Create a ray from the mouse position
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit))
        {
            // Get the GameObject that was hit
            GameObject hoveredObject = hit.collider.gameObject;
            SpriteRenderer sr = hoveredObject.GetComponentInParent<SpriteRenderer>();

            if (sr != null) {
                sr.color = Color.red;
            }

            if(lastSeenObject!= null)
                if (lastSeenObject != hoveredObject)
                {
                    lastSeenObject.GetComponentInParent<SpriteRenderer>().color = Color.white;
                    Debug.Log("last seen different");
                }

            lastSeenObject = hoveredObject;
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
        
        
    }
}