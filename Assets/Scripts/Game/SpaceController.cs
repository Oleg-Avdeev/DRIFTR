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
        [SerializeField] private BossTurret[] bossTurretPrefabs;
        [SerializeField] private SphereMap sphearMapPrefab;
        [SerializeField] private TimeScaleEffect timeScaleEffect;
        [SerializeField] private BossBattleEffect bossBattleEffect;

        private PlayerShip player;
        private List<Turret> enemies;
        private SphereMap map;
        private BossTurret boss;

        private bool finishGame;
        private float endTimer;
        private int killedEnemies;
        private List<Ship> playerList;
        private bool bossBattle;
        private int currentLevel = 0;

        private Turret chooseRandomTurret()
        {
            return turretPrefabs[Random.Range(currentLevel, currentLevel + 3)];
        }

        public override void Initialize()
        {
            player = Instantiate(playerShipPrefab, Vector3.zero, Quaternion.identity, transform) as PlayerShip;
            Camera.main.GetComponent<CameraFollow>().setTarget(player.transform);
            player.OnDestroyed += () => { 
                finishGame = true;
                endTimer = Time.fixedTime + 1;
                timeScaleEffect.Deactivate();
            };

            map = Instantiate(sphearMapPrefab, Vector3.right*10, Quaternion.identity, transform);
            map.Initialize();
            var turretPositions = map.SelectTurretPositions(100);
            playerList = new List<Ship>() { player };

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
            GameController.Multiplier *= 1.1f;
            killedEnemies++;

            if (killedEnemies >= 8 && currentLevel == 0 && !bossBattle)
            {
               StartBossBattle(bossTurretPrefabs[0]);
            }

            if (killedEnemies >= 20 && currentLevel == 1 && !bossBattle)
            {
               StartBossBattle(bossTurretPrefabs[1]);
            }
        }

        private void StartBossBattle(BossTurret bossPrefab)
        {
            boss = Instantiate(bossPrefab, Vector3.zero, Quaternion.identity, transform);
            boss.InitializeBoss(playerList, HandleEnemyDestruction);
            player.PointToBoss(boss.transform);
            bossBattleEffect.Activate();
            map.SetBossBattle(true);
            bossBattle = true;
            
            boss.OnDefeated += () => {
                currentLevel++;

                bossBattleEffect.Deactivate();
                map.SetBossBattle(false);
                bossBattle = false;    
                player.StopPointingToBoss();
                player.LevelUp(currentLevel);

                map.AddRandomRotation();

                var turretPositions = map.SelectTurretPositions(100);
                for (int i = 0; i < turretPositions.Count; i++)
                {
                    enemies.Add(Instantiate(chooseRandomTurret(), turretPositions[i], Quaternion.identity, transform));
                    enemies[enemies.Count - 1].UpdateShipList(playerList);
                    enemies[enemies.Count - 1].OnDestroyed += HandleEnemyDestruction;
                }
            };
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
            if (boss) boss.Update();
            
            if (!bossBattle)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i]) enemies[i].Update();
                }
            }
            
            timeScaleEffect?.Update();
            
        }

        
    }
}