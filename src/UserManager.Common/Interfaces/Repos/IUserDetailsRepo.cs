﻿using UserManager.Repo.Entities;

namespace UserManager.Common.Interfaces.Repos
{
    public interface IUserDetailsRepo
    {
        Task<UserDetails> GetUserDetailsAsync(int userId);
    }
}
