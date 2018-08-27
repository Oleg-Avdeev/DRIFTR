using UnityEngine;
using UnityEngine.PostProcessing;

namespace Game.Effects
{
    public class TimeScaleEffect : Updatable
    {
        [SerializeField] private PostProcessingProfile effectStackProfile;

        private bool slowDown = false;
        private float waitTimeScale = 0;
        private float targetTimeScale = 1.0f;

        private ChromaticAberrationModel.Settings chromaticFx;
        private VignetteModel.Settings vigneteFx;
        private BloomModel.Settings bloomFx;

        public void Initialize()
        {
            chromaticFx = effectStackProfile.chromaticAberration.settings;
            vigneteFx = effectStackProfile.vignette.settings;
            bloomFx = effectStackProfile.bloom.settings;
        }

        public void Activate()
        {
            targetTimeScale = 0.6f;
            slowDown = true;
        }

        public override void Update()
        {
            if (Time.timeScale > 0.99f && !slowDown) return;

            UpdateFX();
            GameController.AddPoints((int)((1f - Time.timeScale)*100));
            GameController.MainCamera.orthographicSize = 13 + (1f - Time.timeScale)*5;

            if (waitTimeScale > Time.unscaledTime) return;

            if (Mathf.Abs(targetTimeScale - Time.timeScale) > 0.01)
            {
                Time.timeScale = Time.timeScale + (targetTimeScale - Time.timeScale)/40;
            }
            else
            {
                if (slowDown)
                {
                    waitTimeScale = Time.unscaledTime + 1.5f;
                    targetTimeScale = 1;
                    slowDown = false;
                }
            }
        }

        private void UpdateFX()
        {
            chromaticFx.intensity = (1f - Time.timeScale)*2f;
            vigneteFx.intensity = (1f - Time.timeScale)*0.5f;
            bloomFx.bloom.intensity = 1f - Time.timeScale;

            effectStackProfile.chromaticAberration.settings = chromaticFx;
            effectStackProfile.vignette.settings = vigneteFx;
            effectStackProfile.bloom.settings = bloomFx;
        }
    }
}