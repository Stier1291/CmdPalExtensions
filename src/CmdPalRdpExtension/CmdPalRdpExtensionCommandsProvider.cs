// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CmdPalRdpExtension.Controls.Pages;
using CmdPalRdpExtension.Helpers;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CmdPalRdpExtension;

public partial class CmdPalRdpExtensionCommandsProvider : CommandProvider
{
  public static IconInfo RdpIcon = IconHelpers.FromRelativePaths("Assets\\RdpLightTheme.png", "Assets\\RdpDarkTheme.png");

  private readonly ICommandItem[] _commands;

  public CmdPalRdpExtensionCommandsProvider()
  {
    DisplayName = "Remote Desktop";
    Icon = RdpIcon;
    _commands = [
      new CommandItem(new CmdPalRdpExtensionPage(new LocalStateHelper())) { Title = DisplayName },
    ];
  }

  public override ICommandItem[] TopLevelCommands()
  {
    return _commands;
  }
}
