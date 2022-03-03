using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;

namespace ToDo_Mvc_App.Helper
{
    public class ToDoAPI
    {
        IConfiguration _configuration;

        public ToDoAPI(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public HttpClient Init()
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("ToDoApiUrl"));
                return client;
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }
    }
}