using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Services.Interfaces
{
    public interface ICacheService
    {
        //chera generic
        //chon badan category ,... ham momkene ezafe konim
        //in 2 method ra khodemoon neveshtim
        //method redis nistan
        //vali dakheleshon az method haye redis estefade mikonim
        void Set<T>(string key, T value, TimeSpan? expiry = null);//sakhte klid

        T? Get<T>(string key);//gereftan data

        void Remove(string key);//hazf Cache

        int GetVersion(string key);//daryaft version

        long IncrementVersion(string key);//afzayesh version
    }
}
