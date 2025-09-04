using CmdPalRdpExtension.Model;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace CmdPalRdpExtension.Helpers
{
  public class LocalStateHelper
  {
    private readonly RdpState _state;
    private readonly string _path;

    public LocalStateHelper()
    {
      var directory = Utilities.BaseSettingsPath("Microsoft.CmdPal");
      Directory.CreateDirectory(directory);
      _path = Path.Combine(directory, "rdp_v1.json");

      if (File.Exists(_path))
      {
        var jsonStringReading = File.ReadAllText(_path);
        if (!string.IsNullOrEmpty(jsonStringReading))
        {
          _state = JsonSerializer.Deserialize(jsonStringReading, RdpStateJsonContext.Default.RdpState) ?? new RdpState();
        }
        else
        {
          _state = new RdpState();
        }
      }
      else
      {
        _state = new RdpState();
      }
    }

    public IEnumerable<RdpStateItem> GetItems() => _state.Items;

    public void Update(RdpStateItem item)
    {
      if (item.Id == 0)
      {
        item.Id = _state.Items.Count == 0 ? 1 : _state.Items.Max(i => i.Id) + 1;
        _state.Items.Insert(0, item);
      }
      else
      {
        for (var i = 0; i < _state.Items.Count; i++)
        {
          if (item.Id == _state.Items[i].Id)
          {
            _state.Items[i] = item;
            break;
          }
        }
      }
      Persist();
    }

    public void Remove(RdpStateItem item)
    {
      _state.Items.Remove(item);
      Persist();
    }

    private void Persist()
    {
      var json = JsonSerializer.Serialize(_state, RdpStateJsonContext.Default.RdpState);
      File.WriteAllText(_path, json);
    }


  }
}
