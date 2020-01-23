using System;
using System.Collections.Generic;

namespace Subscription
{
    [Serializable]
    public class DataBoundary
    {
        public string CommandName { get; set; }
        public string TypeName { get; set; }
        public Dictionary<object, object> Attributes { get; set; }
    }

}
