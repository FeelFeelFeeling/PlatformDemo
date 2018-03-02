using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//自定义添加
using WPF.PowerModule.UIStrategy;


namespace WPF.PowerModule.ViewModel
{
    public class UserManagerViewModel : BaseViewModel<UserRUIDUIStrategy>
    {
        public UserManagerViewModel()
            : base(new BridgeService.UserBridgeService())
        {

        }

        #region [数据列表]检索条件，本页面特有的检索条件
        public string LoginId { get; set; }
        public string UserName { get; set; }

        #endregion

        #region 命令方法——检索
        public override dynamic SearchCondition
        {
            get
            {
                dynamic condition = new
                {
                    page = this.PageCount,
                    size = this.PageSize,
                    LoginId = this.LoginId,
                    UserName = this.UserName
                };

                return condition;
            }
        }
        #endregion
    }
}
