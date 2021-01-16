using System;
using ElysiumTest.Scripts.Presentation.Common;
using ElysiumTest.Scripts.Presentation.Components;
using ElysiumTest.Scripts.Presentation.Controllers.InputEvents;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ElysiumTest.Scripts.Presentation.Controllers
{
    public interface IInputController
    {
        Vector3? MousePosition { get; }
    }

    public class InputController : MonoBehaviour, IInputController
    {
        [SerializeField] private MousePressEvent onPressMouse;
        [SerializeField] private UnityEvent onMouseRelease;
        [SerializeField] private MouseMoveEvent onMouseMove;
        [SerializeField] private Camera currentCamera;
        [SerializeField] private float screenFrontDistance = 1f;

        public Vector3? MousePosition
        {
            get
            {
                var mouse = Mouse.current;
                if (mouse == null)
                    return null;

                var position = mouse.position.ReadValue();
                return currentCamera.ScreenToWorldPoint(position);
            }
        }

        // todo: use PlayerInput to get input indirectly
        private void Update()
        {
            var mouse = Mouse.current;
            if (mouse == null)
                return;
            var mousePosition = mouse.position.ReadValue();
            
            ExecuteMouseMove(mousePosition);
            
            if (mouse.leftButton.wasPressedThisFrame)
            {
                ExecuteMousePress(mousePosition);
            }
            else if (mouse.leftButton.wasReleasedThisFrame)
            {
                onMouseRelease.Invoke();
            }
        }

        private void ExecuteMouseMove(Vector2 mousePosition)
        {
            var vec3 = (Vector3) mousePosition;
            vec3.z = screenFrontDistance;
            var worldPoint = currentCamera.ScreenToWorldPoint(vec3);
            onMouseMove.Invoke(worldPoint);
        }

        private void ExecuteMousePress(Vector2 mousePosition)
        {
            var ray = currentCamera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider.CompareTag(Tags.InventoryItem))
                {
                    // todo: use Interactable instead of object explicitly
                    var widget = hit.collider.GetComponent<ItemWidget>();
                    if (!ReferenceEquals(widget, null))
                    {
                        onPressMouse.Invoke(widget);
                    }
                }
            }
        }
    }
}