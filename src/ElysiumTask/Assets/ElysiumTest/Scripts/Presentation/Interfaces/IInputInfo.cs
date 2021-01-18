using System;
using ElysiumTest.Scripts.Presentation.Controllers.InputEvents;
using UnityEngine;

namespace ElysiumTest.Scripts.Presentation.Interfaces
{
    public interface IInputInfo
    {
        Vector3? MousePosition { get; }

        event Action<Vector3> CursorMove; 
    }
}