using Scripts.Ball;
using Scripts.Crystal;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Cell
{
    public class PathController
    {
        public event Action CollectedCrystal;
        public CellView FirstCell { get { return _first; } }

        private CellView _cellPrefab;
        private Transform _rootTrack;

        private int _widthPath;

        private CellView _first;
        private CellView _last;
        private List<CellView> _freeCells;
        private Position _lastPosition;
        private Direction _lastDirectionPath;
        private int _lengthSection;
        private float _borderSreen;
        private GetCrystals _algorithm;

        public PathController(CellView cellPrefab, GetCrystals algorithm, Transform rootTrack, float borderSreen)
        {
            _cellPrefab = cellPrefab;
            _rootTrack = rootTrack;
            _borderSreen = borderSreen;
            _algorithm = algorithm;
        }

        public void Init(int countSpawnedLines, int widthPath)
        {
            _algorithm.Init(5);
            _widthPath = widthPath;

            _first = null;
            _last = null;
            _freeCells = new List<CellView>(countSpawnedLines * _widthPath);

            for (int i = 0; i < countSpawnedLines * _widthPath; i++)
            {
                var cell = GameObject.Instantiate(_cellPrefab, _rootTrack);
                cell.Hide(true);
                _freeCells.Add(cell);
            }

            DrawStartSquare();          
            UpdateCellTrack();
        }

        public void Reset()
        {
            ResetCell(_first);
            _first = null;
            _last = null;
            DrawStartSquare();
            UpdateCellTrack();
        }

        private void ResetCell(CellView cell)
        {
            cell.Hide(true);
            _freeCells.Add(cell);
            if (cell.NextCell != null)
                ResetCell(cell.NextCell);
        }

        private void DrawStartSquare()
        {
            for (int i = 0; i < 3; i++)
                AddLineCell(new Position() { x = i - 1, y = -1 }, 3, Direction.Rigth, false);
            AddLineCell(new Position() { x = 2, y = 0 }, 3, Direction.Left, false);

            _lastDirectionPath = Direction.Rigth;
            _lastPosition = new Position() { x = 4, y = _widthPath > 2 ? -1 : 0 };
            _lengthSection = _widthPath;
        }

        public void UpdateCellTrack()
        {
            while (_freeCells.Count >= _widthPath)
            {
                var newDirection = _lastDirectionPath;
                if (_lengthSection <= 0)
                {
                    newDirection = (Direction)UnityEngine.Random.Range(0, 2);

                    if (TestOutScreen(newDirection))
                        newDirection = SwapDirection(newDirection);

                    _lengthSection = _widthPath;
                }
                _lengthSection--;

                var startPosition = CalcStartPosition(_lastPosition, newDirection, newDirection != _lastDirectionPath);
                AddLineCell(startPosition, _widthPath, newDirection, _algorithm.GetCrystal());
                _lastPosition = startPosition;
                _lastDirectionPath = newDirection;
            }

            if (!_first.gameObject.activeSelf)
            {
                _freeCells.Add(_first);
                _first = _first.NextCell;
            }
        }

        public bool CheckEntry(CellView cell, Vector3 checkingPosition)
        {
            var cellPosition = cell.transform.localPosition;
            if (TestCellEntry(checkingPosition, cellPosition))
            {
                if (cell.Cryslal)
                {
                    CollectedCrystal?.Invoke();
                    cell.ShowCrystal(false);
                }
                return true;
            }
            else
            {
                if (cellPosition.x + _widthPath < checkingPosition.x || cellPosition.y + _widthPath < checkingPosition.y)
                    cell.Hide();

                if (cell.NextCell != null)
                    return CheckEntry(cell.NextCell, checkingPosition);
                else
                    return false;
            }
        }

        private static bool TestCellEntry(Vector3 checkingPosition, Vector3 cellPosition)
        {
            return cellPosition.x - 0.5f < checkingPosition.x && cellPosition.x + 0.5f > checkingPosition.x &&
                            cellPosition.y - 0.5f < checkingPosition.y && cellPosition.y + 0.5f > checkingPosition.y;
        }

        private bool TestOutScreen(Direction newDirection)
        {
            return (_last.transform.position.x + _widthPath * 2 > _borderSreen && Direction.Rigth == newDirection)
                                 || (_last.transform.position.x - _widthPath * 2 < -_borderSreen && Direction.Left == newDirection);
        }

        private Direction SwapDirection(Direction direction)
        {
            if (direction == Direction.Rigth)
                return Direction.Left;
            else
                return Direction.Rigth;
        }

        private void AddLineCell(Position startPosition, int width, Direction direction, bool showCrystal)
        {
            int dx = 0;
            int dy = 0;
            int indexCrystal = showCrystal ? UnityEngine.Random.Range(0, width) : -1;
            for (int i = 0; i < width; i++)
            {
                if (direction == Direction.Rigth)
                    dy = i;
                else
                    dx = i;

                var cell = _freeCells[0];
                _freeCells.RemoveAt(0);
                cell.transform.localPosition = new Vector3(startPosition.x + dx, startPosition.y + dy, 0);
                cell.transform.SetAsLastSibling();
                cell.gameObject.name = $"x={startPosition.x + dx}; y={startPosition.y + dy}; d={direction}";
                cell.Show();
                cell.SetNext(null);
                cell.ShowCrystal(i == indexCrystal);
                if (_first == null)
                {
                    _first = cell;
                    _last = cell;
                }
                _last.SetNext(cell);
                _last = cell;
                _last.SetNext(null);
            }
        }

        private Position CalcStartPosition(Position lastPosition, Direction direction, bool newDirection)
        {
            if (direction == Direction.Rigth)
            {
                lastPosition.x++;
                if (newDirection)
                {
                    lastPosition.x--;
                    lastPosition.y++;
                }
            }
            else
            {
                lastPosition.y++;
                if (newDirection)
                {
                    lastPosition.x++;
                    lastPosition.y--;
                }
            }
            return lastPosition;
        }
    }
}