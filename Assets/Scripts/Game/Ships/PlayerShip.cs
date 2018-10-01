using System.Collections.Generic;
using Engine;
using UnityEngine;

namespace Game
{
    public class PlayerShip : Ship
    {        
        [SerializeField] private GameObject trailRenderer;
        [SerializeField] private PlayerPointer pointer;
        private bool jumped;
        private float waitJump;

        protected override void UpdateState()
        {
            var direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            direction.z = 0;
            
            acceleration = (direction).normalized*maxAcceleration/100;
            if (Input.GetMouseButtonDown(0)) { Shoot(direction.normalized); }

            if ((speed.magnitude < maxSpeed*0.5f) && (Time.timeScale > 0.5f))
            {
                GameController.Multiplier = 1f;
            }

            FixPosition();
            pointer.Act();
        }

        public void LevelUp(int level)
        {
            if (level == 1)
            {
                weaponCooldown -= 0.05f;
            }
            if (level == 2)
            {
                weaponCooldown -= 0.05f;
            }
        }

        public void PointToBoss(Transform boss)
        {
            pointer.Activate(boss);
        }

        public void StopPointingToBoss()
        {
            pointer.Deactivate();
        }

        private void FixPosition()
        {
            if (transform.localPosition.x < -150)
                pointer.PointToCenter();
            else if (transform.localPosition.x > 150)
                pointer.PointToCenter();
            else if (transform.localPosition.y < -150)
                pointer.PointToCenter();
            else if (transform.localPosition.y > 150)
                pointer.PointToCenter();

            else pointer.StopPointingToCenter();
        }
    }

}