using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Pathfinding
{
    public static List<NavigationNode> FindPath(NavigationNode start, NavigationNode end)
    {
        List<NavigationNode> toSearch = new();
        List<NavigationNode> processed = new();
        toSearch.Add(start);

        while (toSearch.Count > 0)
        {
            NavigationNode current = toSearch[0];
            foreach(NavigationNode node in toSearch)
            {
                if(node.Score < current.Score || (node.Score == current.Score && node.StepsToEnd < current.StepsToEnd))
                    current = node;
            }

            processed.Add(current);
            toSearch.Remove(current);

            if(current == end)
            {
                NavigationNode currentPathNode = end;
                List<NavigationNode> path = new();
                var count = 170;
                while (currentPathNode != start)
                {
                    path.Add(currentPathNode);
                    currentPathNode = currentPathNode.Connection;
                    count--;
                    if (count < 0) return null;
                }

                //foreach(NavigationNode node in path) node.Tile.GetComponent<SpriteRenderer>().color = Color.red; ////////////////////////////
                //start.Tile.GetComponent<SpriteRenderer>().color = Color.red;
                //Debug.Log(path.Count);
                return path;
            }

            foreach(NavigationNode neighbor in current.Neighbors.Where(n => n.Tile.IsWalkable && !processed.Contains(n)))
            {
               // neighbor.Tile.GetComponent<SpriteRenderer>().color = Color.green; ////////////////////////////////////////////////////////
                bool inSearch  = toSearch.Contains(neighbor);
                int cost = current.StepsFromStart + NavigationNode.GetDistanceTo(current, neighbor);

                if (!inSearch || cost < neighbor.StepsFromStart)
                {
                    neighbor.SetStepsFromStart(cost);
                    neighbor.SetConnection(current);

                    if (!inSearch)
                    {
                        neighbor.SetStepsToEnd(NavigationNode.GetDistanceTo(neighbor, end));
                        toSearch.Add(neighbor);
                    }
                }
            }
        }


        return null;
    }
}
