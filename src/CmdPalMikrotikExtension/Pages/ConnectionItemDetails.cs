using System.Globalization;
using CmdPalMikrotikExtension.Model.ConnectionItem;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CmdPalMikrotikExtension.Pages
{
  public partial class ConnectionItemDetails : Details
  {
    public ConnectionItemDetails(ConnectionItem item, int defaultWinboxVersion)
    {
      Title = item.Name ?? "<noname>";
      Body = $"**Host:** {item.Host}\n\n" +
              $"**Username:** {item.Username}\n\n" +
              $"**Winbox version:** {(item.WinboxVersion.HasValue ? item.WinboxVersion.Value.ToString(CultureInfo.InvariantCulture) : $"Default [{defaultWinboxVersion}]")}";
    }
  }
}
