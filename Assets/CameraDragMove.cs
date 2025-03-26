using UnityEngine;

public class CameraDragMove : MonoBehaviour
{
    public float dragSpeed = 2f;
    private Vector3 lastMousePosition;
    private bool isDragging = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Start dragging
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
            Cursor.lockState = CursorLockMode.Confined; // Keep cursor in the window
            Cursor.visible = false; // Hide cursor while dragging
        }

        if (Input.GetMouseButtonUp(0)) // Stop dragging
        {
            isDragging = false;
            Cursor.lockState = CursorLockMode.None; // Unlock cursor
            Cursor.visible = true; // Show cursor
        }

        if (isDragging)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 move = new Vector3(-delta.x * dragSpeed * Time.deltaTime, -delta.y * dragSpeed * Time.deltaTime, 0);
            transform.position += move;
            lastMousePosition = Input.mousePosition;
        }
    }
}
