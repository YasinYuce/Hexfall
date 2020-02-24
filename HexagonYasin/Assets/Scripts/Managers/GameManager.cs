using UnityEngine;

namespace YasinYuce.HexagonYasin
{
    public class GameManager : MonoBehaviour
    {
        public GridManager GridManager;
        public SelectorManager SelectorManager;
        public Stats Stats;
        public InputManager InputManager;
        public UIManager UIManager;
        public ParticleManager ParticleManager;

        public GameMode CurrentGameMode;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            GridManager.Initialize(this, SelectorManager, Stats, ParticleManager);
            SelectorManager.Initialize(this, GridManager, InputManager, SelectorManager, Stats.MoveCount);
            Stats.Initialize(this);
            ParticleManager.Initialize();
            InputManager.Initialize(SelectorManager);
            BombPieceManager.Initialize(this, GridManager);
            ExplosionSystem.Initialize(this, Stats.Score, ParticleManager, GridManager);
        }

        private void Start()
        {
            StartGame();
        }

        /// <summary>
        /// The main func that Sets level requirements and Starts the game.
        /// </summary>
        public void StartGame()
        {
            Stats.StartGame(CurrentGameMode.ScoreMultiplier);
            UIManager.StartGame();
            Color[] colors = CurrentGameMode.Colors.ToArray();
            ParticleManager.StartGame(colors);
            GridManager.StartGame(CurrentGameMode.GridXLength, CurrentGameMode.GridYLength, CurrentGameMode.BombScoreLimit, colors);
            SelectorManager.StartGame(GridManager.oneSideScale, CurrentGameMode.GridElements);
            ExplosionSystem.StartGame(CurrentGameMode.ExplosionTypes, CurrentGameMode.GridYLength);
            InputManager.IsReadyForInput = true;
        }

        /// <summary>
        /// The main func that overs the game.
        /// </summary>
        public void GameOver()
        {
            UIManager.GameOver();
            InputManager.IsReadyForInput = false;
        }

        public void GameOverChecks()
        {
            if (GridManager.IsAnyOtherMoveExist() && !BombPieceManager.IsBombPieceExploded(Stats.MoveCount.CurrentMoveCount))
            {
                SelectorManager.CurrentSelectorObject.SetActive(true);
                InputManager.IsReadyForInput = true;
            }
        }
    }
}