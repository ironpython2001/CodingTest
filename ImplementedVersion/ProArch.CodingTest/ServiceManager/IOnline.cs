﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ProArch.CodingTest.ServiceManager
{
    public interface IOnline
    {
        IOffline Offline { get; set; }
    }
}
