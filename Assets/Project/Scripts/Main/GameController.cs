using Scripts.Ball;
using Scripts.Cell;
using Scripts.Crystal;
using UnityEngine;

namespace Scripts.Main
{
    public class GameController : MonoBehaviour
    {
        private const int COUNT_SPAWNED_LINES = 40;

        [SerializeField] private UIController _uIController;
        [SerializeField] private float _offsetYCamera;
        [SerializeField] private float _offsetZCamera;
        [SerializeField] private Transform _transformCamera;
        [SerializeField] private CellView _cellPrefab;
        [SerializeField] private BallView _ballPrefab;
        [SerializeField] private Transform _rootTrack;

        private PathController _pathController;
        private BallController _ball;

        private StateGame _stateGame;

        void Start()
        {
            _stateGame = StateGame.Pause;
            _uIController.ShowMenuStartGame();
            _uIController.OnStartGame += NewGame;
            _uIController.OnReStartGame += RestartGame;
        }

        private void NewGame(int speedBall, int widthPath, int algorithm)
        {
            _uIController.OnStartGame -= NewGame;
            var borderSreen = -Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -10)).x;

            _pathController = new PathController(_cellPrefab,
               algorithm == 0 ? (GetCrystals)new RandomAlgorithm() : (GetCrystals)new OrderAlgorithm()
                , _rootTrack, borderSreen);

            _pathController.Init(COUNT_SPAWNED_LINES, widthPath);
            _pathController.CollectedCrystal += _uIController.CollectedCrystal;

            _ball = new BallController(_ballPrefab, _rootTrack);
            _ball.Init(speedBall, Direction.Idle);
            _ball.SetLocalPosition(new Vector3(0, 0, 0));
            _stateGame = StateGame.WaitingGame;
        }

        private void RestartGame()
        {
            _pathController.Reset();
            _ball.SetLocalPosition(new Vector3(0, 0, 0));
            _ball.Show();
            _stateGame = StateGame.WaitingGame;
        }

        private void Update()
        {
            switch (_stateGame)
            {
                case StateGame.Game:
                    if (Input.GetMouseButtonDown(0))
                        _ball.SwapDirection();
                    _ball.Move(Time.deltaTime);
                    var checkEntry = _pathController.CheckEntry(_pathController.FirstCell, _ball.LocalPosition);
                    if (!checkEntry)
                    {
                        _ball.Hide();
                        _stateGame = StateGame.Pause;
                        _uIController.ShowMenuReStartGame();
                    }
                    _pathController.UpdateCellTrack();
                    break;
                case StateGame.WaitingGame:
                    if (Input.GetMouseButtonDown(0))
                    {
                        _uIController.HideInfoLabel();
                        _ball.SwapDirection();
                        _stateGame = StateGame.Game;
                    }
                    break;
            }
        }

        private void LateUpdate()
        {
            switch (_stateGame)
            {
                case StateGame.Game:
                case StateGame.WaitingGame:
                    var positionBall = _ball.Position;
                    positionBall.x = 0;
                    _transformCamera.localPosition = positionBall + new Vector3(0, _offsetYCamera, _offsetZCamera);
                    break;
            }
        }
    }
}