using AutoMapper;

namespace UserPanel.Helpers
{
    public static class MapperExtensions<T,S>
    {
        public static List<T> MappLists(IMapper mapper, List<S> source)
        {
            List<T> list = new List<T>();
            foreach (S s in source) { 
                list.Add(mapper.Map<T>(s));
            }
            return list;
        }
    }
}
