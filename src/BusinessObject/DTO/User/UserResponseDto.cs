﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO.User
{
    public class UserResponseDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public string Role { get; set; }
    }
}
