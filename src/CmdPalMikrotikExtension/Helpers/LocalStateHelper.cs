using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using CmdPalMikrotikExtension.Model.ConnectionItem;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CmdPalMikrotikExtension.Helpers
{
  public class LocalStateHelper
  {
    private readonly string _connectionInfoPath;

    private readonly Dictionary<int, string> _exeInfos;

    private readonly List<ConnectionItem> _items;

    public LocalStateHelper()
    {
      var directory = Utilities.BaseSettingsPath("Microsoft.CmdPal");
      Directory.CreateDirectory(directory);

      _exeInfos = new Dictionary<int, string>
      {
        { 3, Path.Combine(directory, "Winbox", "3", "winbox64.exe") },
        { 4, Path.Combine(directory, "Winbox", "4", "WinBox.exe") }
      };

      var winboxPath = Path.Combine(directory, "Winbox");
      if (!Directory.Exists(winboxPath))
      {
        Directory.CreateDirectory(winboxPath);
        using var stream = typeof(LocalStateHelper).Assembly.GetManifestResourceStream("CmdPalMikrotikExtension.Assets.Winbox.Winbox.zip")!;
        var zip = new ZipArchive(stream);
        zip.ExtractToDirectory(directory);
      }

      _connectionInfoPath = Path.Combine(directory, "connections_v1.json");
      if (File.Exists(_connectionInfoPath))
      {
        var json = File.ReadAllText(_connectionInfoPath);
        _items = JsonSerializer.Deserialize<List<ConnectionItem>>(json, ConnectionItemJsonContext.Default.ListConnectionItem)!;
      }
      else
      {
        _items = [];
      }
    }

    public string GetExePath(int version)
    {
      return _exeInfos[version];
    }

    public IEnumerable<ConnectionItem> GetItems() => _items;

    public void UpdateItem(ConnectionItem item)
    {
      if (item.IsNew)
      {
        item.Guid = Guid.NewGuid();
        _items.Add(item);
      }
      else
      {
        for (var i = 0; i < _items.Count; i++)
        {
          if (item.Guid == _items[i].Guid)
          {
            _items[i] = item;
            break;
          }
        }
      }
      PersistItems();
    }

    public void RemoveItem(ConnectionItem item)
    {
      _items.Remove(item);
      PersistItems();
    }

    public void MoveUpItem(ConnectionItem item)
    {
      var index = _items.IndexOf(item);
      if(index <= 0) return;

      var preItem = _items[index - 1];
      _items[index - 1] = item;
      _items[index] = preItem;
      PersistItems();
    }

    public void MoveDownItem(ConnectionItem item)
    {
      var index = _items.IndexOf(item);
      if (index == _items.Count - 1) return;

      var nextItem = _items[index + 1];
      _items[index + 1] = item;
      _items[index] = nextItem;
      PersistItems();
    }

    private void PersistItems()
    {
      var json = JsonSerializer.Serialize(_items, ConnectionItemJsonContext.Default.ListConnectionItem);
      File.WriteAllText(_connectionInfoPath, json);
    }
  }
}
