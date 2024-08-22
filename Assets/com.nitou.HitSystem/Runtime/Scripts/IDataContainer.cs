
namespace nitou{

    public interface IDataContainer<TData> {

        int Count { get; }

        TData First { get; }

        void Add(TData item);
        void Clear();
        bool Remove(TData item);

        bool Contains(TData item);
    }

}
