using UserPanel.Models.Adverts;

namespace UserPanel.Helpers
{
    public static class AdvertFormatHelper
    {
        private static Dictionary<string, int> SortedOrder = new Dictionary<string, int>()
        {
            { "300x300", 1 },
            { "300x600", 2 },
        };
        
        public static void SortAndFill<T>(List<T> formats, int size) where T : AdvertFormatForm, new()
        {
            int currSize = formats.Count;
            for (int i = currSize; i < size; i++)
            {
                formats.Add(new T());
            }

            formats.Sort((one,two) => {

                string sizeOne = string.IsNullOrEmpty(one.Size) ? "default" : one.Size;
                string sizeTwo = string.IsNullOrEmpty(two.Size) ? "default" : two.Size;

                int orderOne = SortedOrder.ContainsKey(sizeOne) ? SortedOrder[sizeOne] : int.MaxValue;
                int orderTwo = SortedOrder.ContainsKey(sizeTwo) ? SortedOrder[sizeTwo] : int.MaxValue;

                return orderOne.CompareTo(orderTwo);
            });
        }
    }
}
