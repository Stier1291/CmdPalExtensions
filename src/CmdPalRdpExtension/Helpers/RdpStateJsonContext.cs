using System.Text.Json.Serialization;
using CmdPalRdpExtension.Model;

namespace CmdPalRdpExtension.Helpers;

[JsonSourceGenerationOptions]
[JsonSerializable(typeof(RdpState))]
[JsonSerializable(typeof(RdpStateItem))]
public partial class RdpStateJsonContext : JsonSerializerContext
{
}