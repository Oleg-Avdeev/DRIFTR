using System.Collections.Generic;
using Game.SpaceObjects;
using Game.Effects;
using UnityEngine;
using Engine;

namespace Game
{
    public class SpaceController : Controller
    {
        public event System.Action OnGameEnded;

        [SerializeField] private Ship playerShipPrefab;
        [SerializeField] private Turret[] turretPrefabs;
        [SerializeField] private SphereMap sphearMapPrefab;
        [SerializeField] private TimeScaleEffect timeScaleEffect;

        private Ship player;
        private List<Turret> enemies;
        private SphereMap map;

        private bool finishGame;
        private float endTimer;

        private Turret chooseRandomTurret()
        {
            return turretPrefabs[Random.Range(0, turretPrefabs.Length)];
        }

        public override void Initialize()
        {
            player = Instantiate(playerShipPrefab, Vector3.zero, Quaternion.identity, transform);
            Camera.main.GetComponent<CameraFollow>().setTarget(player.transform);
            player.OnDestroyed += () => { 
                finishGame = true;
                endTimer = Time.fixedTime + 3;
            };

            map = Instantiate(sphearMapPrefab, Vector3.right*10, Quaternion.identity, transform);
            map.Initialize();
            var turretPositions = map.SelectTurretPositions(100);
            var playerList = new List<Ship>() { player };

            enemies = new List<Turret>();
            for (int i = 0; i < turretPositions.Count; i++)
            {
                enemies.Add(Instantiate(chooseRandomTurret(), turretPositions[i], Quaternion.identity, transform));
                enemies[i].UpdateShipList(playerList);
                enemies[i].OnDestroyed += HandleEnemyDestruction;
            }

            timeScaleEffect?.Initialize();
        }

        private void HandleEnemyDestruction()
        {
            timeScaleEffect?.Activate();
            GameController.AddPoints(1000);
        }

        public override void Update()
        {
            if (finishGame)
            {
                if (Time.fixedTime > endTimer)
                {
                    OnGameEnded?.Invoke();
                }
            }

            map.Update();
            if (player) player.Update();
            
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i]) enemies[i].Update();
            }
            
            timeScaleEffect?.Update();
        }

        
    }
}