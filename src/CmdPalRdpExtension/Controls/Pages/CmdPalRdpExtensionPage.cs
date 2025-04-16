// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using CmdPalRdpExtension.Commands;
using CmdPalRdpExtension.Helpers;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CmdPalRdpExtension.Controls.Pages;

internal sealed partial class CmdPalRdpExtensionPage : DynamicListPage
{
  private IListItem[] _items;

  public CmdPalRdpExtensionPage()
  {
    Icon = CmdPalRdpExtensionCommandsProvider.RdpIcon;
    Title = "Remote Desktop";
    Name = "Open";

    _items = GetHistory().ToArray();
  }

  public override IListItem[] GetItems()
  {
    return _items;
  }

  public override void UpdateSearchText(string oldSearch, string newSearch)
  {
    if (newSearch == oldSearch) return;
    RefreshList();
  }

  private void RefreshList()
  {
    _items = new List<IListItem> { GetListItem(SearchText, null) }
      .Concat(GetHistory())
      .ToArray();
    RaiseItemsChanged(_items.Length);
  }

  private IEnumerable<IListItem> GetHistory()
  {
    var iconInfo = IconHelpers.FromRelativePaths("Assets\\HistoryLightTheme.png", "Assets\\HistoryDarkTheme.png");
    return HistoryHelper.Instance.Items.Select(i =>
      GetListItem(i.Arguments, iconInfo, new CommandContextItem(new RemoveRdpHistoryEntryCommand(i.Id, RefreshList) { Name = "Remove" })));
  }

  private static ListItem GetListItem(string arguments, IIconInfo? icon, params IContextItem[] items)
  {
    var moreCommands = new List<IContextItem>
      {
        new CommandContextItem(new ConnectRdpCommand(arguments, true) { Name = "Admin session" })
      }
      .Concat(items)
      .ToArray();
    return new ListItem(new ConnectRdpCommand(arguments))
    {
      Icon = icon,
      MoreCommands = moreCommands
    };
  }
}