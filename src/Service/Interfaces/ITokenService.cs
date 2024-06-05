using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Entities;

namespace Service.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}