using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.PowerModule.BridgeService
{
    public interface IRUIDService<RUID>
        where RUID : UIStrategy.BaseUI, new()
    {
        int GetModelsCount(dynamic searchCondition, out string message);
        ObservableCollection<RUID> GetModels(dynamic searchCondition, out string message);
        string SaveModel(string json);
        string DeleteModels(string json);
    }
}
