using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CmdPalMikrotikExtension.Model.ConnectionItem
{
  [JsonSourceGenerationOptions]
  [JsonSerializable(typeof(List<ConnectionItem>))]
  public partial class ConnectionItemJsonContext : JsonSerializerContext
  {
  }
}
