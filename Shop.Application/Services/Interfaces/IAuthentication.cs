using Shop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Services.Interfaces
{
    public interface IAuthentication
    {
        User Validation(string username, string pass);
        string GenerateToken(User user);
    }
}
