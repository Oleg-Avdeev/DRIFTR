using UnityEngine;

namespace Game.Projectiles
{
    public class Phaser : Projectile
    {
        [SerializeField] private int hitsCount = 2;

        protected override void Destroy()
        {
            hitsCount--;
            if (hitsCount <= 0)
            {
                base.Destroy();
            }
        }
    }
}