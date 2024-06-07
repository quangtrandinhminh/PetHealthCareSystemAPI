﻿using BusinessObject.DTO.User;
using BusinessObject.Entities.Identity;
using Riok.Mapperly.Abstractions;

namespace BusinessObject.Mapper;

[Mapper]
public partial class MapperlyMapper
{
    //  public partial entity Map(dto request); --for create
    // public partial dtoresponse Map(entity entity); --for get
    // public partial IList<dtoresponse> Map(IList<entity> entity); --for get all
    // public partial void Map(dto request, entity entity); --for update

    // user
    public partial UserEntity Map(RegisterDto request);
    public partial UserResponseDto Map(UserEntity entity);
    public partial IList<UserResponseDto> Map(IList<UserEntity> entity);
    public partial void Map(RegisterDto request, UserEntity entity);

}