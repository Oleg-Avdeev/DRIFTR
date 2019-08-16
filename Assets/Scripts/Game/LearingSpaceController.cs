using System.Collections.Generic;
using Engine.MachineLearning;
using Game.SpaceObjects;
using UnityEngine;

namespace Game
{
    public class LearingSpaceController : SpaceController
    {
        [SerializeField] private Ship nnShip;
        [SerializeField] private SphereMap sphearMapPrefab;
        [SerializeField] private float mutationFactor;
        [SerializeField] private Transform target;

        private List<SmartBoss> ships;
        private SphereMap map;
        private float endTimer;


        public override void Initialize()
        {
            ships = new List<SmartBoss>();
            for (int i = 0; i < 60; i++)
            {
                var nn = new NeuralNetwork();
                nn.Initialize(5, 6, 1);
                nn.FillWithRandomCoefficients(i);

                var ship = Instantiate(nnShip, Vector3.zero, Quaternion.identity, transform) as SmartBoss;
                ship.Initialize();
                ship.InitializeNeuralNetwork(nn);
                ship.SetTarget(target);
                
                ships.Add(ship);
            }

            map = Instantiate(sphearMapPrefab, Vector3.right*10, Quaternion.identity, transform);
            map.Initialize();
        }

        public override void Act()
        {
            int shipCount = 0;
            for (int i = 0; i < ships.Count; i++)
            {
                if (ships[i] && ships[i].gameObject.activeSelf)
                {
                    ships[i].Act();
                    shipCount++;
                }
            }
            
            endTimer++;
            var newNetworks = new List<NeuralNetwork>();

            if (endTimer > 300 || shipCount == 0)
            {
                ships.Sort((s1, s2) => { return s2.GetFitness().CompareTo(s1.GetFitness()); });

                newNetworks.Add(ships[0].GetNeuralNetwork());

                for (int i = 1; i < ships.Count; i++)
                {   
                    int i1 = Random.Range(0,10);
                    int i2 = Random.Range(0,5);

                    var s1 = ships[i1];
                    var s2 = ships[i2];

                    NeuralNetwork nnn = new NeuralNetwork(s1.GetNeuralNetwork(), s2.GetNeuralNetwork());
                    nnn.Mutate(2);
                    newNetworks.Add(nnn);
                }

                for (int i = 0; i < ships.Count; i++)
                {
                    ships[i].InitializeNeuralNetwork(newNetworks[i]);
                    ships[i].transform.position = Vector3.zero;
                    ships[i].gameObject.SetActive(true);
                }

                ships[0].Highlight();

                endTimer = 0;
            }
        }
    }
}