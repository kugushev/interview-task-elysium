using ElysiumTest.Scripts.Game.Models;
using ElysiumTest.Scripts.Presentation.Common;

namespace ElysiumTest.Scripts.Presentation.Interfaces
{
    public interface IBackpack
    {
        bool TryGetAttachPosition(Item item, out Position attachPosition);

        void Put(Item item);
    }
}