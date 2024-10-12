namespace DelegatesAndEvents
{
    internal class TestDataClass
    {
        private string _name;
        private float _value;

        public string Name => _name;
        public float Value => _value;

        public TestDataClass(int rangeStart, int rangeEnd)
        {
            if (rangeEnd <= rangeStart)
            {
                throw new ArgumentException("Конец диапазона должен быть больше начала диапазона.");
            }

            Random random = new Random();
            double range = rangeEnd - rangeStart;
            _value = (float)(random.NextDouble() * range + rangeStart);

            _name = $"{nameof(TestDataClass)} - {Value}";
        }
    }
}
