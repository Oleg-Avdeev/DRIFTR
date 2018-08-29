using UnityEngine;

namespace Game.Projectiles
{
    public class Projectile : ActiveObject
    {
        [SerializeField] private Explosion explosionPrefab;
        [SerializeField] protected float speed = 10;
        [SerializeField] private float lifespan;

        protected Vector3 direction;
        protected Fraction fraction;
        protected Transform target;


        public void SetValues(Vector3 direction, Fraction fraction, Transform target)
        {
            direction = speed * direction.normalized;
            float angle = Vector3.Angle(Vector3.up, direction) - 90;
            if (direction.x > 0) angle = 360 - angle;
            transform.localRotation = Quaternion.Euler(0, 0, angle);

            this.direction = direction;
            this.fraction = fraction;
            this.target = target;
        }

        protected virtual void LogicUpdate() {}

        public override void Update()
        {
            LogicUpdate();
            transform.localPosition += direction * Time.timeScale * Time.deltaTime;
            lifespan -= Time.deltaTime;
            if (lifespan <= 0)
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