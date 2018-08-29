using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    public abstract void Act();
}

public abstract class Controller : Actor
{
    public abstract void Initialize();
}

public abstract class ActiveObject : Actor
{
    public virtual void Initialize() { }
    public T Create<T>(T gameObject) where T : ActiveObject
    {
        return Instantiate(gameObject, transform.position, Quaternion.identity, Game.GameController.Root);
    }
}