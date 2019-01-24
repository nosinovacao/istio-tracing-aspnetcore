using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Istio.Tracing.Propagation.Tests
{
    public class MockHeaderDictionary : Dictionary<string, StringValues>, IHeaderDictionary
    {
        public long? ContentLength { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
