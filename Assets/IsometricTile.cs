using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IsometricTile : MonoBehaviour
{
    [Flags]
    public enum Type
    {
        Floor, // Can be walked on and place as wall
        Wall, // Can be walked on and place as wall
        Decoration, // Can't be walked on but can be placed as wall
        Door,
        None
    }

    public int G = int.MaxValue;
    public int H = int.MaxValue;
    public int F => G + H;

    public IsometricTile previous;

    public Vector3Int pos;

    public bool isWalkable = false;

    [SerializeField] private Collider2D[] sideColliders;
    
    public Type type = Type.None;

    public List<IsometricTile> tilesImBlocking = new List<IsometricTile>();
    public List<IsometricTile> tilesBlockingMe = new List<IsometricTile>();

    public void ToggleSideColliders(bool toggle)
    {
        for (int i = 0; i < sideColliders.Length; i++)
        {
            sideColliders[i].enabled = toggle;
        }
    }
}
