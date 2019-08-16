using Engine.MachineLearning;
using UnityEngine;

namespace Game
{
    public class SmartBoss : Ship
    {
        private double moveAroundTimer = 0;
        private NeuralNetwork neuralNetwork;
        public float TimeAlive;

        [SerializeField] private float[] input;
        [SerializeField] private float[] output;
        [SerializeField] private string sum;
        [SerializeField] private float angleSpeed;
        [SerializeField] private float angleTarget;
        private Transform target;

        private SpriteRenderer renderer;

        public override void Initialize()
        {
            maxAcceleration = maxAcceleration / 100;
            base.Initialize();

            renderer = GetComponentInChildren<SpriteRenderer>();
        }

        public void InitializeNeuralNetwork(NeuralNetwork network)
        {
            neuralNetwork = new NeuralNetwork(network);
            TimeAlive = 0;

            speed = Vector3.zero;
            acceleration = Vector3.zero;

            sum = neuralNetwork.GetNetworkMap();
            renderer.transform.localScale = new Vector3(2.4816f,2.4816f,2.4816f);
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        public NeuralNetwork GetNeuralNetwork()
        {
            return neuralNetwork;
        }

        public override void Act()
        {
            UpdateMovement();
            base.Act();
            
            renderer.color = Color.Lerp(Color.red, Color.green, GetFitness()*50);
        }

        // NOT SO SMART ANYMoRE
        private void UpdateMovement()
        {
            if (speed == Vector3.zero) speed = Vector3.up;

            float vr = speed.magnitude / maxSpeed;
            
            var v = (target.position - transform.position).normalized; 
            var da = Mathf.Atan2(v.x, v.y) - Mathf.Atan2(speed.x, speed.y);
            
            angleTarget = Mathf.Atan2(v.x, v.y)*180/Mathf.PI;
            angleSpeed = Mathf.Atan2(speed.x, speed.y)*180/Mathf.PI;
            

            float[] hr = new float[5];
            
            float maxDistance = 10;
            Vector3 vector = Quaternion.Euler(0,0,-45) * speed;
            for (int i = 0; i < 5; i++)
            {
                RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, vector, maxDistance);
                if (hitInfo.distance == 0) hr[i] = 0;
                else hr[i] = 1 - hitInfo.distance/maxDistance;
                vector = Quaternion.Euler(0,0,45f/2) * vector;
            }

            input = new float[] { 0, hr[0], hr[1], hr[2], hr[3], hr[4]};
            output = neuralNetwork.Run(input);

            angleTarget = -angleTarget + output[0]*360;

            acceleration = Quaternion.Euler(0, 0, angleTarget) * Vector3.up;
            acceleration = (acceleration).normalized * maxAcceleration;

            TimeAlive++;
        }

        public void Highlight()
        {
            renderer.transform.localScale = new Vector3(5,5,5);
        }

        public float GetFitness()
        {
            float rank = 1/(transform.position - target.position).magnitude;
            if (!gameObject.activeSelf) rank = rank/1.1f;
            return rank;
        }

    }
}