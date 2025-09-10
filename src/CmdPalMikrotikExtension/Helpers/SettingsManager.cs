using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Globalization;
using System.IO;

namespace CmdPalMikrotikExtension.Helpers
{
  public class SettingsManager : JsonSettingsManager, ISettingsInterface
  {
    private readonly ChoiceSetSetting _winboxVersion = new(nameof(WinboxVersion), "version", "Default Winbox version", [
        new ChoiceSetSetting.Choice("Version 3", "3"),
        new ChoiceSetSetting.Choice("Version 4", "4")
      ]);

    public int WinboxVersion => string.IsNullOrEmpty(_winboxVersion.Value) ? 3 : int.Parse(_winboxVersion.Value, NumberStyles.Any, CultureInfo.InvariantCulture);

    public SettingsManager()
    {
      var directory = Utilities.BaseSettingsPath("Microsoft.CmdPal");
      Directory.CreateDirectory(directory);
      FilePath = Path.Combine(directory, "settings.json");

      Settings.Add(_winboxVersion);

      LoadSettings();

      Settings.SettingsChanged += (_, _) => SaveSettings();
    }
  }
}
