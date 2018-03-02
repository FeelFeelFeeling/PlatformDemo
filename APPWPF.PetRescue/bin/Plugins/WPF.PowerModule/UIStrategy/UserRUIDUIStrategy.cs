using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Customer.Runtime.UIAttributes;
using Customer.Runtime.ValidAttributes;
namespace WPF.PowerModule.UIStrategy
{
    /// <summary>
    /// 用户增删改查的UI策略
    /// </summary>
    public class UserRUIDUIStrategy : BaseUI
    {
        #region 特有业务属性(有解析说明、支持扩展)
        public int CloOrderBy { get; set; }//列表显示时的排序
        public int EditOrderBy { get; set; }//编辑时的排序

        [UIName(Name = "序号")]
        [UIIsListShow(IsListShow = true)]
        [UIListControlType(ControlType = Endic_ShowControlType.文本)]
        public int NUM { get; set; }//序号

        [UIName(Name = "用户头像")]
        [UIIsListShow(IsListShow = true)]
        [UIListControlType(ControlType = Endic_ShowControlType.自定义图片)]
        [UIIsEditInput(IsEditInput = true)]
        [UIEditControlType(ControlType = EnDic_EditControlType.选取文件)]
        [ValidFile]
        public string HeaderImage { get; set; }

        [UIName(Name = "最新文件")]
        [UIIsListShow(IsListShow = true)]
        [UIListControlType(ControlType = Endic_ShowControlType.自定义图片)]
        [UIIsEditInput(IsEditInput = true)]
        [UIEditControlType(ControlType = EnDic_EditControlType.选取文件)]
        [ValidFile]
        public string LastImage { get; set; }

        [UIName(Name = "登录账号")]
        [UIIsListShow(IsListShow = true)]
        [UIIsEditInput(IsEditInput = true)]
        [UIEditControlType(ControlType = EnDic_EditControlType.文本框)]
        [ValidNotNull]
        [ValidLengthMax(50)]
        public string LoginId { get; set; }

        [UIName(Name = "登录密码")]
        [UIIsEditInput(IsEditInput = true)]
        [UIEditControlType(ControlType = EnDic_EditControlType.文本框)]
        [ValidNotNull]
        [ValidLengthMax(50)]
        public string Password { get; set; }

        [UIName(Name = "账号状态")]
        [UIIsListShow(IsListShow = true)]
        [UIIsEditInput(IsEditInput = true)]
        [UIEditControlType(ControlType = EnDic_EditControlType.字典下拉框)]
        [ValidNotNull]
        public string Dic_AccountState { get; set; }

        [UIName(Name = "真实姓名")]
        [UIIsListShow(IsListShow = true)]
        [UIIsEditInput(IsEditInput = true)]
        [UIEditControlType(ControlType = EnDic_EditControlType.文本框)]
        [ValidNotNull]
        [ValidLengthMax(20)]
        public string Name { get; set; }

        [UIName(Name = "年龄")]
        [UIIsListShow(IsListShow = true)]
        [UIIsEditInput(IsEditInput = true)]
        [UIEditControlType(ControlType = EnDic_EditControlType.文本框)]
        [ValidNotNull]
        [ValidValueRang(1, 1000)]
        public int Age { get; set; }

        [UIName(Name = "手机")]
        [UIIsListShow(IsListShow = true)]
        [UIIsEditInput(IsEditInput = true)]
        [UIEditControlType(ControlType = EnDic_EditControlType.文本框)]
        [ValidNotNull]
        [ValidValueRang(8, 11)]
        public string Phone { get; set; }

        [UIName(Name = "微信")]
        [UIIsListShow(IsListShow = true)]
        [UIIsEditInput(IsEditInput = true)]
        [UIEditControlType(ControlType = EnDic_EditControlType.文本框)]
        [ValidNotNull]
        [ValidLengthRang(1, 100)]
        public string WXCode { get; set; }

        [UIName(Name = "性别")]
        [UIIsListShow(IsListShow = true)]
        [UIIsEditInput(IsEditInput = true)]
        [UIEditControlType(ControlType = EnDic_EditControlType.字典下拉框)]
        [ValidNotNull]
        public string Dic_Sex { get; set; }

        [UIName(Name = "身份证")]
        [UIIsListShow(IsListShow = true)]
        [UIIsEditInput(IsEditInput = true)]
        [UIEditControlType(ControlType = EnDic_EditControlType.文本框)]
        [ValidNotNull]
        [ValidLengthRang(15, 18)]
        public string NOCode { get; set; }

        [UIName(Name = "备注")]
        [UIIsListShow(IsListShow = true)]
        [UIIsEditInput(IsEditInput = true)]
        [UIEditControlType(ControlType = EnDic_EditControlType.文本框)]
        [ValidLengthMax(300)]
        public string Remark { get; set; }

        #endregion
    }
}
