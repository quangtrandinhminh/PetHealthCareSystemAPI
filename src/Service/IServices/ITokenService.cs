using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Entities.Identity;

namespace Service.IServices
{
    public interface ITokenService
    {
        string CreateToken(UserEntity userEntity);
    }
}