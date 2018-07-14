using System;
using System.Collections.Generic;
using System.Text;
using Unity;
using Unity.Interception;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;

namespace ProArch.CodingTest.ServiceManager
{
    public class Unity
    {
        public static IUnityContainer Container;

        public static void Initialize()
        {
            Container = new UnityContainer();

            Container.AddNewExtension<Interception>();
            Container.RegisterType<ILogger, OnlineLogger>();
            Container.Configure<Interception>().SetInterceptorFor<ILogger>(new InterfaceInterceptor());
        }


    }
}
