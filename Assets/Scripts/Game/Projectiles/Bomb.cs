using UnityEngine;

namespace Game.Projectiles
{
    public class Bomb : Projectile
    {
        [SerializeField] private Projectile shrapnelPrefab;

        public override void Explode()
        {
            transform.localPosition -= direction * Time.timeScale * Time.deltaTime;

            for (int i = 0; i < 10; i++)
            {
                direction = Quaternion.Euler(0,0,36) * direction;
                Create(shrapnelPrefab).SetValues(direction, fraction, target);
            }
            
            base.Explode();
        }
    }
}