using System;
using ElysiumTest.Scripts.Presentation.Common;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ElysiumTest.Scripts.Presentation.Controllers
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] private Camera currentCamera;
        
        // todo: use PlayerInput to get input indirectly
        private void FixedUpdate()
        {
            var mouse = Mouse.current;
            if (mouse == null)
                return;
            
            
            
            if (mouse.leftButton.isPressed)
            {
                var mousePosition = mouse.position.ReadValue();
                Ray ray = currentCamera.ScreenPointToRay(mousePosition);
                Debug.DrawRay(ray.origin, ray.direction);
                if (Physics.Raycast(ray, out var hit))
                {
                    if (hit.collider.CompareTag(Tags.InventoryItem))
                    {
                        print(hit.collider.gameObject.name);
                    }
                    
                    //hit.collider.CompareTag("")
                    // var interactable = hit.collider.GetComponent<PlayerInteractableComponent>();
                    // Character passive = null;
                    // if (!ReferenceEquals(null, interactable))
                    //     passive = interactable.Character;

                    // var position = new Position(hit.point);
                    // var success = playerInteractionsService.TryFindAndSetBehavior(character, passive, position);
                    // if (!success) 
                    //     Debug.LogWarning($"Can't interact by {character} with {passive} at {position}");
                }
                
            }

        }
    }
}
