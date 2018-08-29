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
                direction = speed * (direction + acceleration).normalized  * Time.deltaTime;
            }
        }
    }
}