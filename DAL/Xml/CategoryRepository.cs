using Core;

namespace DAL.Xml
{
    internal class CategoryRepository : RepositoryBase<Category, Data.Category>
    {
        protected override string FileName => _CategoriesFileName;
    }
}
