using System.Collections.Generic;
using Game.Projectiles;
using UnityEngine;

namespace Game
{
    public class SmartBoss : Boss
    {
        private double moveAroundTimer = 0;

        public override void Act()
        {
            UpdateMovement();
            for (int i = 0; i < subTurrets.Count; i++)
            {
                if (subTurrets[i]) subTurrets[i].Act();
            }
        }

        private void UpdateMovement()
        {
            if (target != null)
            {
                var direction = (target.transform.position - transform.position);

                moveAroundTimer += 0.1;
                if (moveAroundTimer > 40)
                {
                    direction = Vector2Extension.Rotate(target.GetVelocityVector(), 90);
                    if (moveAroundTimer > 60)
                    {
                        moveAroundTimer = 0;
                    }
                }

                if (transform.position.magnitude > 8)
                {
                    direction += -transform.position;
                }

                direction.z = 0;
                acceleration = (direction).normalized * maxAcceleration / 100;
            }

            speed += 4 * acceleration * Engine.GameLoop.NormalizedDeltaTime;
            speed = speed.normalized * Mathf.Min(speed.magnitude, maxSpeed);
            transform.localPosition += speed * 4 * Engine.GameLoop.NormalizedDeltaTime;

            float angle = Vector3.Angle(Vector3.up, acceleration);
            if (acceleration.x > 0) angle = 360 - angle;
            transform.localRotation = Quaternion.Euler(0, 0, angle);
        }

    }
}