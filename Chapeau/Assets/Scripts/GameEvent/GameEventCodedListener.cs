using System;
using UnityEngine;

public class GameEventCodedListener : IGameEventListener
{
    [SerializeField]
    private GameEvent @event;
    [SerializeField]
    private Action _response;

    private void OnEnable(Action response)
    {
        @event?.RegisterListener(this);
        _response = response;
    }

    private void OnDisable()
    {
        @event?.UnregisterListener(this);
        _response = null;
    }

    public void OnEventRaised()
    {
        _response?.Invoke();
    }
}
