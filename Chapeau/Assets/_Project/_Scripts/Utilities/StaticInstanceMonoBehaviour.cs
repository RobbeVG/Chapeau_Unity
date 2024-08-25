using UnityEngine;

/// <summary>
/// A static instance is similar to a singleton, but instead of destroying any new
/// instances, it overrides the current instance. This is handy for resetting the state
/// and saves you doing it manually
/// </summary>
public abstract class StaticInstanceMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected virtual void Awake() => Instance = this as T;

    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
}

/// <summary>
/// This transforms the static instance into a basic singleton. This will destroy any new
/// versions created, leaving the original instance intact
/// </summary>
public abstract class SingletonMonobehaviour<T> : StaticInstanceMonoBehaviour<T> where T : MonoBehaviour {
    protected override void Awake() {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        base.Awake(); //setting the Instance
    }
}

