using System.Collections.Generic;
using Engine;
using UnityEngine;

namespace Game.Projectiles
{
    public class Bomb : Projectile
    {
        [SerializeField] private Projectile shrapnelPrefab;
        private bool exploded;

        public override void Explode()
        {
            if (exploded)
            {
                base.Destroy();
                return;
            }

            transform.localPosition -= direction * GameLoop.NormalizedDeltaTime;

            for (int i = 0; i < 10; i++)
            {
                direction = Quaternion.Euler(0,0,36) * direction;
                var shrapnel = Create(shrapnelPrefab);
                shrapnel.SetValues(direction, fraction, target);
                projectileList.Add(shrapnel);
            }
            
            exploded = true;
            deathTime += lifespan;
            speed = 0;
            gameObject.SetActive(false);
            this.Create(explosionPrefab).Initialize();
        }
    }
}