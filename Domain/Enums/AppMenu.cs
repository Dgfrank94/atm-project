﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_App.Domain.Enums
{
    public enum AppMenu
    {
        CheckBalance = 1,
        PlaceDeposit,
        MakeWithdrawal,
        SelfTransfer,
        InternalTransfer,
        ViewTransaction,
        Logout
    }
}
