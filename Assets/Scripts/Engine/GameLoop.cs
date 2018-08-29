using UnityEngine;

namespace Engine
{
    public class GameLoop : MonoBehaviour
    {
        public static float NormalizedDeltaTime;
        [SerializeField] private MonoBehaviour gameControllerPrefab;
        [SerializeField] private float deltaTimeScaleFactor = 50;

        Controller rootGameController;

        void Awake()
        {
            rootGameController = Instantiate(gameControllerPrefab, Vector3.zero, Quaternion.identity, transform) as Controller;
            if (rootGameController != null)
            {
                rootGameController.Initialize();
            }
        }

        void Update()
        {
            NormalizedDeltaTime = Time.timeScale * Time.deltaTime * deltaTimeScaleFactor;
            rootGameController?.Act();
        }

    }
}