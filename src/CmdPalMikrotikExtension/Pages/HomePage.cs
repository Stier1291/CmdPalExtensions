// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using CmdPalMikrotikExtension.Commands;
using CmdPalMikrotikExtension.Helpers;
using CmdPalMikrotikExtension.Model.ConnectionItem;
using CmdPalMikrotikExtension.Pages;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Collections.Generic;
using System.Linq;

namespace CmdPalMikrotikExtension;

internal sealed partial class HomePage : DynamicListPage
{
  private readonly LocalStateHelper _localStateHelper;
  private readonly ISettingsInterface _settings;

  private IListItem[] _items = [];

  public HomePage(LocalStateHelper localStateHelper, ISettingsInterface settings)
  {
    Icon = CmdPalMikrotikExtensionCommandsProvider.MikrotikIcon;
    Title = "Mikrotik";
    Name = "Open";
    ShowDetails = true;

    _localStateHelper = localStateHelper;
    _settings = settings;

    RefreshItems();
  }

  public override IListItem[] GetItems()
  {
    return _items;
  }

  public override void UpdateSearchText(string oldSearch, string newSearch)
  {
    if (oldSearch == newSearch) return;
    RefreshItems();
  }

  private void RefreshItems()
  {
    var items = new List<IListItem>();
    if (!SearchText.Equals("add", StringComparison.OrdinalIgnoreCase))
    {
      var newItem = ConnectionItem.Parse(SearchText);
      if (newItem != null)
      {
        items.Add(new ListItem(new ConnectCommand(newItem, GetExePath) { Invoked = OnInvoked }) { Title = newItem.ToString(false) });
      }
    }

    var conItems = _localStateHelper.GetItems().ToArray();
    items.AddRange(conItems
      .Where(item => item.Name?.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase) ?? false)
      .Select((item, index) =>
      new ListItem(new ConnectCommand(item, GetExePath))
      {
        Title = item.ToString(),
        MoreCommands = GetMoreCommands(item, index, conItems.Length).ToArray(),
        Details = new ConnectionItemDetails(item, _settings.WinboxVersion)
      }));
    items.Add(new ListItem(new ConnectionItemPage(new ConnectionItem(), OnSave, OnDelete)) { Title = "Add entry" });
    items.Add(new ListItem(new OpenCommand(GetExePath)) { Title = "Open Winbox" });

    _items = items.ToArray();
    RaiseItemsChanged(_items.Length);

    return;

    IEnumerable<IContextItem> GetMoreCommands(ConnectionItem item, int index, int count)
    {
      yield return new CommandContextItem(new ConnectionItemPage(item, OnSave, OnDelete));

      if (index > 0)
      {
        yield return new CommandContextItem(new MoveItemCommand(item, OnMoveUp) { Name = "Move up" });
      }
      if (index < count - 1)
      {
        yield return new CommandContextItem(new MoveItemCommand(item, OnMoveDown) { Name = "Move Down" });
      }
    }
  }

  private string GetExePath(int? version) => _localStateHelper.GetExePath(version ?? _settings.WinboxVersion);

  private void OnInvoked(ConnectionItem item) => _localStateHelper.UpdateItem(item);

  private void OnSave(ConnectionItem item)
  {
    _localStateHelper.UpdateItem(item);
    RefreshItems();
  }

  private void OnDelete(ConnectionItem item)
  {
    _localStateHelper.RemoveItem(item);
    RefreshItems();
  }

  private void OnMoveUp(ConnectionItem item)
  {
    _localStateHelper.MoveUpItem(item);
    RefreshItems();
  }

  private void OnMoveDown(ConnectionItem item)
  {
    _localStateHelper.MoveDownItem(item);
    RefreshItems();
  }
}
