using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BusinessObject.Entities.Base;
using System.Collections.Generic;

namespace DataAccessLayer.Base
{
    public class BaseDao<T> where T : BaseEntity, new()
    {
        
    }
}
