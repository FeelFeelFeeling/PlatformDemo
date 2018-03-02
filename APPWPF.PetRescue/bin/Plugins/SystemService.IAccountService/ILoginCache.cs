using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SystemService.IAccountService
{
    /// <summary>
    /// 登录后的缓存
    /// </summary>
    public interface ILoginCache
    {
        /// <summary>
        /// 获取当前用户的已有模块
        /// </summary>
        /// <returns>仅获取URL集合</returns>
        List<string> GetLoginModulesURL();
        bool SetLoginModulesURL(List<string> list);
        /// <summary>
        /// 获取系统所有的模块
        /// </summary>
        /// <returns>模块的对象集合</returns>
        ObservableCollection<ModuleData> GetSystemModulesURL();
        /// <summary>
        /// 获取当前用户的真实姓名
        /// </summary>
        /// <returns></returns>
        string GetCurrentUserName();
        /// <summary>
        /// 获取当前用户的登录ID
        /// </summary>
        /// <returns></returns>
        string GetCurrentLoginId();

        /// <summary>
        /// 获取当前程序的主窗体（弹出窗口等，需要父窗体）
        /// </summary>
        /// <returns></returns>
        Window GetCurrentMainWindow();
    }
}
