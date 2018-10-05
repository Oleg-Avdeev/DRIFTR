using System.Collections.Generic;
using Game.SpaceObjects;
using Game.Effects;
using UnityEngine;
using Engine;
using Game.Projectiles;

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
        private List<Projectile> projectileList = new List<Projectile>();
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
            player.Initialize();
            player.SetProjectileList(projectileList);
            Camera.main.GetComponent<CameraFollow>().setTarget(player.transform);
            player.OnDestroyed += () => { 
                finishGame = true;
                endTimer = Time.fixedTime + 1;
                
                SFX.SoundEffectController.Instance.UnlockEffects();
                timeScaleEffect.Deactivate();
                SFX.SoundEffectController.Instance.SetTargetLowPass(500f);
            };

            map = Instantiate(sphearMapPrefab, Vector3.right*10, Quaternion.identity, transform);
            map.Initialize();
            map.SetPlayerTransform(player.transform);
            var turretPositions = map.SelectTurretPositions(100);
            playerList = new List<Ship>() { player };

            enemies = new List<Turret>();
            for (int i = 0; i < turretPositions.Count; i++)
            {
                enemies.Add(Instantiate(chooseRandomTurret(), turretPositions[i].position, Quaternion.identity, turretPositions[i]));
                enemies[i].Initialize();
                enemies[i].UpdateShipList(playerList);
                enemies[i].SetProjectileList(projectileList);
                enemies[i].OnDestroyed += HandleEnemyDestruction;
            }

            timeScaleEffect?.Initialize();
            bossBattleEffect?.Initialize();
        }

        private void HandleEnemyDestruction()
        {
            timeScaleEffect?.Activate();
            GameController.Multiplier += 2f;
            killedEnemies++;

            if (killedEnemies >= 8 && currentLevel == 0 && !bossBattle)
            {
               StartBossBattle(bossTurretPrefabs[0]);
            }

            if (killedEnemies >= 22 && currentLevel == 1 && !bossBattle)
            {
               StartBossBattle(bossTurretPrefabs[1]);
            }
        }

        private void StartBossBattle(BossTurret bossPrefab)
        {
            boss = Instantiate(bossPrefab, Vector3.zero, Quaternion.identity, transform);
            boss.InitializeBoss(playerList, HandleEnemyDestruction);
            boss.SetProjectileList(projectileList);
            player.PointToBoss(boss.transform);
            bossBattleEffect.Activate();
            map.SetBossBattle(true);
            bossBattle = true;

            for (int i = 0; i < enemies.Count; i++)
                if (enemies[i]) enemies[i].DisableCollisions();
            
            boss.OnDefeated += () => {
                
                GameController.Multiplier *= 2;

                currentLevel++;

                bossBattleEffect.Deactivate();
                map.SetBossBattle(false);
                bossBattle = false;    
                player.StopPointingToBoss();
                player.LevelUp(currentLevel);

                for (int i = 0; i < enemies.Count; i++)
                    if (enemies[i]) enemies[i].ReenableCollisions();

                map.AddRandomRotation();
                var turretPositions = map.SelectTurretPositions(65);
                for (int i = 0; i < turretPositions.Count; i++)
                {
                    var enemy = Instantiate(chooseRandomTurret(), turretPositions[i].position, Quaternion.identity, turretPositions[i]);
                    enemy.Initialize();
                    enemy.UpdateShipList(playerList);
                    enemy.OnDestroyed += HandleEnemyDestruction;
                    enemy.SetProjectileList(projectileList);
                    enemies.Add(enemy);
                }
            };
        }

        public override void Act()
        {
            if (finishGame)
            {
                if (Time.fixedTime > endTimer)
                {
                    OnGameEnded?.Invoke();
                }
            }

            if (player) player.Act();
            map.Act();

            var score = 0.1f*(1.5f - map.GetClosestPlanet());
            GameController.Multiplier += (Mathf.Max(0, score)) * GameLoop.NormalizedDeltaTime;

            if (boss) boss.Act();
            
            if (!bossBattle)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i]) enemies[i].Act();
                }
            }

			for (int i = 0; i < projectileList.Count; i++)
			{
				if (projectileList[i]) projectileList[i].Act();
			}
            
            timeScaleEffect?.Act();
            bossBattleEffect?.Act();
        }

        
    }
}