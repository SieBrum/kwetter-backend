﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.DTO
{
    public class VerifyEmailTokenModel: LoginModel
    {
        public Guid VerifyEmailToken { get; set; }
    }
}
