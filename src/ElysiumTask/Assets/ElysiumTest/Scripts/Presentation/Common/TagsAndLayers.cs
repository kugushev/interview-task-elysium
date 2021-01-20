using UnityEngine;

namespace ElysiumTest.Scripts.Presentation.Common
{
    internal static class TagsAndLayers
    {
        #region Tags

        internal const string InventoryTag = "Inventory";
        internal const string InventoryItemTag = "InventoryItem";
        internal const string InventoryItemUITag = "InventoryItemUI";

        #endregion

        #region Layers

        internal const string DefaultLayer = "Default";
        internal const string BackpackLayer = "Backpack";
        internal const string BackpackItemLayer = "BackpackItem";

        #endregion

        #region Rules

        public static int PressableLayers { get; } = LayerMask.GetMask(DefaultLayer, BackpackLayer);

        public static int ReleasableLayers { get; } = LayerMask.GetMask(BackpackLayer);

        #endregion
    }
}