using UnityEngine;

namespace Engine
{
    public class GameLoop : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour gameControllerPrefab;

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
            rootGameController?.Update();
        }

    }
}