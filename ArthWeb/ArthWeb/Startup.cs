﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ArthWeb.Startup))]
namespace ArthWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
