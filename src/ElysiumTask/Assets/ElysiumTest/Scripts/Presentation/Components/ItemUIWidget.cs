using System;
using ElysiumTest.Scripts.Game.Models;
using TMPro;
using UnityEngine;

namespace ElysiumTest.Scripts.Presentation.Components
{
    public class ItemUIWidget: MonoBehaviour
    {
        [SerializeField] private Item item;
        [SerializeField] private TextMeshProUGUI caption;

        private void Start()
        {
            caption.text = item.ItemName;
        }

        public Item Item => item;
    }
}