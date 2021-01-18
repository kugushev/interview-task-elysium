using System;
using UnityEngine;

namespace ElysiumTest.Scripts.Presentation.Interfaces
{
    public interface IInputInfo
    {
        event Action<Vector3> CursorMove; 
    }
}