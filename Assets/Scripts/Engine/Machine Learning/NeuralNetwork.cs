using System;

namespace Engine.MachineLearning
{
    public class NeuralNetwork
    {
        private float[] hiddenLayer;
        private float[] hc1, hc2, b;
        private int inputSize;
        private int outputSize;

        public NeuralNetwork() {}

        public NeuralNetwork(NeuralNetwork source)
        {
            Initialize(source.hiddenLayer.Length, source.inputSize, source.outputSize);
            for (int i = 0; i < hc1.Length; i++) hc1[i] = source.hc1[i];
            for (int i = 0; i < hc2.Length; i++) hc2[i] = source.hc2[i];
            for (int i = 0; i < b.Length; i++) b[i] = source.b[i];
        }

        public NeuralNetwork(NeuralNetwork parent1, NeuralNetwork parent2)
        {
            Random rng = new Random();

            Initialize(parent1.hiddenLayer.Length, parent1.inputSize, parent1.outputSize);
            
            for (int i = 0; i < hc1.Length; i++) 
                hc1[i] = (rng.NextDouble() > 0.8) ? parent1.hc1[i] : parent2.hc1[i];
            for (int i = 0; i < hc2.Length; i++) 
                hc2[i] = (rng.NextDouble() > 0.8) ? parent1.hc2[i] : parent2.hc2[i];
        }

        public void Initialize(int hiddenLayerSize, int inputSize, int outputSize)
        {
            this.inputSize = inputSize;
            this.outputSize = outputSize;

            hiddenLayer = new float[hiddenLayerSize];
            hc1 = new float[hiddenLayerSize*inputSize];
            hc2 = new float[hiddenLayerSize*outputSize];
            b = new float[hiddenLayerSize];
        }

        public float[] Run(float[] input)
        {
            for (int hi = 0; hi < hiddenLayer.Length; hi++)
                hiddenLayer[hi] = 0;

            for (int hi = 0; hi < hiddenLayer.Length; hi++)
            {
                for (int ii = 0; ii < inputSize; ii++)
                    hiddenLayer[hi] += input[ii] * hc1[ii*hiddenLayer.Length + hi];
                hiddenLayer[hi] = s(hiddenLayer[hi] + b[hi]);
            }

            float[] output = new float[outputSize];
            for (int oi = 0; oi < outputSize; oi++)
            {
                for (int hi = 0; hi < hiddenLayer.Length; hi++)
                    output[oi] += hiddenLayer[hi] * hc2[oi*hiddenLayer.Length + hi];
                output[oi] = output[oi];
            }

            return output;
        }

        public void FillWithRandomCoefficients(int seed)
        {
            Random rng = new Random(seed);

            for (int i = 0; i < hc1.Length; i++)
                hc1[i] = (1 - 2*(float)rng.NextDouble())/2;

            for (int i = 0; i < hc2.Length; i++)
                hc2[i] = (1 - 2*(float)rng.NextDouble())/2;

            for (int i = 0; i < b.Length; i++)
                b[i] = 1f;
        }

        public void Mutate(float mutationFactor)
        {
            Random rng = new Random();

            if (rng.NextDouble() > 0.4) mutationFactor = 0.1f;

                for (int i = 0; i < hc1.Length; i++)
                    if (rng.NextDouble() < 0.3)
                        hc1[i] += (1f - ((float)rng.NextDouble()*2f))*mutationFactor;
                    else hc1[i] += (1f - ((float)rng.NextDouble()*2f))*mutationFactor*0.05f;

                for (int i = 0; i < hc2.Length; i++)
                    if (rng.NextDouble() < 0.3)
                        hc2[i] += (1f - ((float)rng.NextDouble()*2f))*mutationFactor;
                    else hc2[i] += (1f - ((float)rng.NextDouble()*2f))*mutationFactor*0.05f;
        }

        private float s(float sum)
        {
            // return Math.Max(0, sum);
            return 1f/(1f + (float)Math.Exp(-sum));
        }

        public string GetNetworkMap()
        {
            string map = "";
            for (int i = 0; i < hc1.Length; i++) map = map + string.Format("{0:0.00}", hc1[i]) + " ";
            map += "\n";
            for (int i = 0; i < hc2.Length; i++) map = map + string.Format("{0:0.00}", hc2[i]) + " ";
            return map;
        }
    }
}