using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private LayerMask ignoreRaycastLayer;
    private int ignoreLayer;
    [SerializeField] private LayerMask clickableLayer;
    private int clickableLayerInt;

    [SerializeField] private GameObject theWholeIsland;
    [SerializeField] private MouseHoverDetector2D scriptToPlaceObject;
    private bool placingObject = true;

    // Start is called before the first frame update
    void Start()
    {
        ignoreLayer = GetFirstLayerFromMask(ignoreRaycastLayer);
        clickableLayerInt = GetFirstLayerFromMask(clickableLayer);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (placingObject)
            {
                if (clickableLayerInt != -1)
                {
                    // Assign the layer to the target object
                    theWholeIsland.layer = clickableLayerInt;
                    Debug.Log("Assigned layer: " + LayerMask.LayerToName(clickableLayerInt) + " to " + theWholeIsland.name);
                }
                scriptToPlaceObject.ToggleActive(false);
                placingObject = false;
            }
            else
            {
                if (ignoreLayer != -1)
                {
                    // Assign the layer to the target object
                    theWholeIsland.layer = ignoreLayer;
                    Debug.Log("Assigned layer: " + LayerMask.LayerToName(ignoreLayer) + " to " + theWholeIsland.name);
                }
                scriptToPlaceObject.ToggleActive(true);
                placingObject = true;
            }
        }
    }

        int GetFirstLayerFromMask(LayerMask mask)
        {
            // Loop through all possible layers (0 to 31)
            for (int i = 0; i < 32; i++)
            {
                // Check if the layer is included in the mask
                if ((mask.value & (1 << i)) != 0)
                {
                    return i; // Return the first layer found in the mask
                }
            }

            return -1; // No valid layer found
        }
    }
