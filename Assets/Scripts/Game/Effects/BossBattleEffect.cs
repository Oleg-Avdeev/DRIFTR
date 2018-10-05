using UnityEngine;
using UnityEngine.PostProcessing;

namespace Game.Effects
{
    public class BossBattleEffect : Actor
    {
        [SerializeField] private PostProcessingProfile effectStackProfile;
        [SerializeField] private Material[] materialList;

        private bool changing = false;
        private float wait = 0;
        private float currentValue = 0.0f;
        private float targetValue = 0.0f;

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

            Game.SFX.SoundEffectController.Instance.SetTargetDistortion(0.1f);
            Game.SFX.SoundEffectController.Instance.SetTargetLowPass(5000f);
            Game.SFX.SoundEffectController.Instance.SetTargetReverb(3.5f);
            Game.SFX.SoundEffectController.Instance.LockEffects();
        }

        public void Deactivate()
        {
            targetValue = 0f;
            changing = true;

            Game.SFX.SoundEffectController.Instance.UnlockEffects();
            Game.SFX.SoundEffectController.Instance.SetTargetDistortion(0.0f);
            Game.SFX.SoundEffectController.Instance.SetTargetLowPass(20000f);
            Game.SFX.SoundEffectController.Instance.SetTargetReverb(0f);
        }

        public override void Act()
        {
            if (!changing && targetValue == 0) return;

            UpdateFX();
          
            if (wait > currentValue) return;

            if (Mathf.Abs(targetValue - currentValue) > 0.01)
            {
                currentValue = currentValue + (targetValue - currentValue)/20*Engine.GameLoop.NormalizedDeltaTime;
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

            for (int i = 0; i < materialList.Length; i++)
            {
                materialList[i].SetFloat("_White",currentValue);
            }
        }
    }
}