using System;
using System.Diagnostics;
using CmdPalMikrotikExtension.Model.ConnectionItem;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CmdPalMikrotikExtension.Commands
{
  public partial class ConnectCommand(ConnectionItem item, Func<int?, string> getExePath) : InvokableCommand
  {
    public Action<ConnectionItem>? Invoked { get; set; }

    public override ICommandResult Invoke()
    {
      var args = item.Host;
      if (!string.IsNullOrEmpty(item.Username))
      {
        args += $" {item.Username}";
        if (item.TryGetPassword(out var password))
        {
          args += $" {password}";
        }
      }
      Process.Start(getExePath(item.WinboxVersion), args);
      Invoked?.Invoke(item);
      return CommandResult.GoHome();
    }
  }
}
