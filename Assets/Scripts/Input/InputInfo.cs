using UnityEngine;

namespace TestInvaders.Input
{
    public class InputInfo
    {
        public Vector2 MoveDir;
        public bool IsShooting;

        public void Reset()
        {
            MoveDir = Vector2.zero;
            IsShooting = false;
        }
    }
}