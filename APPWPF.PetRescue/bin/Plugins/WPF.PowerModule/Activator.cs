using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using UIShell.OSGi;

using SystemService.IAccountService;
namespace WPF.PowerModule
{
    public class Activator : IBundleActivator
    {
        internal static IBundleContext CustomerContext;
        internal static ILoginCache LoginCache;
        public void Start(IBundleContext context)
        {
            //加载账号服务
            if (context != null)
            {
                //第一步：保存插件上下文
                CustomerContext = context;

                //第二步：获取账号服务(检查当前模块是否在合法菜单内)
                LoginCache = CustomerContext.GetFirstOrDefaultService<SystemService.IAccountService.ILoginCache>();

                //第三步：监听系统账号服务
                CustomerContext.ServiceChanged += CustomerContext_ServiceChanged;
            }

            //加载字典
            //Application firstOrDefaultService = context.GetFirstOrDefaultService<Application>();
            //if (firstOrDefaultService != null)
            //{
            //    try
            //    {
            //        firstOrDefaultService.Resources = new ResourceDictionary
            //        {
            //            MergedDictionaries = 
            //            {
            //                new ResourceDictionary
            //                {
            //                    Source = new Uri("/FirstFloor.ModernUI,Version=1.0.9.0;component/Assets/ModernUI.xaml", UriKind.RelativeOrAbsolute)
            //                },
            //                new ResourceDictionary
            //                {
            //                    Source = new Uri("/FirstFloor.ModernUI,Version=1.0.9.0;component/Assets/ModernUI.Light.xaml", UriKind.RelativeOrAbsolute)
            //                }
            //            }
            //        };
            //    }
            //    catch
            //    {
            //    }
            //}
        }

        //监控服务的变化
        void CustomerContext_ServiceChanged(object sender, ServiceEventArgs e)
        {
            if (CustomerContext != null && e.ServiceType == "SystemService.IAccountService.ILoginCache")
            {
                if (e.ServiceEventType == ServiceEventType.Add)
                {
                    LoginCache = CustomerContext.GetFirstOrDefaultService<SystemService.IAccountService.ILoginCache>();
                }
                if (e.ServiceEventType == ServiceEventType.Remove)
                {
                    LoginCache = null;
                }
            }
        }
        public void Stop(IBundleContext context)
        {
            try
            {
                if (CustomerContext != null)
                {
                    //移除服务监听
                    CustomerContext.ServiceChanged -= CustomerContext_ServiceChanged;
                    //清空缓存的上下文
                    CustomerContext = null;
                }
            }
            catch { }
        }
    }
}
