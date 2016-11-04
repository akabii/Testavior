﻿namespace GeekLearning.Test.Integration.Environment
{
    using Configuration.Startup;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Net.Http;

    public class TestEnvironment<TStartup, TStartupConfigurationService> : ITestEnvironment
        where TStartup : class
        where TStartupConfigurationService : class, IStartupConfigurationService, new()
    {
        private string contentRootPath;

        public TestServer Server { get; }

        public HttpClient Client { get; }

        public IServiceProvider ServiceProvider { get; private set; }

        public IServiceCollection ServiceCollection { get; private set; }

        public TestEnvironment(string contentRootPath = null)
        {
            this.contentRootPath = contentRootPath;
            this.Server = this.CreateTestServer();
            this.Client = Server.CreateClient();
        }

        protected virtual TestServer CreateTestServer()
        {
            IStartupConfigurationService externalStartupConfigurationService = new TStartupConfigurationService();
            externalStartupConfigurationService.RegisterExternalStartupConfigured(() =>
            {
                ServiceCollection = externalStartupConfigurationService.ServiceCollection;
                ServiceProvider = externalStartupConfigurationService.ServiceProvider;                
            });

            return new TestServer(new WebHostBuilder().ConfigureStartup(externalStartupConfigurationService, this.contentRootPath).UseStartup<TStartup>());
        }
    }
}
