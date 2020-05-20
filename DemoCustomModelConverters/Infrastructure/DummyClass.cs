using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;

namespace DemoCustomModelConverters.Infrastructure
{
    public class DummyClass : IDummyInterface
    {

        public DummyClass()
        {
            SomeText = "Hello world from the dummy class";
        }

        public string SomeText { get; set; }
    }
}