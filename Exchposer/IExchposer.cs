﻿using System;
namespace Exchposer
{
    public interface IExchposer
    {
        void OfflineSync(DateTime syncFromTime, DateTime syncToTime);
        bool AutoRun { get; set; }
    }
}
