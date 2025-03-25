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



        public void AddNode(Vector2Int pos, IsometricTile node)
        {

            Nodes[pos] = node;
        }


    }
}
