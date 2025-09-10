// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CmdPalMikrotikExtension.Helpers;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CmdPalMikrotikExtension;

public partial class CmdPalMikrotikExtensionCommandsProvider : CommandProvider
{
  public static IconInfo MikrotikIcon = IconHelpers.FromRelativePath("Assets\\StoreLogo.scale-100.png");

  private readonly SettingsManager _settingsManager;
  private readonly ICommandItem[] _commands;

  public CmdPalMikrotikExtensionCommandsProvider()
  {
    DisplayName = "Mikrotik";
    Icon = MikrotikIcon;

    _settingsManager = new SettingsManager();
    Settings = _settingsManager.Settings;

    _commands = [
        new CommandItem(new HomePage(new LocalStateHelper(), _settingsManager))
        {
          Title = DisplayName,
          MoreCommands = [new CommandContextItem(_settingsManager.Settings.SettingsPage)]
        }
        ];
  }

  public override ICommandItem[] TopLevelCommands()
  {
    return _commands;
  }

}
