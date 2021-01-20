using System;
using ElysiumTest.Scripts.Presentation.Common;
using ElysiumTest.Scripts.Presentation.Components;
using ElysiumTest.Scripts.Presentation.Controllers.InputEvents;
using ElysiumTest.Scripts.Presentation.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ElysiumTest.Scripts.Presentation.Controllers
{
    public class InputController : MonoBehaviour, IInputInfo
    {
        [SerializeField] private MousePressItemEvent onPressItemMouse;
        [SerializeField] private MousePressBackpackEvent onPressBackpackMouse;
        [SerializeField] private MouseReleaseEvent onMouseRelease;
        [SerializeField] private MouseMoveEvent onMouseMove;
        [SerializeField] private Camera currentCamera;
        [SerializeField] private float mouseZ = 1f;
        [SerializeField] private float raycastDistance = 16f;

        public event Action<Vector3> CursorMove;

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
                ExecuteMouseRelease(mousePosition);
            }
        }

        private void ExecuteMouseMove(Vector2 mousePosition)
        {
            var worldPoint = GetMouseWorldPoint(mousePosition);
            onMouseMove.Invoke(worldPoint);
            CursorMove?.Invoke(worldPoint);
        }

        private void ExecuteMousePress(Vector2 mousePosition)
        {
            var ray = currentCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out var hit, raycastDistance, TagsAndLayers.PressableLayers))
            {
                if (hit.collider.CompareTag(TagsAndLayers.InventoryItemTag))
                {
                    var widget = hit.collider.GetComponent<ItemWidget>();
                    if (!ReferenceEquals(widget, null))
                        onPressItemMouse.Invoke(widget);
                }
                
                if (hit.collider.CompareTag(TagsAndLayers.InventoryTag))
                {
                    var widget = hit.collider.GetComponent<BackpackWidget>();
                    if (!ReferenceEquals(widget, null))
                        onPressBackpackMouse.Invoke(widget);
                }
            }
        }

        private void ExecuteMouseRelease(Vector2 mousePosition)
        {
            BackpackWidget backpack = null;

            var ray = currentCamera.ScreenPointToRay(mousePosition);


            if (Physics.Raycast(ray, out var hit, raycastDistance, TagsAndLayers.ReleasableLayers))
            {
                if (hit.collider.CompareTag(TagsAndLayers.InventoryTag))
                    backpack = hit.collider.GetComponent<BackpackWidget>();
            }

            onMouseRelease.Invoke(backpack);
        }

        private Vector3 GetMouseWorldPoint(Vector2 mousePosition)
        {
            var vec3 = (Vector3) mousePosition;
            vec3.z = mouseZ;
            var worldPoint = currentCamera.ScreenToWorldPoint(vec3);
            return worldPoint;
        }
    }
}