// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using CmdPalRdpExtension.Commands;
using CmdPalRdpExtension.Helpers;
using CmdPalRdpExtension.Model;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CmdPalRdpExtension.Controls.Pages;

internal sealed partial class CmdPalRdpExtensionPage : DynamicListPage
{
  private readonly LocalStateHelper _localStateHelper;
  private IListItem[] _items;

  public CmdPalRdpExtensionPage(LocalStateHelper localStateHelper)
  {
    _localStateHelper = localStateHelper;
    Icon = CmdPalRdpExtensionCommandsProvider.RdpIcon;
    Title = "Remote Desktop";
    Name = "Open";

    RefreshList();
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
    var items = new List<IListItem>();
    if (!string.IsNullOrEmpty(SearchText) && !string.IsNullOrWhiteSpace(SearchText))
    {
      items.Add(new ListItem(new ConnectRdpCommand(SearchText) { Name = $"Connect {SearchText}", Invoked = OnSave }) { MoreCommands = [new CommandContextItem(new ConnectRdpCommand(SearchText, true) { Name = "Admin session" })] });
    }
    items.Add(new ListItem(new RunRdpCommand { Name = "Run remote desktop" }) { MoreCommands = [new CommandContextItem(new RunRdpCommand(true) { Name = "Admin session" })] });
    items.Add(new ListItem(new EditRdpStateItemPage(new RdpStateItem(), OnSave, OnDelete) { Name = "Add" }));
    items.AddRange(_localStateHelper.GetItems().Select(i =>
      new ListItem(new ConnectRdpCommand(i.Host) { Name = $"Connect {i.Name}" })
      {
        TextToSuggest = i.Host,
        MoreCommands =
        [
          new CommandContextItem(new ConnectRdpCommand(i.Host, true) { Name = "Admin session" }),
          new CommandContextItem(new EditRdpStateItemPage(i, OnSave, OnDelete))
        ]
      }));
    _items = items.ToArray();
    RaiseItemsChanged(_items.Length);
  }

  private void OnSave(RdpStateItem item)
  {
    _localStateHelper.Update(item);
    RefreshList();
  }

  private void OnDelete(RdpStateItem item)
  {
    _localStateHelper.Remove(item);
    RefreshList();
  }
}