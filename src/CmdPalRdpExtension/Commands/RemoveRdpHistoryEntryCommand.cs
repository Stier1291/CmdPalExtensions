using System;
using CmdPalRdpExtension.Helpers;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CmdPalRdpExtension.Commands
{
  public class RemoveRdpHistoryEntryCommand(int id, Action refreshList): InvokableCommand
  {
    public override ICommandResult Invoke()
    {
      var historyChanged = HistoryHelper.Instance.Remove(id);
      if (historyChanged)
      {
        refreshList();
      }
      return CommandResult.KeepOpen();
    }
  }
}
