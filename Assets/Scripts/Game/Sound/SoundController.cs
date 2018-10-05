using UnityEngine;

namespace Game.SFX
{
    public class SoundController : MonoBehaviour
    {
        public static SoundController Instance;
        [SerializeField] private AudioSource[] audioUnits;
        [SerializeField] private SoundEffectController soundEffectControllers;
        private int currentUnitIndex;

        public void Initialize()
        {
            Instance = this;
            soundEffectControllers.Initialize();
        }

        public void PlaySound(AudioClip clip)
        {
            audioUnits[currentUnitIndex].PlayOneShot(clip);
            currentUnitIndex = (currentUnitIndex + 1) % audioUnits.Length;
        }

    }
}