using UnityEngine;

namespace TestInvaders.Input
{
    public class StandaloneInput
    {
        private readonly InputInfo _input = new InputInfo();

        public InputInfo Update()
        {
            _input.Reset();

            if (UnityEngine.Input.GetKey(KeyCode.A) || UnityEngine.Input.GetKey(KeyCode.LeftArrow))
            {
                _input.MoveDir = Vector2.left;
            }
            
            if (UnityEngine.Input.GetKey(KeyCode.D) || UnityEngine.Input.GetKey(KeyCode.RightArrow))
            {
                _input.MoveDir = Vector2.right;
            }
            
            if (UnityEngine.Input.GetKey(KeyCode.Space))
            {
                _input.IsShooting = true;
            }

            return _input;
        }
    }
}