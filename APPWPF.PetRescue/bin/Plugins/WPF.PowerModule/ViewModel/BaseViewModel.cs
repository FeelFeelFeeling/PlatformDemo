using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FirstFloor.ModernUI.Presentation;
using System.Collections.ObjectModel;
using Component.PageState;
using Component.ShowMessage;
using Customer.Utility;
using FirstFloor.ModernUI.Windows.Controls;
using Component.EditControl;
using Customer.Runtime.UIAttributes;
using Customer.Runtime.ValidAttributes;
namespace WPF.PowerModule.ViewModel
{
    public abstract class BaseViewModel<RUID> : UserControl, INotifyPropertyChanged
        where RUID : UIStrategy.BaseUI, new()//确定RUID必须继承自BaseUI，并且有无参构造
    {
        public BridgeService.IRUIDService<RUID> Service { get; set; }

        #region UI命令与定义、构造方法
        public ICommand SearchCmd { get; set; }
        public ICommand AddCmd { get; set; }
        public ICommand ModifyCmd { get; set; }
        public ICommand DeleteCmd { get; set; }
        public ICommand SaveCmd { get; set; }
        public BaseViewModel(BridgeService.IRUIDService<RUID> service)
        {
            //==============服务类
            this.Service = service;

            //=======================定义命令
            //增删改查命令
            SearchCmd = new RelayCommand(new Action<object>((par) => { this.Query(); }), null);
            AddCmd = new RelayCommand(new Action<object>((par) => { this.AddModel(par); }), null);
            ModifyCmd = new RelayCommand(new Action<object>((par) => { this.ModifyModel(); }), null);
            DeleteCmd = new RelayCommand(new Action<object>((par) => { this.DeleteModels(); }), null);

            SaveCmd = new RelayCommand(new Action<object>((par) => { this.SaveModel(); }), null);
            //初始化必要属性
            this.ItemSource = new ObservableCollection<RUID>();

            //域模型处理
            this.Query();//异步加载数据列表
        }
        #endregion

        #region 页面反馈的绑定属性——状态控件的监控点、信息提示的监控点
        //页面状态(初始化)
        private EnDic_PageState state = EnDic_PageState.默认状态;//默认状态
        public EnDic_PageState PageState
        {
            get { return state; }
            set
            {
                if (state != value)
                {
                    state = value;
                    this.Notify("PageState");
                }
            }
        }

        //消息提示(初始化)
        private CustomerMessageData result = new CustomerMessageData(EnDic_MessageType.忽视此消息
            , "系统提示"
            , string.Empty
            , null);
        //自定义消息类型（为页面状态控制控件，提供绑定属性）
        public CustomerMessageData ResultMessage
        {
            get { return result; }
            set
            {
                if (result != value)
                {
                    result = value;
                    this.Notify("ResultMessage");
                }
            }
        }
        #endregion

        #region 分页控件的绑定属性——总信息数，页码，每页数量
        //====================分页相关
        private int infoCount;
        public int TotalInfoCount
        {
            get { return this.infoCount; }
            set
            {
                if (this.infoCount != value)
                {
                    this.infoCount = value;
                    this.Notify("TotalInfoCount");
                }
            }
        }
        private int count = 1;
        public int PageCount
        {
            get { return this.count; }
            set
            {
                if (this.count != value)
                {
                    this.count = value;
                    this.Notify("PageCount");
                    this.Query();
                }
            }
        }
        private int size = 10;
        public int PageSize
        {
            get { return this.size; }
            set
            {
                if (this.size != value)
                {
                    this.size = value;
                    this.Notify("PageSize");
                    this.Query();
                }
            }
        }
        #endregion

        #region [数据列表]的集合、当前选项、新增模型、修改模型

        private ObservableCollection<RUID> source = new ObservableCollection<RUID>();
        /// <summary>
        /// 数据列表的集合
        /// </summary>
        public ObservableCollection<RUID> ItemSource
        {
            get { return this.source; }
            set
            {
                if (this.source != value)
                {
                    this.source = value;
                    Notify("ItemSource");
                }
            }
        }
        private RUID selected;
        /// <summary>
        /// 当前选中项
        /// </summary>
        public RUID SelectedItem
        {
            get { return this.selected; }
            set
            {
                if (!this.selected.Equals(value))
                {
                    this.selected = value;
                    DeepCopyUtility.SetValue<RUID>(this.OperationObject, value);//完全覆盖
                    Notify("SelectedItem");
                }
            }
        }

        //操作的数据（和用户交互同步）——注意：使用之前，需要设置持久化
        private RUID operationObject = new RUID();
        public RUID OperationObject
        {
            get { return this.operationObject; }
            set
            {
                if (!this.operationObject.Equals(value))
                {
                    this.operationObject = value;
                    Notify("OperationObject");
                }
            }
        }
        #endregion

        #region 命令方法——检索
        //数据列表的检索条件
        //检索条件
        public abstract dynamic SearchCondition { get; }

        private int GetTotalInfoCount()
        {
            try
            {
                string message = string.Empty;
                int count = Service.GetModelsCount(SearchCondition, out message);
                return count;
            }
            catch
            {
                return 0;
            }
        }
        protected virtual void Query()//可以重写查询
        {
            //设置页面状态
            this.PageState = EnDic_PageState.查询中;
            //开始异步查询
            var tk = Task<dynamic>.Run(() => this.QueryTK());
            //查询结束，回到主线程，恢复页面状态
            tk.GetAwaiter().OnCompleted(() =>
            {
                //此处替换不能在异步中进行
                //如果此处直接赋值，则会闪烁
                DeepCopyUtility.SetValue<RUID>(this.ItemSource, tk.Result.list);
                //处理提示
                if (tk.Result.info != string.Empty)//如果消息不是空的，需要处理返回结果
                {
                    this.ResultMessage = new CustomerMessageData(EnDic_MessageType.浮出, "查询提示", string.Format("查询过程出现问题：“{0}”", tk.Result), null);
                }
                //无论是否成功，都恢复页面状态
                this.PageState = EnDic_PageState.默认状态;
            });
        }
        dynamic QueryTK()
        {
            try
            {
                this.TotalInfoCount = this.GetTotalInfoCount();//此句
                string message = string.Empty;

                //加载模型集合，作为数据列表集合
                var temp = Service.GetModels(this.SearchCondition, out message);

                var result = new { info = message, list = temp };
                return result;
            }
            catch (Exception ex)
            {
                var result = new { info = ex.Message, list = new ObservableCollection<RUID>() };
                return result;
            }
        }

        #endregion

        #region 命令方法——增删改

        ModernDialog editdialog;

        private ObservableCollection<ControlDataItem> controlDataSource = new ObservableCollection<ControlDataItem>();
        public ObservableCollection<ControlDataItem> ControlDataSource
        {
            get { return this.controlDataSource; }
            set
            {
                if (this.controlDataSource != value)
                {
                    this.controlDataSource = value;
                    this.Notify("ControlDataSource");
                }
            }
        }//页面显示的内容
        private void ModelToControlDataSource(bool isnew)
        {
            try
            {
                this.ControlDataSource.Clear();//先清空

                var model = this.SelectedItem == null ? this.OperationObject : this.SelectedItem;//无论是否有值，只要有类型即可
                foreach (var item in model.GetType().GetProperties())
                {
                    var list = item.GetCustomAttributes(typeof(UIIsEditInput), true);

                    if (list.Count() > 0)//需要用户输入的属性，才需要生成
                    {
                        ControlDataItem cdi = new ControlDataItem();//一个绑定
                        //1、添加属性名称
                        cdi.PropertyName = item.Name;

                        //获取属性的UI显示名称
                        var l1 = item.GetCustomAttributes(typeof(UIName), true);
                        UIName p1 = l1.Count() > 0 ? l1[0] as UIName : null;
                        string uiName = p1 == null ? item.Name : p1.Name;//如果UI显示名称是空，则按照属性名称显示

                        //2、设置显示名
                        cdi.DisplayName = uiName;

                        //3、显示必填提示
                        var l1l2 = item.GetCustomAttributes(typeof(ValidNotNull), true);
                        ValidNotNull p1p2 = l1l2.Count() > 0 ? l1l2[0] as ValidNotNull : null;
                        Visibility v = p1p2 == null ? Visibility.Hidden : Visibility.Visible;//如果有必填要求，则显示必填提示

                        cdi.VisibleNotNull = v;

                        //3、设置类型
                        var l2 = item.GetCustomAttributes(typeof(UIEditControlType), true);
                        UIEditControlType p2 = l2.Count() > 0 ? l2[0] as UIEditControlType : null;
                        cdi.ControlType = p2 == null ? EnDic_EditControlType.文本框 : p2.ControlType;

                        //4、设置数据源
                        switch (cdi.ControlType)
                        {
                            case EnDic_EditControlType.下拉框:
                                break;
                            case EnDic_EditControlType.单选框:
                                break;
                            case EnDic_EditControlType.复选框:
                                break;
                            case EnDic_EditControlType.字典下拉框:
                                cdi.Source = Customer.Utility.ComboboxUtility.GetEnDicCombobox(item);//字典解析为集合
                                break;
                            case EnDic_EditControlType.文本框:
                                break;
                            case EnDic_EditControlType.时间控件:
                                break;
                            case EnDic_EditControlType.空:
                                break;
                            case EnDic_EditControlType.选取文件:
                                break;
                            case EnDic_EditControlType.选取路径:
                                break;
                            default:
                                break;
                        }
                        //5、根据是新增还是修改，来处理数据加载
                        if (!isnew)
                        {
                            //获取当前属性值
                            object proValue = item.GetValue(model);
                            if (proValue != null)
                            {
                                if ((cdi.ControlType == EnDic_EditControlType.下拉框 || cdi.ControlType == EnDic_EditControlType.字典下拉框))
                                {
                                    if (cdi.Source != null)
                                    {
                                        var itemlists = ((ObservableCollection<dynamic>)cdi.Source);
                                        cdi.CurrentValue = itemlists.FirstOrDefault(p => p.Text == proValue.ToString());
                                    }
                                }
                                else
                                {
                                    cdi.CurrentValue = proValue;
                                }
                            }
                        }
                        //6、处理默认选中的情况（新增时，需要处理）
                        if (cdi.Source != null && cdi.CurrentValue == null)
                        {
                            cdi.CurrentValue = ((ObservableCollection<dynamic>)cdi.Source).FirstOrDefault();
                        }

                        //添加到数据解析集合
                        this.ControlDataSource.Add(cdi);
                    }
                }
            }
            catch (Exception)
            {
                this.ControlDataSource = new ObservableCollection<ControlDataItem>();
            }
        }
        private void ModelFromControlDataSource()
        {
            foreach (var item in ControlDataSource)
            {
                var pro = this.OperationObject.GetType().GetProperty(item.PropertyName);

                //属性存在才操作
                if (pro != null)
                {
                    object objvalue = null;
                    if (item.CurrentValue == null)
                    {
                        objvalue = null;
                    }
                    else
                    {
                        switch (item.ControlType)
                        {
                            case EnDic_EditControlType.下拉框:
                                dynamic cbb = item.CurrentValue;
                                objvalue = cbb.Id;
                                break;
                            case EnDic_EditControlType.单选框:
                                break;
                            case EnDic_EditControlType.复选框:
                                break;
                            case EnDic_EditControlType.字典下拉框:
                                objvalue = Convert.ToInt32(((dynamic)item.CurrentValue).Id);
                                break;
                            case EnDic_EditControlType.文本框:
                                if (pro.PropertyType.FullName.Contains("String"))
                                {
                                    objvalue = item.CurrentValue.ToString();
                                }
                                else if (pro.PropertyType.FullName.Contains("Int"))
                                {
                                    int temp;
                                    if (int.TryParse(item.CurrentValue.ToString(), out temp))
                                    {
                                        objvalue = temp;
                                    }
                                }
                                else if (pro.PropertyType.FullName.Contains("double"))
                                {
                                    double temp;
                                    if (double.TryParse(item.CurrentValue.ToString(), out temp))
                                    {
                                        objvalue = temp;
                                    }
                                }
                                else if (pro.PropertyType.FullName.Contains("float"))
                                {
                                    float temp;
                                    if (float.TryParse(item.CurrentValue.ToString(), out temp))
                                    {
                                        objvalue = temp;
                                    }
                                }
                                break;
                            case EnDic_EditControlType.时间控件:
                                DateTime dt = (DateTime)item.CurrentValue;
                                objvalue = dt;
                                break;
                            case EnDic_EditControlType.空:
                                break;
                            case EnDic_EditControlType.选取文件:
                                break;
                            case EnDic_EditControlType.选取路径:
                                break;
                            default:
                                break;
                        }
                    }

                    pro.SetValue(this.OperationObject, objvalue);//设置属性值
                }
            }
        }
        //此命令只是跳转，并没有保存
        protected virtual void AddModel(object par)
        {
            //第一步：把Model解析成页面显示逻辑
            ModelToControlDataSource(true);//true表示新增

            //第二步：显示页面
            Pages.UserEdit edit = new Pages.UserEdit(this.ControlDataSource);
            editdialog = new ModernDialog() { Title = "新增用户", Content = edit };
            Button btn = new Button();
            btn.Content = "ok";
            btn.IsDefault = true;//默认是ok
            btn.Command = this.SaveCmd;

            editdialog.Buttons = new Button[] { btn, editdialog.CloseButton };
            editdialog.ShowDialog();
        }
        //此命令只是跳砖，并没有保存
        protected virtual void ModifyModel()
        {
            var model = this.SelectedItem;
            if (model == null)
            {
                ModernDialog.ShowMessage("请选择一位用户修改信息！", "修改提示", MessageBoxButton.OK);
                return;
            }

            //第一步：把Model解析成页面显示逻辑
            ModelToControlDataSource(false);//false表示修改

            //第二步：显示页面
            //显示设置菜单页面(仅处理菜单的勾选情况)
            Pages.UserEdit edit = new Pages.UserEdit(this.ControlDataSource);
            editdialog = new ModernDialog() { Title = "修改用户信息", Content = edit };
            Button btn = new Button();
            btn.Content = "ok";
            btn.IsDefault = true;//默认是ok
            btn.Command = this.SaveCmd;

            editdialog.Buttons = new Button[] { btn, editdialog.CloseButton };
            editdialog.ShowDialog();
        }
        //保存单体模型更改
        protected virtual void SaveModel()
        {
            ModelFromControlDataSource();//解析页面返回的内容，为数据模型赋值
            //处理操作用户和时间
            string res = Service.SaveModel(JsonUtility.SerializeObject(this.OperationObject));
            dynamic result = new { message = string.Empty, operaResult = false };
            JsonUtility.DeserializeAnonymousType<dynamic>(res, result);


            if (result.operaResult)
            {
                this.ResultMessage = new CustomerMessageData(EnDic_MessageType.浮出, "成功提示", "您已成功保存了数据！", null);
                editdialog.Close();
                this.Query();
            }
            else
            {
                Window win = Activator.LoginCache == null ? null : Activator.LoginCache.GetCurrentMainWindow();
                string message = result.message;
                this.ResultMessage = new CustomerMessageData(EnDic_MessageType.对话框, "错误提示", string.Format("提示：{0}", message), win);
            }
        }
        //批量删除模型
        protected virtual void DeleteModels()
        {
            var model = this.SelectedItem;
            if (model == null)
            {
                ModernDialog.ShowMessage("请选择需要删除的信息！", "修改提示", MessageBoxButton.OK);
                return;
            }

            List<string> keys = new List<string>();
            if (!string.IsNullOrEmpty(model.KeyIdentity))
            {
                keys.Add(model.KeyIdentity);
            }

            //处理操作用户和时间
            string res = Service.DeleteModels(JsonUtility.SerializeObject(keys));
            dynamic result = new { message = string.Empty, operaResult = false };
            JsonUtility.DeserializeAnonymousType<dynamic>(res, result);

            if (result.operaResult)
            {
                this.ResultMessage = new CustomerMessageData(EnDic_MessageType.浮出, "成功提示", "您已成功保存了数据！", null);
                editdialog.Close();
                this.Query();
            }
            else
            {
                Window win = Activator.LoginCache == null ? null : Activator.LoginCache.GetCurrentMainWindow();
                string message = result.message;
                this.ResultMessage = new CustomerMessageData(EnDic_MessageType.对话框, "错误提示", string.Format("提示：{0}", message), win);
            }
        }

        #endregion

        #region 实现INotifyPropertyChanged接口

        public event PropertyChangedEventHandler PropertyChanged;
        private void Notify(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion
    }
}
