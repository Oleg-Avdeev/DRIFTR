using UnityEngine;
using UnityEngine.PostProcessing;

namespace Game.Effects
{
    public class TimeScaleEffect : Actor
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
            UpdateFX();
        }

        public void Activate()
        {
            targetTimeScale = 0.6f;
            slowDown = true;
        }

        public void Deactivate()
        {
            targetTimeScale = 1f;
            slowDown = false;
        }

        public override void Act()
        {
            if (Time.timeScale > 0.99f && !slowDown) return;

            float m = Engine.GameLoop.NormalizedDeltaTime/Time.timeScale;

            UpdateFX();
            GameController.AddPoints((int)((1f - Time.timeScale) * 4 * 100 * m));

            GameController.MainCamera.orthographicSize = 13 + (1f - Time.timeScale)*5;

            if (waitTimeScale > Time.unscaledTime) return;

            if (Mathf.Abs(targetTimeScale - Time.timeScale) > 0.01)
            {
                Time.timeScale = Time.timeScale + (targetTimeScale - Time.timeScale)/10*m;
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