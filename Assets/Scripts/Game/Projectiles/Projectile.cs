using System.Collections.Generic;
using Engine;
using UnityEngine;

namespace Game.Projectiles
{
    public class Projectile : ActiveObject
    {
        [SerializeField] protected Explosion explosionPrefab;
        [SerializeField] protected float speed = 10;
        [SerializeField] protected float lifespan;

        protected Vector3 direction;
        protected Fraction fraction;
        protected Transform target;
        protected float deathTime;
        protected List<Projectile> projectileList;

        public void SetProjectileList(List<Projectile> list)
        {
            projectileList = list;
        }

        public void SetValues(Vector3 direction, Fraction fraction, Transform target)
        {
            direction = speed * direction.normalized;
            float angle = Vector3.Angle(Vector3.up, direction) - 90;
            if (direction.x > 0) angle = 360 - angle;
            transform.localRotation = Quaternion.Euler(0, 0, angle);

            this.direction = direction;
            this.fraction = fraction;
            this.target = target;

            deathTime = Time.fixedTime + lifespan;
        }

        protected virtual void LogicUpdate() {}

        public override void Act()
        {
            LogicUpdate();
            transform.localPosition += 4 * direction * GameLoop.NormalizedDeltaTime;

            if (deathTime <= Time.fixedTime)
                Explode();
        }

        public virtual void Explode()
        {
            this.Create(explosionPrefab).Initialize();
            Destroy();
        }

        protected virtual void Destroy()
        {
            GameObject.Destroy(gameObject);
        }

        public Fraction GetFraction()
        {
            return fraction;
        }
    }
}