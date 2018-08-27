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

		private List<Projectile> projectileList = new List<Projectile>();
		private float shootLock = 0;

        protected Vector3 acceleration = Vector3.zero;
        protected Vector3 speed = Vector3.right / 100;

        protected virtual void UpdateState() { }

        public override void Update()
        {	
            UpdateState();
			speed += acceleration;
			speed = speed.normalized * Mathf.Min(speed.magnitude, maxSpeed);
            transform.localPosition += speed * Time.timeScale;

			float angle = Vector3.Angle(Vector3.up, acceleration);
            if (acceleration.x > 0) angle = 360 - angle;
            transform.localRotation = Quaternion.Euler(0, 0, angle);

			for (int i = 0; i < projectileList.Count; i++)
			{
				if (projectileList[i]) projectileList[i].Update();
			}
        }

		public void Explode()
		{
			Create(explosionPrefab).Initialize();
			OnDestroyed?.Invoke();
			Destroy(gameObject);
		}

		protected void Shoot(Vector3 direction)
		{
			if (shootLock < Time.fixedTime)
			{
				var projectile = Create(mainProjectile);
				projectile.SetValues(direction, fraction);
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
    	}
    }
}
