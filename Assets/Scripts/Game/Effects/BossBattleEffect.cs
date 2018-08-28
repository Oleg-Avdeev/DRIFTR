using UnityEngine;
using UnityEngine.PostProcessing;

namespace Game.Effects
{
    public class BossBattleEffect : Updatable
    {
        [SerializeField] private PostProcessingProfile effectStackProfile;

        private bool changing = false;
        private float wait = 0;
        private float currentValue = 0.0f;
        private float targetValue = 1.0f;

        private ChromaticAberrationModel.Settings chromaticFx;
        private BloomModel.Settings bloomFx;

        public void Initialize()
        {
            chromaticFx = effectStackProfile.chromaticAberration.settings;
            bloomFx = effectStackProfile.bloom.settings;
            UpdateFX();
        }

        public void Activate()
        {
            targetValue = 1f;
            changing = true;
        }

        public void Deactivate()
        {
            targetValue = 0f;
            changing = false;
        }

        public override void Update()
        {
            if (currentValue < 0.1f && !changing) return;

            UpdateFX();
          
            if (wait > currentValue) return;

            if (Mathf.Abs(targetValue - currentValue) > 0.01)
            {
                currentValue = currentValue + (targetValue - currentValue)/40;
            }
            else
            {
                changing = false;
            }
        }

        private void UpdateFX()
        {
            chromaticFx.intensity = currentValue*0.8f;
            bloomFx.bloom.intensity = currentValue*0.8f;

            effectStackProfile.chromaticAberration.settings = chromaticFx;
            effectStackProfile.bloom.settings = bloomFx;
        }
    }
}