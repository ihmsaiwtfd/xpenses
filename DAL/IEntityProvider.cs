using Core;

namespace DAL
{
    internal interface IEntityProvider<T>
        where T: EntityBase
    {
        T Cast();
        void From(T source);
    }
}
