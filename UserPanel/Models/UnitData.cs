namespace UserPanel.Models
{
    public struct UnitData<T> where T : struct
    {
        public UnitData(T value, string date)
        {
            Date = date;
            Value = value;
        }
        public string Date { get; set; }
        public T Value { get; set; }


    }
}
