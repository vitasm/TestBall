using UnityEngine;

namespace Scripts.Crystal
{
    public class RandomAlgorithm : GetCrystals
    {
        private int _indexRandom;
        private int _index;
        private int _sizeBlock;

        public override bool GetCrystal()
        {
            _index++;

            var getCrustal = _index == _indexRandom;

            if (_index == _sizeBlock)
                NewRandonIndex();

            return getCrustal;
        }

        private void NewRandonIndex()
        {
            _index = 0;
            _indexRandom = Random.Range(0, _sizeBlock + 1);
        }

        public override void Init(int sizeBlock)
        {
            _sizeBlock = sizeBlock;

            NewRandonIndex();
        }
    }
}