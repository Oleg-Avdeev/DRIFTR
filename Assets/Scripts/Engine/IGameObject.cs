using UnityEngine;

public abstract class Updatable : MonoBehaviour
{
    public abstract void Update();
}

public abstract class Controller : Updatable
{
    public abstract void Initialize();
}

public abstract class ActiveObject : Updatable
{
    public virtual void Initialize() { }
    public T Create<T>(T gameObject) where T : ActiveObject
    {
        return Instantiate(gameObject, transform.position, Quaternion.identity, Game.GameController.Root);
    }
}