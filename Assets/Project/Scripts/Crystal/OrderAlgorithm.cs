namespace Scripts.Crystal
{
    public class OrderAlgorithm : GetCrystals
    {
        private int _indexCrystal;
        private int _offsetIndex;
        private int _index;
        private int _sizeBlock;

        public override bool GetCrystal()
        {
            _index++;
            
            var getCrustal = _index == _indexCrystal;
            if (_index == _sizeBlock)
                NewIndex();

            return getCrustal;
        }

        private void NewIndex()
        {
            _offsetIndex++;
            if (_offsetIndex > _sizeBlock)
                _offsetIndex = 1;
            _index = 0;
            _indexCrystal = _offsetIndex;
        }

        public override void Init(int sizeBlock)
        {
            _sizeBlock = sizeBlock;

            _index = 0;
            _indexCrystal = 1;
            _offsetIndex = 1;
        }
    }
}