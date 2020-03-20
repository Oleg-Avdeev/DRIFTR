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
        
        private float sizeMin = 14;//13.0f;
        private float sizeMax = 19;//18.0f;

        private ChromaticAberrationModel.Settings chromaticFx;
        private VignetteModel.Settings vigneteFx;
        private BloomModel.Settings bloomFx;

        public void Initialize()
        {
            GameController.MainCamera.orthographicSize = sizeMin;
            chromaticFx = effectStackProfile.chromaticAberration.settings;
            vigneteFx = effectStackProfile.vignette.settings;
            bloomFx = effectStackProfile.bloom.settings;
            UpdateFX();
        }

        public void Activate()
        {
            targetTimeScale = 0.6f;
            slowDown = true;

            Game.SFX.SoundEffectController.Instance.SetTargetReverb(1.18f, 7f);
            Game.SFX.SoundEffectController.Instance.SetTargetLowPass(7000f);
        }

        public void Deactivate()
        {
            targetTimeScale = 1f;
            slowDown = false;

            Game.SFX.SoundEffectController.Instance.SetTargetReverb(0f);
            Game.SFX.SoundEffectController.Instance.SetTargetLowPass(20000f);
        }

        public override void Act()
        {
            if (Time.timeScale > 0.99f && !slowDown) return;

            float m = Engine.GameLoop.NormalizedDeltaTime/Time.timeScale;

            UpdateFX();
            GameController.AddPoints((int)((1f - Time.timeScale) * 5 * 100 * m));

            GameController.MainCamera.orthographicSize = sizeMin + (1f - Time.timeScale)*(sizeMax - sizeMin);

            if (waitTimeScale > Time.unscaledTime) return;

            if (Mathf.Abs(targetTimeScale - Time.timeScale) > 0.01)
            {
                Time.timeScale = Time.timeScale + (targetTimeScale - Time.timeScale)/10*m;
            }
            else
            {
                if (slowDown)
                {
                    waitTimeScale = Time.unscaledTime + 1.2f;
                    targetTimeScale = 1;
                    slowDown = false;

                    Game.SFX.SoundEffectController.Instance.SetTargetReverb(0f, 80f);
                    Game.SFX.SoundEffectController.Instance.SetTargetLowPass(20000f);
                }
            }
        }

        private void UpdateFX()
        {
            vigneteFx.intensity = (1f - Time.timeScale)*0.5f;
            effectStackProfile.vignette.settings = vigneteFx;

            // chromaticFx.intensity = (1f - Time.timeScale)*2f;
            // effectStackProfile.chromaticAberration.settings = chromaticFx;
            
            // bloomFx.bloom.intensity = 1f - Time.timeScale;
            // effectStackProfile.bloom.settings = bloomFx;
        }
    }
}