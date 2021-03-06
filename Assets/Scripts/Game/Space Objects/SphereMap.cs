using System.Collections.Generic;
using UnityEngine;

namespace Game.SpaceObjects
{
    public class SphereMap : ActiveObject
    {
        public Obstacle Sphere;
        private List<Obstacle> spheres;
        private int stoppedCounter = 0;

        private bool bossFight;

        private Transform playerTransform;
        private float closestPlanetDistance;

        public override void Initialize()
        {
            spheres = new List<Obstacle>();

            for ( int i = 0; i < 200; i++)
            {
                float x = Random.Range(-100, 100); 
                float y = Random.Range(-100, 100);
                if (x*x + y*y < 400)
                {
                    i--;
                    continue;
                } 

                spheres.Add(Instantiate(Sphere, new Vector3(x,y,0), Quaternion.identity, transform.parent));
                spheres[i].transform.localScale = Random.Range(0,1)*Vector3.one;
                spheres[i].SetRandomRotation();
            }

            while (stoppedCounter < spheres.Count)
            {
                for (int i = 0; i < spheres.Count; i++)
                {
                    if (spheres[i].Stopped) continue;
                    spheres[i].transform.localScale += 0.1f*Vector3.one;
                    for (int j = 0; j < spheres.Count; j++)
                    {
                        if (i == j) continue;
                        var distance = (spheres[i].transform.localPosition - spheres[j].transform.localPosition).magnitude;
                        var scaleSum = (spheres[i].transform.localScale.x + spheres[j].transform.localScale.x)*0.8f;
                        if (scaleSum > distance)
                        {
                            stoppedCounter++;
                            spheres[i].Stopped = true;
                        }
                    }
                }
            }

            for (int i = 0; i < spheres.Count; i++)
            {
                if (spheres[i].transform.localScale.x < 0.2f)
                {
                    Destroy(spheres[i].gameObject);
                    spheres.RemoveAt(i);
                    continue;
                }

                if (i < spheres.Count && spheres[i].transform.localScale.x < 2.8f)
                {
                    spheres[i].EnabledTurret = false;
                }

                spheres[i].SetScale();
            }
        }

        public void AddRandomRotation()
        {
            for (int i = 0; i < spheres.Count; i++)
            {
                spheres[i].AddRandomRotation();
            }

            spheres.Shuffle();
        }

        public void SetPlayerTransform(Transform player)
        {
            playerTransform = player;
        }

        public float GetClosestPlanet()
        {
            return closestPlanetDistance;
        }

        public List<Transform> SelectTurretPositions(int number)
        {
            var list = new List<Transform>();
            number = Mathf.Min(spheres.Count, number);
            for (int i = 0; i < number; i++)
            {
                if (spheres[i].EnabledTurret)
                {
                    var slot =spheres[i].GetTurretPosition();
                    if (slot == null) continue;
                    list.Add(slot);
                }
            }
            return list;
        }

        public void SetBossBattle(bool activate)
        {
            bossFight = activate;

            for (int i = 0; i < spheres.Count; i++)
            {
                spheres[i].SetCollidable(!activate);
            }
        }

        public override void Act()
        {
            closestPlanetDistance = 1000;
            
            for (int i = 0; i < spheres.Count; i++)
            {
                spheres[i].Act();
                
                if (playerTransform)
                {
                    var displacement = spheres[i].transform.localPosition - playerTransform.localPosition;
                    var distance = displacement.magnitude - 0.5f*spheres[i].transform.localScale.x;
                    if (distance < closestPlanetDistance)
                    {
                        closestPlanetDistance = distance;
                    }

                    ShiftObstacle(spheres[i], displacement);
                }
            }   
        }

        private void ShiftObstacle(Obstacle target, Vector2 displacement)
        {
            float x = 0, y = 0;
            
            if (displacement.x > 150) x = -210;
            else if (displacement.x < -150) x = 210;
            
            if (displacement.y > 150) y = -210;
            else if (displacement.y < -150) y = 210;

            target.ShiftPosition(x, y);
        }
    }
}