using System;
using System.Diagnostics;
using CmdPalRdpExtension.Helpers;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CmdPalRdpExtension.Commands;

public partial class ConnectRdpCommand(string arguments, bool admin = false) : InvokableCommand
{
  public override string Name => admin ? "Admin session" : (string.IsNullOrEmpty(arguments) ? "Run remote desktop" : $"Connect {arguments}");

  public override ICommandResult Invoke(object? sender)
  {
    if (string.IsNullOrEmpty(arguments))
    {
      Process.Start("mstsc.exe", admin ? "/admin" : string.Empty);
    }
    else
    {
      Process.Start("mstsc.exe", $"/v:{arguments}{(admin ? " /admin" : string.Empty)}");
      HistoryHelper.Instance.Add(arguments);
    }
    return CommandResult.Dismiss();
  }
}