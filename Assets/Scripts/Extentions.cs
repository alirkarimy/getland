using Funny.Mechanic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extentions
{
    public static Vector3 X0Z(this Vector3 vector3)
    {
        vector3.y = 0;
        return vector3;
    }

    public static LinkedListNode<T> GetNode<T>(this LinkedList<T> linkedList, T instance) where T : IComparable<T>
    {
        LinkedListNode<T> temp = linkedList.First;
        Debug.Log($"other is null : {instance == null}");
        while (temp.Next != null)
        {
            if (temp.Value.CompareTo(instance) == 1)
                return temp;
            else
                temp = temp.Next;

        }
        return null;

    }
    public static LinkedList<Tile> ReplaceTilesBetween(this LinkedList<Tile> list, Tile from, Tile to, Stack<Tile> newNodes,ref List<Tile> oldNodes) 
    {
        LinkedListNode<Tile> startNode = list.GetNode(from);

        //validate inputs
        while(startNode.Next!=null && startNode.Next.Value.CompareTo(to) != 1)
        {
            oldNodes.Add(startNode.Next.Value);
            list.Remove(startNode.Next);
        }

        while(newNodes.Count > 0)
        {
            list.AddAfter(startNode, newNodes.Pop());
        }
      
        return list;
    }
}
