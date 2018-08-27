using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerShip : Ship
    {        


        protected override void UpdateState()
        {
            var direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            direction.z = 0;
            
            acceleration = (direction).normalized*maxAcceleration/100;
            if (Input.GetMouseButtonDown(0)) { Shoot(direction.normalized); }

            if (speed.magnitude < maxSpeed*0.5f)
            {
                GameController.Multiplier = 1f;
            }
            else
            {
                GameController.Multiplier += 0.01f;
            }
        }
    }

}