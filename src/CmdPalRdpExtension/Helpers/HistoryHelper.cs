using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using CmdPalRdpExtension.Model;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CmdPalRdpExtension.Helpers
{
  public class HistoryHelper
  {
    public static HistoryHelper Instance { get; } = new();

    private readonly RdpHistory _history;
    private readonly string _path;

    public IEnumerable<RdpHistoryItem> Items => _history.Items;

    public HistoryHelper()
    {
      var directory = Utilities.BaseSettingsPath("Microsoft.CmdPal");
      Directory.CreateDirectory(directory);

      _path = Path.Combine(directory, "rdp_history_v1.json");

      if (File.Exists(_path))
      {
        var jsonStringReading = File.ReadAllText(_path);
        if (!string.IsNullOrEmpty(jsonStringReading))
        {
          _history = JsonSerializer.Deserialize(jsonStringReading, typeof(RdpHistoryItem), new RdpHistoryJsonContext()) as RdpHistory ?? new RdpHistory();
        }
        else
        {
          _history = new RdpHistory();
        }
      }
      else
      {
        _history = new RdpHistory();
      }
    }

    public void Add(string arguments)
    {
      if (_history.Items.Any(i => i.Arguments == arguments)) return;
      var nextId = _history.Items.Count == 0 ? 1 : _history.Items.Max(i => i.Id) + 1;
      _history.Items.Insert(0, new RdpHistoryItem(nextId, arguments));
      Persist();
    }

    public bool Remove(int id)
    {
      var item = _history.Items.FirstOrDefault(i => i.Id == id);
      if (item != null)
      {
        _history.Items.Remove(item);
        Persist();
        return true;
      }
      return false;
    }

    private void Persist()
    {
      var json = JsonSerializer.Serialize(_history, typeof(RdpHistory), new RdpHistoryJsonContext());
      File.WriteAllText(_path, json);
    }


  }
}
