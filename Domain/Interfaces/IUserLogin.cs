using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_App.Domain.Interfaces
{
    public interface IUserLogin
    {
        void CheckUserCardNumberandPassword();
    }
}
