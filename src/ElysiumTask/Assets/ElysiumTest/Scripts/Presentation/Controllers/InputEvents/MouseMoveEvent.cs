using System;
using UnityEngine;
using UnityEngine.Events;

namespace ElysiumTest.Scripts.Presentation.Controllers.InputEvents
{
    [Serializable]
    public class MouseMoveEvent: UnityEvent<Vector3>
    {
        
    }
}