﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class AppConfigurations
    {
        public const string SectionName = "AppConfigurations";
        public JWTConfigurations JWTConfigurations { get; set; }

        public string ConnectionStrings { get; set; }
    }

    public class JWTConfigurations
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Secret { get; set; }
    }
}
