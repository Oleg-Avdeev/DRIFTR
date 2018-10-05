using UnityEngine;

namespace Game.SFX
{
    public class ShipSoundEffects : MonoBehaviour
    {
        [SerializeField] private AudioClip explodeClip;
        [SerializeField] private AudioClip shootClip;

        public void Shoot()
        {
            SoundController.Instance.PlaySound(shootClip);
        }

        public void Explode()
        {
            SoundController.Instance.PlaySound(explodeClip);
        }
    }
}