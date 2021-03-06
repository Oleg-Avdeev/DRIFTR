using UnityEngine;

[RequireComponent(typeof(ImageAnimator))]
public class Explosion : ActiveObject
{
    public override void Initialize()
    {
        GetComponent<ImageAnimator>().EndCallback = DestroyGameObject;
    }

    public override void Act()
    {

    }

    private void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}