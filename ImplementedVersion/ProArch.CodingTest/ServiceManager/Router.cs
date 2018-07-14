using System;
using System.Collections.Generic;
using System.Text;

namespace ProArch.CodingTest.ServiceManager
{
    public class Router<TContract, TOnline, TOffline>
         where TOnline : TContract
         where TOffline : TContract
    {
        public TContract Logger;

        public Router()
        {
            //Logger = Unity.Container.Resolve<TContract>();
        }
    }

}
