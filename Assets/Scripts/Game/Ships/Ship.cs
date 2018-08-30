using System.Collections.Generic;
using Game.Projectiles;
using UnityEngine;

namespace Game
{
	public enum Fraction
	{
		Player,
		Enenmy
	}

    public class Ship : ActiveObject
    {
		public event System.Action OnDestroyed;

        [SerializeField] protected Fraction fraction;
        [SerializeField] protected Explosion explosionPrefab;
        [SerializeField] protected Projectile mainProjectile;
        [SerializeField] protected float weaponCooldown;
        [SerializeField] protected float maxAcceleration;
        [SerializeField] protected float maxSpeed;

		private List<Projectile> projectileList;
		private float shootLock = 0;
		private Collider2D collider;

        protected Vector3 acceleration = Vector3.zero;
        protected Vector3 speed = Vector3.right / 100;

        protected virtual void UpdateState() { }

		public void SetProjectileList(List<Projectile> list)
		{
			projectileList = list;
		}

		public override void Initialize()
		{
			collider = GetComponent<Collider2D>();
		}

        public override void Act()
        {	
            UpdateState();
			speed += 4 * acceleration * Engine.GameLoop.NormalizedDeltaTime;
			speed = speed.normalized * Mathf.Min(speed.magnitude, maxSpeed);
            transform.localPosition += speed * 4 * Engine.GameLoop.NormalizedDeltaTime;

			float angle = Vector3.Angle(Vector3.up, acceleration);
            if (acceleration.x > 0) angle = 360 - angle;
            transform.localRotation = Quaternion.Euler(0, 0, angle);
        }

		public void DisableCollisions()
		{
			collider.enabled = false;
		}

		public void ReenableCollisions()
		{
			collider.enabled = true;
		}

		public void Explode()
		{
			Create(explosionPrefab).Initialize();
			OnDestroyed?.Invoke();
			Destroy(gameObject);
		}

		protected void Shoot(Vector3 direction, Transform target = null)
		{
			if (shootLock < Time.fixedTime)
			{
				var projectile = Create(mainProjectile);
				projectile.SetValues(direction, fraction, target);
				projectile.SetProjectileList(projectileList);
				projectileList.Add(projectile);
				shootLock = Time.fixedTime + weaponCooldown;
			}
		}

		protected virtual void OnTriggerEnter2D(Collider2D other)
    	{
    	    if (other.gameObject.CompareTag("Projectile"))
    	    {
    	        Projectile projectile = other.GetComponent<Projectile>();
    	        if (projectile.GetFraction() != fraction)
    	        {
    	            projectile.Explode();
					Explode();
    	        }
    	    }
			else if (other.gameObject.CompareTag("Ship"))
    	    {
    	        PlayerShip ship = other.GetComponent<PlayerShip>();
				if (ship != null)
				{
					Explode();
				}
			}
    	}
    }
}
