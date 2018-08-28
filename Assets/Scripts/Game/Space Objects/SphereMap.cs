using System.Collections.Generic;
using UnityEngine;

namespace Game.SpaceObjects
{
    public class SphereMap : ActiveObject
    {
        [SerializeField] private Material planetMaterial;

        public Obstacle Sphere;
        private List<Obstacle> spheres;
        private int stoppedCounter = 0;

        private float targetWhite = 0;
        private float currentWhite = 0;
        private bool changing;

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
                }

                if (spheres[i].transform.localScale.x < 2.8f)
                {
                    spheres[i].EnabledTurret = false;
                }
            }

            planetMaterial.SetFloat("_White",0);
        }

        public List<Vector3> SelectTurretPositions(int number)
        {
            var list = new List<Vector3>();
            number = Mathf.Min(spheres.Count, number);
            for (int i = 0; i < number; i++)
            {
                if (spheres[i].EnabledTurret)
                {
                    list.Add(spheres[i].GetTurretPosition());
                }
            }
            return list;
        }

        public void SetBossBattle(bool activate)
        {
            changing = true;
            targetWhite = !activate ? 0 : 1;

            for (int i = 0; i < spheres.Count; i++)
            {
                spheres[i].SetCollidable(!activate);
            }
        }

        public override void Update()
        {
            if (changing)
            {
                currentWhite = currentWhite + (targetWhite - currentWhite)/10f;
                planetMaterial.SetFloat("_White",currentWhite);
                
                if (currentWhite >= 0.99f)
                {
                    changing = false;
                }
            }
        }
    }
}