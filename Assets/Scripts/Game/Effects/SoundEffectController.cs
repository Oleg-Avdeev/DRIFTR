using UnityEngine;
using UnityEngine.PostProcessing;

namespace Game.SFX
{
    public class SoundEffectController : Actor
    {

        public static SoundEffectController Instance;

        [SerializeField] private AudioDistortionFilter distortionFilter;
        [SerializeField] private AudioLowPassFilter lowPassFilter;
        [SerializeField] private AudioReverbFilter reverbFilter;

        private float distortionTarget;
        private float distortionSpeed;
        private float lowPassTarget;
        private float lowPassSpeed;
        private float reverbTarget;
        private float reverbSpeed;

        private bool locked;

        public void SetTargetDistortion(float value, float speed = 20)
        {
            if (locked) return;
            distortionTarget = value;
            distortionSpeed = speed;
        }

        public void SetTargetLowPass(float value, float speed = 20)
        {
            if (locked) return;
            lowPassTarget = value;
            lowPassSpeed = speed;
        }
        
        public void SetTargetReverb(float value, float speed = 20)
        {
            if (locked) return;
            reverbTarget = Mathf.Min(20, value*9016 - 9000);
            reverbSpeed = speed;
        }

        public void LockEffects()
        {
            locked = true;
        }

        public void UnlockEffects()
        {
            locked = false;
        }

        public override void Act()
        {
            if (Mathf.Abs(distortionTarget - distortionFilter.distortionLevel) > 0.01)
                distortionFilter.distortionLevel += (distortionTarget - distortionFilter.distortionLevel)/distortionSpeed;
            
            if (Mathf.Abs(reverbTarget - reverbFilter.dryLevel) > 0.01)
                reverbFilter.reverbLevel += (reverbTarget - reverbFilter.reverbLevel)/reverbSpeed;
            
            if (Mathf.Abs(lowPassTarget - lowPassFilter.cutoffFrequency) > 0.01)
                lowPassFilter.cutoffFrequency += (lowPassTarget - lowPassFilter.cutoffFrequency)/lowPassSpeed;
        }

        public void Initialize()
        {
            Instance = this;
            distortionTarget = 0;
            lowPassTarget = 20000f;
            reverbTarget = 0f;

            reverbSpeed = 100f;
            distortionSpeed = 100f;
            lowPassSpeed = 100f;
        }


    }
}