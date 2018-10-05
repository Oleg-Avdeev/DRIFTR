using UnityEngine;

namespace Game.Projectiles
{
    public class Bouncer : Projectile
    {
        [SerializeField] private int hitsCount = 5;

        protected override void Destroy()
        {
            hitsCount--;

            if (collisionNormal != Vector3.zero)
            {
                float angle = 2 * Vector3.Angle(direction, collisionNormal) - 180;
                if (Vector3.Cross(direction, collisionNormal).z < 0) angle = -angle;
                direction = Vector2Extension.Rotate(direction, angle);
                collisionNormal = Vector3.zero;
            }

            if (hitsCount <= 0)
            {
                base.Destroy();
            }
        }
    }
}