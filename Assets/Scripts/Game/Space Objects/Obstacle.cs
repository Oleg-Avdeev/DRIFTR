using Game.Projectiles;
using UnityEngine;

namespace Game.SpaceObjects
{
    public class Obstacle : ActiveObject
    {
        public bool Stopped;
        public bool EnabledTurret = true;
        [SerializeField] private Transform turretSlot;

        public override void Update()
        {
            
        }

        public void SetRandomRotation()
        {
            transform.localRotation = Quaternion.Euler(0,0,Random.Range(-180,180));
        }

        public Vector3 GetTurretPosition()
        {
            return turretSlot.position;
        }

		void OnTriggerEnter2D(Collider2D other)
    	{
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