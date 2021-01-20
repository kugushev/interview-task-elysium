﻿using System;
using ElysiumTest.Scripts.Presentation.Components;
using UnityEngine.Events;

namespace ElysiumTest.Scripts.Presentation.Controllers.InputEvents
{
    [Serializable]
    public class MouseReleaseOnItemUIEvent : UnityEvent<ItemUIWidget>
    {
        
    }
}