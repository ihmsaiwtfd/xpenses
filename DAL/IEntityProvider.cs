using Core;

namespace DAL
{
    public interface IEntityProvider<T>
        where T: EntityBase
    {
        T Cast();
    }
}
