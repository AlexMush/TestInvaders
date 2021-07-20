using System.Threading.Tasks;
using TestInvaders.Config;
using UnityEngine;

namespace TestInvaders.Components
{
    public class ConfigComponent : IContextComponent
    {
        public ObjectConfig ObjectConfig { get; private set; }
        public GameConfig GameConfig { get; private set; }
        public CharacterConfig PlayerConfig { get; private set; }
        public CharacterConfig NpcConfig { get; private set; }
        
        public void Initialize(IContext context)
        {
            ObjectConfig = Resources.Load<ObjectConfig>("Configs/ObjectConfig");
            GameConfig = Resources.Load<GameConfig>("Configs/GameConfig");
            PlayerConfig = Resources.Load<CharacterConfig>("Configs/PlayerConfig");
            NpcConfig = Resources.Load<CharacterConfig>("Configs/NpcConfig");
        }

        public Task Load()
        {
            return Task.CompletedTask;
        }
    }
}