using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualServer.PowerService
{
    /// <summary>
    /// 针对Power的服务，隔绝性质的服务，不存在自定义类型，使用Json类型交互
    /// </summary>
    interface IUserRUIDAppService
    {
        /// <summary>
        /// 获取符合条件的用户数量
        /// </summary>
        /// <param name="SearchCondition">？条件</param>
        /// <returns>数量</returns>
        int GetModelsCount(string SearchCondition);
        /// <summary>
        /// 获取符合条件的用户信息集合
        /// </summary>
        /// <param name="SearchCondition">？条件</param>
        /// <returns>集合？元素类型</returns>
        string GetModels(string SearchCondition);
        /// <summary>
        /// 保存（更新存在的信息，不存在创建新的信息）
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        string SaveModel(string json);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="json"></param>
        /// <returns>操作结果</returns>
        string DeleteModels(string json);
    }
}
