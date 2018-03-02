using System;
using System.Collections.Generic;
using System.Text;
using UIShell.OSGi;

namespace VirtualServer.PowerService
{
    public class Activator : IBundleActivator
    {
        public void Start(IBundleContext context)
        {
            context.AddService<IUserRUIDAppService>(new AppService.UserRUIDAppService());
        }

        public void Stop(IBundleContext context)
        {
            //todo:
        }
    }
}
