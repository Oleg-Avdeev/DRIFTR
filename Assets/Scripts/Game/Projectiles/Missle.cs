using Engine;
using UnityEngine;

namespace Game.Projectiles
{
    public class Missle : Projectile
    {
        private Vector3 acceleration;
        protected override void LogicUpdate()
        {
            if (target)
            {
                acceleration = (target.localPosition - transform.localPosition);
                direction = 4 * speed * (direction + acceleration).normalized  * GameLoop.NormalizedDeltaTime;
            }
        }
    }
}