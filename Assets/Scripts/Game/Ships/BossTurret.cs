using System.Collections.Generic;
using Game.Projectiles;
using UnityEngine;
using System;

namespace Game
{
    public class BossTurret : ActiveObject
    {

        public event Action OnDefeated;

        [SerializeField] private List<Turret> subTurrets;
        private readonly Quaternion rotationMatrix = Quaternion.Euler(0,0,1);
        private int defeatedTurrets = 0;

        public void InitializeBoss(List<Ship> ships, Action DestroyHandler)
        {
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

        public override void Update()
        {
            transform.localRotation *= rotationMatrix;
            for (int i = 0; i < subTurrets.Count; i++)
            {
                if (subTurrets[i]) subTurrets[i].Update();
            }
        }

    }
}