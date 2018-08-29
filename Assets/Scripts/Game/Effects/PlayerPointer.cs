using System.Collections.Generic;
using Game.Projectiles;
using UnityEngine;

namespace Game
{
    public class PlayerPointer : ActiveObject
    {

        private Transform target;
        private bool pointToCenter;

        public void Activate(Transform target)
        {
            this.target = target;
            gameObject.SetActive(true);
        }

        public void PointToCenter()
        {
            if (target) return;
            if (pointToCenter) return;
            pointToCenter = true;
            gameObject.SetActive(true);
        }

        public void StopPointingToCenter()
        {
            pointToCenter = false;
            if (!target && !pointToCenter) gameObject.SetActive(false);
        }

        public void Deactivate()
        {
            target = null;
            if (!target && !pointToCenter) gameObject.SetActive(false);
        }

	    public override void Act()
        {	
            if (target)
            {
                float angle = Vector2.SignedAngle(Vector3.up, (target.position - transform.position));
                transform.rotation = Quaternion.Euler(0,0,angle);
            }

            if (pointToCenter)
            {
                float angle = Vector2.SignedAngle(Vector3.up, (Vector3.zero - transform.position));
                transform.rotation = Quaternion.Euler(0,0,angle);
            }
        }


    }
}
