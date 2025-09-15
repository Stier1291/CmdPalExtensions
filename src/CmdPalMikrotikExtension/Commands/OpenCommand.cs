using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Diagnostics;

namespace CmdPalMikrotikExtension.Commands
{
  public partial class OpenCommand(Func<int?, string> getExePath) : InvokableCommand
  {
    public override ICommandResult Invoke()
    {
      Process.Start(getExePath(null));
      return CommandResult.Hide();
    }
  }
}
