
using UnityEngine;
using UnityEngine.Events;

public class GameEventUnityListener : MonoBehaviour, IGameEventListener
{
    [SerializeField]
    private GameEvent @event;
    [SerializeField]
    private UnityEvent response;

    private void OnEnable()
    { 
        @event.RegisterListener(this); 
    }

    private void OnDisable()
    { 
        @event.UnregisterListener(this); 
    }

    public void OnEventRaised()
    { 
        response?.Invoke(); 
    }
}
