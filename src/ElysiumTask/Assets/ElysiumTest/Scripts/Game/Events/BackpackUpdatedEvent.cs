using System;
using ElysiumTest.Scripts.Game.Models;
using UnityEngine.Events;

namespace ElysiumTest.Scripts.Game.Events
{
    [Serializable]
    public class BackpackUpdatedEvent: UnityEvent<Item>
    {
        
    }
}