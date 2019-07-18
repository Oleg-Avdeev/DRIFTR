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
        private float angle;
        

        public override void Act()
        {
            if (rotating)
            {
                transform.localRotation *= Quaternion.Euler(0,0, angle*GameLoop.NormalizedDeltaTime );
            }
        }

        public void SetScale()
        {
            turretSlot.localScale = Vector3.one * (1f/transform.localScale.x);
        }
        
        public void SetRandomRotation()
        {
            transform.localRotation = Quaternion.Euler(0,0,Random.Range(-180,180));
            angle = Random.Range(-0.4f, 0.4f);
            if (Random.Range(0, 100) > 70)
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
            // this.collidable = collidable;
        }

        public void ShiftPosition(float sx, float sy)
        {
            transform.localPosition += new Vector3(sx, sy);
        }

		void OnTriggerEnter2D(Collider2D other)
    	{
            if (!collidable) return;

    	    if (other.gameObject.CompareTag("Projectile"))
    	    {
    	        var projectile = other.GetComponent<Projectile>();
                projectile.SetCollisionInfo(projectile.transform.position - transform.position);
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