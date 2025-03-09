using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricTile : MonoBehaviour
{
    [SerializeField] private Collider2D[] sideColliders;

    public void ToggleSideColliders(bool toggle)
    {
        for (int i = 0; i < sideColliders.Length; i++)
        {
            sideColliders[i].enabled = toggle;
        }
    }
}
