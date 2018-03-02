using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Customer.Utility.HttpUtility;
using Customer.Utility;
using System.Collections.ObjectModel;
namespace WPF.PowerModule.BridgeService
{
    /// <summary>
    /// 用户的桥梁服务（负责沟通远程服务）
    /// </summary>
    class UserBridgeService : IRUIDService<UIStrategy.UserRUIDUIStrategy>
    {
        /// <summary>
        /// 获取模型的总数量
        /// </summary>
        /// <param name="SearchCondition">检索条件</param>
        /// <param name="Message">对外返回的提示信息</param>
        /// <returns>模型总数量</returns>
        public int GetModelsCount(dynamic SearchCondition, out string Message)
        {
            Message = string.Empty;
            try
            {
                //访问远程方法
                string postdata = JsonUtility.SerializeObject(SearchCondition);
                //接收返回值
                string result = HttpHelper.Post("", postdata);

                //接受结果的类型
                dynamic obj = new
                {
                    count = 0,
                    message = string.Empty
                };

                JsonUtility.DeserializeAnonymousType<dynamic>(result, obj);
                Message = obj.message;
                return obj.count;
            }
            catch (Exception e)
            {
                Message = string.Format("检索失败，错误：{0}", e.Message);
                return 0;
            }
        }

        public ObservableCollection<UIStrategy.UserRUIDUIStrategy> GetModels(dynamic SearchCondition, out string Message)
        {
            Message = string.Empty;
            try
            {
                //访问远程方法
                string postdata = JsonUtility.SerializeObject(SearchCondition);
                //接收返回值
                string result = HttpHelper.Post("", postdata);

                //接受结果的类型
                dynamic obj = new
                {
                    List = new List<dynamic>(),
                    message = string.Empty
                };

                JsonUtility.DeserializeAnonymousType<dynamic>(result, obj);
                ObservableCollection<UIStrategy.UserRUIDUIStrategy> list = new ObservableCollection<UIStrategy.UserRUIDUIStrategy>();

                //从返回的集合，转换类型并生成新的集合
                foreach (var item in obj.List)
                {

                    UIStrategy.UserRUIDUIStrategy temp = new UIStrategy.UserRUIDUIStrategy();
                    DeepCopyUtility.SetValue<UIStrategy.UserRUIDUIStrategy>(temp, item);
                    list.Add(temp);
                }

                Message = obj.message;
                return list;
            }
            catch (Exception e)
            {
                Message = string.Format("检索失败，错误：{0}", e.Message);
                return new ObservableCollection<UIStrategy.UserRUIDUIStrategy>();
            }
        }

        public string SaveModel(string json)
        {
            return string.Empty;
        }

        public string DeleteModels(string json)
        {
            return string.Empty;
        }
    }
}
