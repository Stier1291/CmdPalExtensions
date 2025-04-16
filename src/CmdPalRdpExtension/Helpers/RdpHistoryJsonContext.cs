using System.Text.Json.Serialization;
using CmdPalRdpExtension.Model;

namespace CmdPalRdpExtension.Helpers;

[JsonSourceGenerationOptions]
[JsonSerializable(typeof(RdpHistory))]
[JsonSerializable(typeof(RdpHistoryItem))]
public partial class RdpHistoryJsonContext : JsonSerializerContext
{
}