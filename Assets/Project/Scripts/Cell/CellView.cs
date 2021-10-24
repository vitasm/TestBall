using System.Collections;
using UnityEngine;

namespace Scripts.Cell
{
    public class CellView : CellObject
    {
        [SerializeField] private GameObject _crystal;
        public CellView NextCell { get { return _nextCell; } }
        private CellView _nextCell;
        public bool Cryslal { get { return _haveCrystal; } }
        private bool _haveCrystal;

        public void ShowCrystal(bool visible)
        {
            _haveCrystal = visible;
            _crystal.SetActive(visible);
        }

        public void SetNext(CellView next)
        {
            _nextCell = next;
        }

        public override IEnumerator PlayHide(bool fast = false)
        {
            ShowCrystal(false);
            return base.PlayHide(fast);
        }
    }
}
