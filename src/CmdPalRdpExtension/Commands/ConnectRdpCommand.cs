using System;
using System.Diagnostics;
using CmdPalRdpExtension.Model;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CmdPalRdpExtension.Commands;

public partial class ConnectRdpCommand(string host, bool admin = false) : InvokableCommand
{
  public Action<RdpStateItem> Invoked { get; set; }

  public override ICommandResult Invoke(object? sender)
  {
    Process.Start("mstsc.exe", $"/v:{host}{(admin ? " /admin" : string.Empty)}");
    Invoked?.Invoke(new RdpStateItem{Name = host, Host = host});
    return CommandResult.Dismiss();
  }
}