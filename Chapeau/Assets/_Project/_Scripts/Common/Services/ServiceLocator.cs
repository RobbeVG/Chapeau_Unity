using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore.Common.Services
{
    public interface IService
    {
    }

    [DisallowMultipleComponent]
    public class ServiceLocator : MonoBehaviour
    {
        private readonly Dictionary<Type, IService> _services = new Dictionary<Type, IService>();

        public T Get<T>() where T : IService
        {
            if (_services.TryGetValue(typeof(T), out var service))
            {
                return (T)service;
            }
            else
            {

                throw new InvalidOperationException($"Service of type {typeof(T)} is not registered.");
            }
        }

        public void Register<T>(T service) where T : IService
        {
            var type = typeof(T);
            if (_services.ContainsKey(type))
            {
                throw new InvalidOperationException($"Service of type {type} is already registered.");
            }
            _services.Add(type, service);
        }

        public void Unregister<T>() where T : IService
        {
            var type = typeof(T);
            if (!_services.ContainsKey(type))
            {
                throw new InvalidOperationException($"Service of type {type} is not registered.");
            }
            _services.Remove(type);
        }
    }
}
