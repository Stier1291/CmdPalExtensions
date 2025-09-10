using System;
using CmdPalMikrotikExtension.Model.ConnectionItem;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CmdPalMikrotikExtension.Commands
{
  public partial class MoveItemCommand : InvokableCommand
  {
    private readonly ConnectionItem _item;
    private readonly Action<ConnectionItem> _moveItemDelegate;

    public MoveItemCommand(ConnectionItem item, Action<ConnectionItem> moveItemDelegate)
    {
      _item = item;
      _moveItemDelegate = moveItemDelegate;
    }

    public override ICommandResult Invoke()
    {
      _moveItemDelegate(_item);
      return CommandResult.KeepOpen();
    }
  }
}
