﻿using BusinessObject.Entities;
using Repository.Base;
using Repository.Interfaces;

namespace Repository.Repositories;

public class PetRepository : BaseRepository<Pet>, IPetRepository
{
    
}