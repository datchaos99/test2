using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MySql.Data.MySqlClient;
using System.Net.Http.Headers;

namespace WebAPI
{
    public static class WebApiConfig
    {

        public static MySqlConnection conn()
        {
            const string server_name = "sql6.freemysqlhosting.net";
            const string port = "3306";
            const string db_name = "sql6414064";
            const string user_name = "sql6414064";
            const string pwd = "TmP3kT8bbJ";
            string conn_string = $"server={server_name};port={port};database={db_name};username={user_name};password={pwd}";
            //string conn_string = "server=sql100.epizy.com;port=3306;database=epiz_28643236_device_control;username=epiz_28643236;password=ltncbtl;";
            //string conn_string = "server=localhost; port=3306;database=datk1;username=root;password=;";
            MySqlConnection conn = new MySqlConnection(conn_string);

            return conn;
        }

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //config default data type is json, not xml
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { controller="tbled", action="getall" }
            );

        }


    }
}
