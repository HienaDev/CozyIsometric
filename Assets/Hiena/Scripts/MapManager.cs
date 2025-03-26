using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class MapManager
    {

        public Dictionary<Vector2Int, IsometricTile> Nodes { get; private set; }
        public MapManager()
        {
            Nodes = new Dictionary<Vector2Int, IsometricTile>();
        }

        public void ClearNodes()
        {
            Nodes.Clear();
        }

        public void AddNode(Vector2Int pos, IsometricTile node)
        {

            Nodes[pos] = node;
        }

        public void RemoveNode(Vector2Int pos)
        {
            Debug.Log("Try to remove node: " + Nodes[pos]);
            Nodes.Remove(pos);

        }


    }
}
