﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mailNiftymebis_netcore.Token
{
    public class Token
    {
        public string AccessToken { get; set; }

        public DateTime Expiration { get; set; }

        public string RefreshToken { get; set; }
    }
}
