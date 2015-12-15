using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace WebApi2.Demo.Inspector
{
   
    public class BaseClientIdentityCheck
    {

        protected bool IsIdentityPass { get; set; }
        protected string ClientTokenName { get; set; }
        protected string ClientToken { get; set; }
        public BaseClientIdentityCheck() { CheckIdentity(); }
        public virtual void CheckIdentity() { }

    }
}
