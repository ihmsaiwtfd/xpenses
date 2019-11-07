using Core;

namespace DAL.Xml
{
    internal class CategoryRelationRepository : RepositoryBase<CategoryRelation, Data.CategoryRelationship>
    {
        protected override string FileName => _CatRelationshipFileName;
    }
}
