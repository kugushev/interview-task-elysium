using System;
using ElysiumTest.Scripts.Game.Models;
using ElysiumTest.Scripts.Presentation.Common;
using ElysiumTest.Scripts.Presentation.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace ElysiumTest.Scripts.Presentation.Components
{
    public class BackpackWidget : MonoBehaviour, IBackpack
    {
        [SerializeField] private Backpack model;
        [SerializeField] private AttachPoint[] attachPoints;
        [SerializeField] private Canvas ui;
        
        public bool TryGetAttachPosition(Item item, out Position attachPosition)
        {
            foreach (var attachPoint in attachPoints)
            {
                if (attachPoint.Item == item)
                {
                    attachPosition = attachPoint.Position;
                    return true;
                }
            }

            attachPosition = default;
            return false;
        }

        public void Put(Item item)
        {
            if (!model.TryPut(item)) 
                Debug.LogError($"Item with id {item.ID} has been already set");
        }

        public void ShowUI() => ui.gameObject.SetActive(true);

        public void HideUI() => ui.gameObject.SetActive(false);

        [Serializable]
        public class AttachPoint
        {
            [SerializeField] private Item item;
            [SerializeField] private Transform attachPointObj;

            public Item Item => item;

            public Position Position => new Position(attachPointObj.position, attachPointObj.rotation);
        }
    }
}