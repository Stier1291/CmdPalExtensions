using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Diagnostics;

namespace CmdPalRdpExtension.Commands
{
  public class RunRdpCommand(bool admin = false) : InvokableCommand
  {
    public override ICommandResult Invoke()
    {
      Process.Start("mstsc.exe", admin ? "/admin" : string.Empty);
      return CommandResult.Dismiss();
    }
  }
}
