using UnityEngine;

namespace Scripts.Ball
{
    public class BallController
    {
        private BallView _ballView;
        private BallView _ballPrefabView;
        private Transform _rootTrack;
        private Direction _directionMove = Direction.Idle;
        private float _speed;

        public Vector3 Position { get { return _ballView.transform.position; } }
        public Vector3 LocalPosition { get { return _ballView.transform.localPosition; } }

        public void SwapDirection()
        {
            _directionMove = Direction.Rigth == _directionMove ? Direction.Left : Direction.Rigth;
        }

        public void Move(float deltaTime)
        {
            var localPosition = LocalPosition;
            if (_directionMove == Direction.Rigth)
                localPosition.x += deltaTime * _speed;

            if (_directionMove == Direction.Left)
                localPosition.y += deltaTime * _speed;

            SetLocalPosition(localPosition);
        }

        public BallController(BallView ballPrefabView, Transform rootTrack)
        {
            _ballPrefabView = ballPrefabView;
            _rootTrack = rootTrack;
        }

        public void Init(float speed, Direction direction)
        {
            _ballView = GameObject.Instantiate(_ballPrefabView, _rootTrack);
            _ballView.SetColor(Color.red);

            _directionMove = direction;
            _speed = speed;
        }

        public void SetLocalPosition(Vector3 position)
        {
            _ballView.transform.localPosition = position;
        }

        public void Hide()
        {
            _directionMove = Direction.Idle;
            _ballView.Hide();
        }

        public void Show()
        {
            _ballView.Show();
            _ballView.SetColor(Color.red);
            _ballView.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }
}