using System.Collections.Generic;
using Game.Projectiles;
using UnityEngine;
using System;
using Engine;

namespace Game
{
    public class BossTurret : ActiveObject
    {
        [SerializeField] private List<Turret> subTurrets;
        [SerializeField] private float angleSpeed = 1;

        public event Action OnDefeated;
        private Quaternion rotationMatrix;
        private int defeatedTurrets = 0;

        public void SetProjectileList(List<Projectile> projectileList)
        {
            for (int i = 0; i < subTurrets.Count; i++)
            {
                subTurrets[i].SetProjectileList(projectileList);
            }
        }

        public void InitializeBoss(List<Ship> ships, Action DestroyHandler)
        {
            rotationMatrix = Quaternion.Euler(0,0,3*angleSpeed*GameLoop.NormalizedDeltaTime);

            for (int i = 0; i < subTurrets.Count; i++)
            {
                subTurrets[i].OnDestroyed += DestroyHandler;
                subTurrets[i].OnDestroyed += CountDestroyedTurrets;
                subTurrets[i].UpdateShipList(ships);
            }
        }

        private void CountDestroyedTurrets()
        {
            defeatedTurrets++;
            if (defeatedTurrets >= subTurrets.Count)
            {
                OnDefeated?.Invoke();
            }
        }

        public override void Act()
        {
            transform.localRotation *= rotationMatrix;
            for (int i = 0; i < subTurrets.Count; i++)
            {
                if (subTurrets[i]) subTurrets[i].Act();
            }
        }

    }
}