using System;
using System.Collections.Generic;
using ElysiumTest.Scripts.Game.Models;
using ElysiumTest.Scripts.Presentation.Common;
using ElysiumTest.Scripts.Presentation.Interfaces;
using JetBrains.Annotations;
using UnityEngine;

namespace ElysiumTest.Scripts.Presentation.Components
{
    public class BackpackWidget : MonoBehaviour, IBackpack
    {
        [SerializeField] private Backpack model;
        [SerializeField] private InventorySlot[] inventory;
        [SerializeField] private Canvas ui;
        [SerializeReference] private List<IItem> inventoryStash = new List<IItem>();

        public bool TryGetAttachPosition(Item item, out Position attachPosition)
        {
            var slot = FindInventorySlot(item);
            if (slot != null)
            {
                attachPosition = slot.Position;
                return true;
            }

            attachPosition = default;
            return false;
        }

        public void Put(IItem item)
        {
            if (!model.TryPut(item.Item))
            {
                Debug.LogError($"Item with id {item.Item} has been already set");
                return;
            }
            
            var slot = FindInventorySlot(item.Item);
            if (slot == null)
                Debug.LogError($"Slot not found for {item}");
            else
                slot.UIEnabled = true;
            
            inventoryStash.Add(item);
        }

        public bool TryTakeAway(Item item, out IItem itemWidget)
        {
            if (TryFindWidget(item, out itemWidget) && model.TryTake(item.ID, out _))
            {
                var slot = FindInventorySlot(item);
                
                if (slot == null)
                    Debug.LogError($"Slot not found for {item}");
                else
                    slot.UIEnabled = false;

                inventoryStash.Remove(itemWidget);
                return true;
            }

            return false;
        }

        private bool TryFindWidget(Item item, out IItem itemWidget)
        {
            foreach (var inventoryWidget in inventoryStash)
            {
                if (inventoryWidget.Item == item)
                {
                    itemWidget = inventoryWidget;
                    return true;
                }
            }

            itemWidget = default;
            return false;
        }

        public void ShowUI() => ui.gameObject.SetActive(true);

        public void HideUI() => ui.gameObject.SetActive(false);

        [CanBeNull]
        private InventorySlot FindInventorySlot(Item item)
        {
            foreach (var slot in inventory)
            {
                if (slot.Item == item)
                {
                    return slot;
                }
            }

            return null;
        }

        [Serializable]
        public class InventorySlot
        {
            [SerializeField] private Item item;
            [SerializeField] private Transform attachPointObj;
            [SerializeField] private ItemUIWidget uiWidget;

            public Item Item => item;

            public Position Position => new Position(attachPointObj.position, attachPointObj.rotation);

            public bool UIEnabled
            {
                get => uiWidget.gameObject.activeSelf;
                set => uiWidget.gameObject.SetActive(value);
            }
        }
    }
}