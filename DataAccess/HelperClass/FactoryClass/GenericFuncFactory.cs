using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.HelperClass.FactoryClass
{
    
    public static class GenericFuncFactory
    {
        public static Func<P1, POut> GetFunc<P1, POut>( object Func)
            => GetFunc<P1, POut>(new[] { Func });
        public static Func<P1,POut> GetFunc<P1, POut>(object[] FuncT)
        {
            return FuncT.Cast<Func<P1, POut>>().FirstOrDefault();
        }
        public static Func<P1, P2, POut> Func<P1, P2, POut>( object Func)
           => GetFunc<P1, P2, POut>(new[] { Func });
        public static Func<P1, P2, POut> GetFunc<P1, P2, POut>(object[] FuncT)
        {
            return FuncT.Cast<Func<P1, P2, POut>>().FirstOrDefault();
        }
        public static Func<P1, P2,P3, POut> Func<P1, P2, P3, POut>( object Func)
          => GetFunc<P1, P2,P3, POut>(new[] { Func });
        public static Func<P1, P2, P3, POut>GetFunc<P1, P2, P3, POut>(object[] FuncT)
        {
            return FuncT.Cast<Func<P1, P2,P3, POut>>().FirstOrDefault();
        }
    }

}
