using Reflex.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore.Game
{
    public class Greeter : MonoBehaviour
    {
        [Inject] private readonly IEnumerable<string> _messages = null;

        private void Start()
        {
            if (_messages != null)
                Debug.Log(string.Join(" ", _messages));    
        }
    }
}
