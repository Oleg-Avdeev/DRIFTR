using System.Collections.Generic;
using Game.Projectiles;
using UnityEngine;

namespace Game
{
    public class Turret : Ship
    {
        [SerializeField] private float shootingAngle;
        [SerializeField] private float shootingDistance;

        private List<Ship> ships;
        private Ship target = null;
        private Vector2 shootDirection;

        public void UpdateShipList(List<Ship> ships)
        {
            this.ships = ships;
			transform.localScale = Vector3.one * (1f/transform.parent.localScale.x);
        }

        protected override void UpdateState()
        {
            if (target == null) SelectTarget();
            if (target == null) return;

            shootDirection = target.transform.position - transform.position;
            acceleration = shootDirection;
            if (shootDirection.magnitude > shootingDistance)
            {
                target = null;
                SelectTarget();
            }
            
            // if (Vector3.Angle(parentShip.GetDirection(), shootDirection) > shootingAngle)
            // {
                // target = null;
                // selectTarget();
            // }

            if (target == null) return;
            shootDirection = ShootAheadVector(shootDirection, target);
            Shoot(shootDirection, target.transform);
        }

        private void SelectTarget()
        {
            if (ships != null)
            {
                for (int shipI = 0; shipI < ships.Count; shipI++)
                {
                    if (ships[shipI] == null) continue;
                    shootDirection = ships[shipI].transform.position - transform.position;
                    if (shootDirection.magnitude > shootingDistance) continue;
                    // if (Vector3.Angle(parentShip.GetDirection(), shootDirection) > ShootingAngle) continue;
                    target = ships[shipI];
                    return;
                }
            }
        }

        protected Vector3 ShootAheadVector(Vector3 initialDirection, Ship target)
        {
            // return initialDirection;
            // if (targetRB == null) targetRB = target.GetComponent<Rigidbody2D>();
            // Vector3 shiftVector = target.GetShipSpeed() * (shootDirection.magnitude / parentShip.GetUnitInfo().GetProjectileSpeed());
            return initialDirection;// + shiftVector;
        }
    }
}