using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricTile : MonoBehaviour
{

    public enum Type
    {
        Floor,
        Wall,
        None
    }

    public int G = int.MaxValue;
    public int H = int.MaxValue;
    public int F => G + H;

    public IsometricTile previous;

    public Vector2Int pos;

    public bool isWalkable = false;

    [SerializeField] private Collider2D[] sideColliders;
    
    public Type type = Type.None;

    public void ToggleSideColliders(bool toggle)
    {
        for (int i = 0; i < sideColliders.Length; i++)
        {
            sideColliders[i].enabled = toggle;
        }
    }
}
