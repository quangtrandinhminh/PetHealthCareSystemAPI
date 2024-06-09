using BusinessObject.Entities;
using DataAccessLayer.Base;

namespace DataAccessLayer.DAO;

public class PetDao : BaseDao<Pet>
{
    public static readonly AppDbContext _context = new();
}