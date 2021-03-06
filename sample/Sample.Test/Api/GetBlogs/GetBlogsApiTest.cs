﻿namespace GeekLearning.Testavior.Sample.Test.Api.GetBlogs
{
    using GeekLearning.Testavior.Sample.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Newtonsoft.Json;
	using System.Collections.Generic;
    using System.Net.Http;

    [TestClass]
    public class GetBlogsApiTest : BaseTestClass
    {
        [TestMethod]
        public void Api_GetBlogsShouldBeOk()
        {
            base.CreateBlogs();

            var response = base.TestEnvironment.Client.GetAsync("/api/blogs").Result;
            response.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<Blog[]>(response.Content.ReadAsStringAsync().Result);

            var expectedResult = new List<Blog>
            {
                new Blog { Url = "http://blog1.io" },
                new Blog { Url = "http://blog2.io" },
                new Blog { Url = "http://blog3.io" }
            };

            expectedResult.IsEqual(result, ignoredProperties: "BlogId");
        }
    }
}
