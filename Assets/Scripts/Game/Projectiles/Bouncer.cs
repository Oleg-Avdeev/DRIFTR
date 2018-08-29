using UnityEngine;

namespace Game.Projectiles
{
    public class Bouncer : Projectile
    {
        [SerializeField] private int hitsCount = 5;

        protected override void Destroy()
        {
            hitsCount--;

            // direction =

            if (hitsCount <= 0)
            {
                base.Destroy();
            }
        }
    }
}