namespace DelegatesAndEvents
{
    internal static class IEnumerableExtensions
    {
        public static T GetMax<T>(this IEnumerable<T> collection, Func<T, float> convertToNumber) where T : class
        {
            T maxElement = null;
            float maxValue = float.MinValue;

            foreach (var item in collection)
            {
                float value = convertToNumber(item);

                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = item;
                }
            }

            return maxElement;
        }

        public static T GetMin<T>(this IEnumerable<T> collection, Func<T, float> convertToNumber) where T : class
        {
            T minElement = null;
            float minValue = float.MaxValue;

            foreach (var item in collection)
            {
                float value = convertToNumber(item);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = item;
                }
            }

            return minElement;
        }

        public static float GetAverage<T>(this IEnumerable<T> collection, Func<T, float> convertToNumber) where T : class
        {
            float sum = 0;
            int count = 0;

            foreach (var item in collection)
            {
                sum += convertToNumber(item);
                count++;
            }

            return count == 0 ? 0 : sum / count;
        }
    }
}
