using System.Text.Json.Serialization;

namespace CmdPalMikrotikExtension.Model.Settings
{
  [JsonSourceGenerationOptions]
  [JsonSerializable(typeof(Settings))]
  public partial class SettingsJsonContext : JsonSerializerContext
  {
  }
}
