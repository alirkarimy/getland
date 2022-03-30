using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funny.Mechanic
{
    public class Tile : MonoBehaviour, IComparable<Tile>
    {
        [SerializeField] private TilePosition _position = TilePosition.inner;
        [SerializeField] private TileStatus _status = TileStatus.active;
        [SerializeField] private int _id = -1;
        internal TilePosition Position => _position;
        internal TileStatus Status => _status;


        public int ID => _id;

        MeshRenderer _meshRenderer;


        internal void Init(int id)
        {
            _id = id;
        }
        internal void MarkAs(TilePosition tilePosition)
        {
            _position = tilePosition;
        }

        internal void PaintAsEdge()
        {

            _position = TilePosition.edge;
            if (_meshRenderer == null)
                _meshRenderer = GetComponent<MeshRenderer>();

            _meshRenderer.material.color = Color.yellow;
        }

        internal void MakeDirty()
        {
            _position = TilePosition.dirty;
            if (_meshRenderer == null)
                _meshRenderer = GetComponent<MeshRenderer>();

            _meshRenderer.material.color = Color.red;
        }

        internal void Dominate()
        {
            _position = TilePosition.dirty;
            _status = TileStatus.deactive;

            if (_meshRenderer == null)
                _meshRenderer = GetComponent<MeshRenderer>();

            _meshRenderer.material.color = Color.red;

            Ray ray;
            RaycastHit[] hits;
            ray = new Ray(transform.position, Vector3.right);
            hits = Physics.RaycastAll(ray, 1);

            if (hits.Length == 0) return;

            Debug.Log($"{name} dominating {hits[0].collider}");

            if (hits[0].collider.GetComponent<Tile>().Position != TilePosition.edge)
            {
                hits[0].collider.GetComponent<Tile>().Dominate();
            }

        }

        internal void MakeClean()
        {
            _position = TilePosition.edge;
            if (_meshRenderer == null)
                _meshRenderer = GetComponent<MeshRenderer>();

            _meshRenderer.material.color = Color.yellow;
        }

        public int CompareTo(Tile other)
        {
            if (other.ID == ID) return 1;
            else return 0;
        }


    }
}