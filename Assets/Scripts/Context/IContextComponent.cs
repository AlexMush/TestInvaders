using System.Threading.Tasks;

namespace TestInvaders
{
    public interface IContextComponent
    {
        void Initialize(IContext context);
        Task Load();
    }
}