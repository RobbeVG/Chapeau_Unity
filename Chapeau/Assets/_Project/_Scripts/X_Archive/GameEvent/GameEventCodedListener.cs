using System;
using UnityEngine;

[Serializable]
public class GameEventCodedListener : IGameEventListener
{
    [SerializeField]
    private GameEvent @event;
    private Action _response;

    public void OnEnable(Action response)
    {
        @event?.RegisterListener(this);
        _response = response;
    }

    public void OnDisable()
    {
        @event?.UnregisterListener(this);
        _response = null;
    }

    public void OnEventRaised()
    {
        _response?.Invoke();
    }
}
