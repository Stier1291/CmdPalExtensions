using System;
using System.Diagnostics;
using CmdPalMikrotikExtension.Model.ConnectionItem;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CmdPalMikrotikExtension.Commands
{
  public partial class ConnectCommand : InvokableCommand
  {
    private readonly ConnectionItem _item;
    private readonly Func<int?, string> _getExePath;

    public ConnectCommand(ConnectionItem item, Func<int?, string> getExePath)
    {
      Name = $"Connect {item.Host}";

      _item = item;
      _getExePath = getExePath;
    }

    public Action<ConnectionItem>? Invoked { get; set; }

    public override ICommandResult Invoke()
    {
      var args = _item.Host;
      if (!string.IsNullOrEmpty(_item.Username))
      {
        args += $" {_item.Username}";
        if (_item.TryGetPassword(out var password))
        {
          args += $" {password}";
        }
      }
      Process.Start(_getExePath(_item.WinboxVersion), args);
      Invoked?.Invoke(_item);
      return CommandResult.GoHome();
    }
  }
}
