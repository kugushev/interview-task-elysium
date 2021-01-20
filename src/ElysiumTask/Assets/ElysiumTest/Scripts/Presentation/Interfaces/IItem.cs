using Cysharp.Threading.Tasks;
using ElysiumTest.Scripts.Game.Models;

namespace ElysiumTest.Scripts.Presentation.Interfaces
{
    public interface IItem
    {
        Item Item { get; }
        UniTask Drag(IInputInfo inputInfo);
        UniTask DropToBackpack(IInputInfo inputInfo, IBackpack backpack);
        UniTask Drop(IInputInfo inputInfo);
    }
}