﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WScan.Shared;

namespace WScan.Service
{
    public interface IScannerService
    {
        public List<Scanner> GetScanners();
    }
}
