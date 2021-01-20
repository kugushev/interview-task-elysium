using System;
using System.Collections.Generic;
using ElysiumTest.Scripts.Game.Models;
using ElysiumTest.Scripts.Presentation.Common;
using ElysiumTest.Scripts.Presentation.Interfaces;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace ElysiumTest.Scripts.Presentation.Components
{
    public class BackpackWidget : MonoBehaviour, IBackpack
    {
        [SerializeField] private Backpack model;
        [SerializeField] private InventorySlot[] inventory;
        [SerializeField] private Canvas ui;
        [SerializeReference] private List<ItemWidget> widgetsStash = new List<ItemWidget>();

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

        public void Put(ItemWidget item)
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
            
            widgetsStash.Add(item);
        }

        public bool TryTakeAway(Item item, out ItemWidget itemWidget)
        {
            if (TryFindWidget(item, out itemWidget) && model.TryTake(item.ID, out _))
            {
                var slot = FindInventorySlot(item);
                
                if (slot == null)
                    Debug.LogError($"Slot not found for {item}");
                else
                    slot.UIEnabled = false;

                widgetsStash.Remove(itemWidget);
                return true;
            }

            return false;
        }

        private bool TryFindWidget(Item item, out ItemWidget itemWidget)
        {
            foreach (var inventoryWidget in widgetsStash)
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