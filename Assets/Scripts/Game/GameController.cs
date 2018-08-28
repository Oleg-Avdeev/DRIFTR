using UnityEngine;
using Engine;
using UI;

namespace Game
{
    public class GameController : Controller
    {   
        public static Camera MainCamera;
        public static float Multiplier = 1f;
        public static long Score 
        {
            get { return score; }
        }
        public static void AddPoints(long points)
        {
            score = score + (long)(points * Multiplier);
        }
        private static long score = 0;

        [SerializeField] private SpaceController spaceControllerPrefab;
        [SerializeField] private GameScreen gameScreenPrefab;
        
        private SpaceController spaceController;
        private GameScreen gameScreen;
        
        public override void Initialize()
        {   
            MainCamera = Camera.main;
            gameScreen = Instantiate(gameScreenPrefab, Vector3.zero, Quaternion.identity, transform.parent);
            gameScreen.OnGameStarted += StartGame;
        }

        public void StartGame()
        {
            score = 0;
            Multiplier = 1f;
            Time.timeScale = 1f;
            spaceController = Instantiate(spaceControllerPrefab, Vector3.zero, Quaternion.identity, transform);
            spaceController.OnGameEnded += EndGame;
            spaceController?.Initialize();
        }

        public void EndGame()
        {
            Destroy(spaceController.gameObject);
            gameScreen.ShowEndGame();
            spaceController = null;
        }

        public override void Update()
        {
            spaceController?.Update();
            gameScreen?.Update();
        }
    }
}