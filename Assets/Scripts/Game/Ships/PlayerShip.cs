using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerShip : Ship
    {        
        [SerializeField] private GameObject trailRenderer;
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
            else
            {
                GameController.Multiplier += 0.01f;
            }
        }

        // private void FixPosition()
        // {
        //     if (jumped)
        //     {
        //         if (waitJump < Time.fixedTime)
        //         {
        //             jumped = false;
        //             trailRenderer.SetActive(true);
        //         }
        //     }

        //     if (transform.localPosition.x < -150)
        //         JumpTo(x: 150);

        //     if (transform.localPosition.x > 150)
        //         JumpTo(x: -150);

        //     if (transform.localPosition.y < -150)
        //         JumpTo(y: 150);

        //     if (transform.localPosition.y > 150)
        //         JumpTo(y: -150);
        // }

        // private void JumpTo(float x = 0, float y = 0)
        // {
        //     if (x == 0) x = transform.localPosition.x;
        //     if (y == 0) y = transform.localPosition.y;
        //     transform.localPosition = new Vector3(x,y,transform.localPosition.z);

        //     trailRenderer.SetActive(false);
        //     jumped = true;
        //     waitJump = Time.fixedTime + 0.5f;
        // }
    }

}