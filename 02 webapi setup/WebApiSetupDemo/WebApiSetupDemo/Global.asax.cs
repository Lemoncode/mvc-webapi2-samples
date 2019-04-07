﻿using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace WebApiSetupDemo
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}