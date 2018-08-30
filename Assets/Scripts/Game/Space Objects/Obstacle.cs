using Engine;
using Game.Projectiles;
using UnityEngine;

namespace Game.SpaceObjects
{
    public class Obstacle : ActiveObject
    {
        public bool Stopped;
        public bool EnabledTurret = true;
        [SerializeField] private Transform turretSlot;
        private bool collidable = true;
        private bool rotating = false;
        

        public override void Act()
        {
            if (rotating)
            {
                transform.localRotation *= Quaternion.Euler(0,0, 2*GameLoop.NormalizedDeltaTime );
            }
        }

        public void SetRandomRotation()
        {
            transform.localRotation = Quaternion.Euler(0,0,Random.Range(-180,180));
            if (Random.Range(0, 100) > 60)
            {
                rotating = true;
            }
        }

        public void AddRandomRotation()
        {
            if (Random.Range(0, 100) > 60)
            {
                rotating = true;
            }
        }

        public Transform GetTurretPosition()
        {
            if (turretSlot.childCount != 0) return null;
            return turretSlot;
        }

        public void SetCollidable(bool collidable)
        {
            this.collidable = collidable;
        }

		void OnTriggerEnter2D(Collider2D other)
    	{
            if (!collidable) return;

    	    if (other.gameObject.CompareTag("Projectile"))
    	    {
    	        var projectile = other.GetComponent<Projectile>();
    	        projectile.Explode();
    	    }

            if (other.gameObject.CompareTag("Ship"))
    	    {
    	        var ship = other.GetComponent<Ship>();
    	        ship.Explode();
    	    }
    	}
    }
}