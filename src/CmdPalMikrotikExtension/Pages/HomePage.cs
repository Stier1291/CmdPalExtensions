// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using CmdPalMikrotikExtension.Commands;
using CmdPalMikrotikExtension.Helpers;
using CmdPalMikrotikExtension.Model.ConnectionItem;
using CmdPalMikrotikExtension.Pages;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CmdPalMikrotikExtension;

internal sealed partial class HomePage : DynamicListPage
{
  private readonly LocalStateHelper _localStateHelper;
  private readonly Model.Settings.Settings _settings;

  private IListItem[] _items;

  public HomePage(LocalStateHelper localStateHelper)
  {
    Icon = CmdPalMikrotikExtensionCommandsProvider.MikrotikIcon;
    Title = "Mikrotik";
    Name = "Open";

    _localStateHelper = localStateHelper;
    _settings = _localStateHelper.LoadSettings();

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
    var newItem = ConnectionItem.Parse(SearchText);
    if (newItem != null)
    {
      items.Add(new ListItem(new ConnectCommand(newItem, GetExePath) { Name = newItem.ToString(false), Invoked = OnInvoked }));
    }
    items.AddRange(_localStateHelper.GetItems().Select(item =>
      new ListItem(new ConnectCommand(item, GetExePath) { Name = item.ToString() })
      {
        TextToSuggest = item.ToString(),
        MoreCommands = [
          new CommandContextItem(new ConnectionItemPage(item, OnSave, OnDelete))
          ]
      }));
    items.Add(new ListItem(new ConnectionItemPage(new ConnectionItem(), OnSave, OnDelete)) { Title = "Add entry" });
    items.Add(new ListItem(new SettingsPage(_settings, () => _localStateHelper.UpdateSettings(_settings))) { Title = "Settings" });

    _items = items.ToArray();
    RaiseItemsChanged(_items.Length);
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
}
