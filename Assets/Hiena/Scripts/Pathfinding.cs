using Map;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding
{

    public List<IsometricTile> FindPath(IsometricTile start, IsometricTile end, MapManager map)
    {
        List<IsometricTile> openList = new List<IsometricTile>();
        List<IsometricTile> closedList = new List<IsometricTile>();

        openList.Add(start);

        while (openList.Count > 0)
        {
            IsometricTile currentNode = openList.OrderBy(x => x.F).First();

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == end)
            {
                return GetFinishedList(start, end);
            }

            var neighborNodes = GetNeighbourNodes(currentNode, map);

            foreach (IsometricTile neighborNode in neighborNodes)
            {
                if(!neighborNode.isWalkable || closedList.Contains(neighborNode))
                {
                    continue;
                }

                neighborNode.G = GetManhattenDistance(start, neighborNode);
                neighborNode.H = GetManhattenDistance(end, neighborNode);

                neighborNode.previous = currentNode;

                if (!openList.Contains(neighborNode))
                {
                    openList.Add(neighborNode);
                }
            }
        }

        return new List<IsometricTile>();
    }

    private List<IsometricTile> GetFinishedList(IsometricTile start, IsometricTile end)
    {
        List<IsometricTile> finishedList = new List<IsometricTile>();

        IsometricTile currentNode = end;

        while(currentNode != start)
        {
            finishedList.Add(currentNode);
            currentNode = currentNode.previous;
        }

        finishedList.Reverse();

        return finishedList;
    }

    private int GetManhattenDistance(IsometricTile start, IsometricTile neighborNode)
    {
        return Math.Abs(start.pos.x - neighborNode.pos.x) + Math.Abs(start.pos.y - neighborNode.pos.y);
    }

    private List<IsometricTile> GetNeighbourNodes(IsometricTile currentNode, MapManager map)
    {
        List<IsometricTile> neighbourNodes = new List<IsometricTile>();

        Vector2Int locationToCheck =new Vector2Int( currentNode.pos.x, currentNode.pos.y + 1);

        if (map.Nodes.ContainsKey(locationToCheck))
        {
            neighbourNodes.Add(map.Nodes[locationToCheck]);
        }

        locationToCheck = new Vector2Int(currentNode.pos.x, currentNode.pos.y - 1);

        if (map.Nodes.ContainsKey(locationToCheck))
        {
            neighbourNodes.Add(map.Nodes[locationToCheck]);
        }

        locationToCheck = new Vector2Int(currentNode.pos.x + 1, currentNode.pos.y);

        if (map.Nodes.ContainsKey(locationToCheck))
        {
            neighbourNodes.Add(map.Nodes[locationToCheck]);
        }

        locationToCheck = new Vector2Int(currentNode.pos.x - 1, currentNode.pos.y);

        if (map.Nodes.ContainsKey(locationToCheck))
        {
            neighbourNodes.Add(map.Nodes[locationToCheck]);
        }
        
        return neighbourNodes;
    }
}
