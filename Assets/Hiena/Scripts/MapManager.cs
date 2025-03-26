using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class MapManager
    {

        public Dictionary<Vector3Int, IsometricTile> Nodes { get; private set; }
        public MapManager()
        {
            Nodes = new Dictionary<Vector3Int, IsometricTile>();
        }

        public void ClearNodes()
        {
            Nodes.Clear();
        }

        public void AddNode(Vector3Int pos, IsometricTile node)
        {

            Nodes[pos] = node;
        }

        public void RemoveNode(Vector3Int pos)
        {
            Debug.Log("Try to remove node: " + Nodes[pos]);
            Nodes.Remove(pos);

        }

        public bool CheckNode(Vector3Int pos)
        {
            return Nodes.ContainsKey(pos);

        }

        public IsometricTile GetNode(Vector3Int pos)
        {
            return Nodes[pos];
        }

    }
}
