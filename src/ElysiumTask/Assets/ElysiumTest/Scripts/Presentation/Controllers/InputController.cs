using System;
using System.Collections.Generic;
using ElysiumTest.Scripts.Presentation.Common;
using ElysiumTest.Scripts.Presentation.Controllers.InputEvents;
using ElysiumTest.Scripts.Presentation.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace ElysiumTest.Scripts.Presentation.Controllers
{
    public class InputController : MonoBehaviour, IInputInfo
    {
        [SerializeField] private MousePressItemEvent onPressItemMouse;
        [SerializeField] private MousePressBackpackEvent onPressBackpackMouse;
        [SerializeField] private MouseReleaseOnBackpackEvent onMouseReleaseOnBackpack;
        [SerializeField] private MouseReleaseOnItemUIEvent onMouseReleaseOnItemUI;
        [SerializeField] private UnityEvent onMouseRelease;
        [SerializeField] private MouseMoveEvent onMouseMove;
        [SerializeField] private Camera currentCamera;
        [SerializeField] private float mouseZ = 1f;
        [SerializeField] private float raycastDistance = 16f;
        [SerializeField] private EventSystem eventSystem;

        private readonly List<RaycastResult> _uiRaycastResults = new List<RaycastResult>();
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
            // 3D objects
            var ray = currentCamera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out var hit, raycastDistance, TagsAndLayers.PressableLayers))
            {
                TryInvokeEvent(onPressItemMouse, TagsAndLayers.InventoryItemTag, hit.collider.gameObject);
                TryInvokeEvent(onPressBackpackMouse, TagsAndLayers.InventoryTag, hit.collider.gameObject);
            }
        }

        private void ExecuteMouseRelease(Vector2 mousePosition)
        {
            // UI
            var pointerEventData = new PointerEventData(eventSystem) {position = mousePosition};
            eventSystem.RaycastAll(pointerEventData, _uiRaycastResults);
            foreach (var uiRaycastResult in _uiRaycastResults)
            {
                TryInvokeEvent(onMouseReleaseOnItemUI, TagsAndLayers.InventoryItemUITag, uiRaycastResult.gameObject);
            }
            _uiRaycastResults.Clear();

            // 3D objects
            var ray = currentCamera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out var hit, raycastDistance, TagsAndLayers.ReleasableLayers))
            {
                TryInvokeEvent(onMouseReleaseOnBackpack, TagsAndLayers.InventoryTag, hit.collider.gameObject);
            }

            // no object
            onMouseRelease.Invoke();
        }

        private void TryInvokeEvent<T>(UnityEvent<T> @event, string colliderTag, GameObject hitCollider)
            where T : MonoBehaviour
        {
            if (hitCollider.CompareTag(colliderTag))
            {
                var widget = hitCollider.GetComponent<T>();
                @event.Invoke(widget);
            }
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