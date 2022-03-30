using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funny.Mechanic
{
    public class Land : MonoBehaviour
    {
        public static Land Instance;
       
        [SerializeField] private Tile[] _allTiles; 
        [SerializeField] private List<Tile> _edgeTiles = new List<Tile>();
        [SerializeField] private Stack<Tile> _bufferTiles = new Stack<Tile>();
        internal LinkedList<Tile> PathTiles = new LinkedList<Tile>();
        internal LinkedListNode<Tile> currentPositionOnPath;

       
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            for (int i = 0; i < _allTiles.Length; i++)
            {
                _allTiles[i].Init(i);
            }
            CreatePath();
        }


        private void CreatePath()
        {
            currentPositionOnPath = PathTiles.AddFirst(_edgeTiles[0]);
            for (int i = 1; i < _edgeTiles.Count; i++)
            {
                currentPositionOnPath = PathTiles.AddAfter(currentPositionOnPath, _edgeTiles[i]);
            }
        }

        internal void AddBufferedTilesToPath(Tile changeNode,Tile newCollisionNode)
        {
            List<Tile> oldNodes = new List<Tile>();
            PathTiles.ReplaceTilesBetween(changeNode, newCollisionNode, _bufferTiles,ref oldNodes);
            _bufferTiles.Clear();
            for (int i = 0; i < _allTiles.Length; i++)
            {
                if (_allTiles[i].Position == TilePosition.dirty && _allTiles[i].Status == TileStatus.active)
                    _allTiles[i].MakeClean();

            }
            currentPositionOnPath = PathTiles.GetNode(newCollisionNode);

            for (int i = 0; i < oldNodes.Count; i++)
            {
                oldNodes[i].Dominate();
            }
        }

        internal void BufferTiles(Tile currentTile)
        {            
            _bufferTiles.Push(currentTile);
        }
    }

}