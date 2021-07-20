using System.Collections.Generic;
using UnityEngine;

namespace TestInvaders.Level
{
    public class VanguardBehaviour
    {
        private int _row;
        private int _column;

        private CharacterBehaviour[,] _characters;
        private List<VanguardBehaviour> _vanguard;

        public Vector3 Position => _characters[_row, _column].transform.position;
        
        public VanguardBehaviour(int row, int column, CharacterBehaviour[,] characters, List<VanguardBehaviour> vanguard)
        {
            _row = row;
            _column = column;
            
            _characters = characters;
            _vanguard = vanguard;
            
            _characters[_row, _column].OnDestroy += OnCharacterDestroyed;
        }

        private void OnCharacterDestroyed(CharacterBehaviour character)
        {
            _characters[_row, _column].OnDestroy -= OnCharacterDestroyed;

            if (NextRow())
            {
                _characters[_row, _column].OnDestroy += OnCharacterDestroyed;
            }
        }

        private bool NextRow()
        {
            _row--;
            if (_row < 0)
            {
                _vanguard.Remove(this);
                _vanguard = null;
                _characters = null;
                return false;
            }

            if (!_characters[_row, _column].IsAlive)
            {
                return NextRow();
            }

            return true;
        }

        public void Shoot()
        {
            _characters[_row, _column].Shoot();
        }
    }
}