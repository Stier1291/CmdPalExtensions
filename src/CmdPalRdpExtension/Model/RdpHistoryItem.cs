namespace CmdPalRdpExtension.Model
{
  public class RdpHistoryItem(int id, string arguments)
  {
    public int Id { get; init; } = id;
    public string Arguments { get; init; } = arguments;
  }
}
