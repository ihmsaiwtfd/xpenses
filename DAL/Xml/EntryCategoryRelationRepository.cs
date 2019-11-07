using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Xml
{
    internal class EntryCategoryRelationRepository : RepositoryBase<Core.EntryCategoryRelation, Data.EntryCategoryRelationship>
    {
        protected override string FileName => _EntryCatRelationshipFileName;
    }
}
