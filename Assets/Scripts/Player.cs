using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Funny.Mechanic
{
    public class Player : MonoBehaviour
    {

        LinkedListNode<Tile> target;
        Tile _currentTile;
        Tile _prevTile;
        Tile _startChangePathFrom;
        [SerializeField] float speed = 4;
        Vector3 _randomDirection = Vector3.right;
        [SerializeField] private PlayerStatus _playerStatus;

        private void Start()
        {
            target = Land.Instance.PathTiles.First;
        }
        private void Update()
        {
            if (Input.anyKeyDown)
            {
                if(_playerStatus == PlayerStatus.WalkOnInner)
                {
                    _randomDirection = Vector3.back;
                    return;
                }
                _startChangePathFrom = _currentTile;
                _playerStatus = PlayerStatus.WalkOnInner;
                return;
            }
            switch (_playerStatus)
            {
                case PlayerStatus.walkOnEdge:
                    if (Vector3.Distance(target.Value.transform.position.X0Z(), transform.position) < 0.1f)
                    {
                        if (target.Next == null)
                            target = Land.Instance.PathTiles.First;
                        else
                            target = target.Next;
                    }
                    transform.position = Vector3.MoveTowards(transform.position, target.Value.transform.position.X0Z(), Time.deltaTime * speed);

                    break;
                case PlayerStatus.WalkOnInner:
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + _randomDirection, Time.deltaTime * speed);

                    break;
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            _prevTile = _currentTile;
            _currentTile = other.GetComponent<Tile>();

            switch (_currentTile.Position)
            {
                case TilePosition.inner:
                    if (_playerStatus == PlayerStatus.walkOnEdge)
                    {
                        Debug.Log($"Path new branch created from {_currentTile.name}");
                        _startChangePathFrom = _prevTile;
                    }
                    Land.Instance.BufferTiles(_currentTile);

                    _currentTile.MakeDirty();
                    _playerStatus = PlayerStatus.WalkOnInner;

                    break;
                case TilePosition.edge:
                    if (_playerStatus == PlayerStatus.WalkOnInner)
                    {
                        Debug.Log("Remark Edge");
                        Debug.Log("Clean Inner Tiles");
                        Debug.Log("Deactivate Outer Tiles");
                        Land.Instance.AddBufferedTilesToPath(_startChangePathFrom,_currentTile);
                        target = Land.Instance.currentPositionOnPath;
                        _randomDirection = Vector3.right;
                    }
                    _playerStatus = PlayerStatus.walkOnEdge;

                    break;
                case TilePosition.dirty:
                    _playerStatus = PlayerStatus.WalkOnDirty;

                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    break;
            }

        }
    }
}