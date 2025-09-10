using System;
using System.Security.Cryptography;
using System.Text;

namespace CmdPalMikrotikExtension.Model.ConnectionItem
{
  public class ConnectionItem
  {
    public static ConnectionItem? Parse(string str)
    {
      var args = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
      if (args.Length == 0) return null;
      var item = new ConnectionItem { Host = args[0] };
      switch (args.Length)
      {
        case 1:
          break;
        case 2:
          item.Username = args[1];
          break;
        case 3:
          item.Username = args[1];
          item.SetPassword(args[2]);
          break;
      }
      return item;
    }

    public Guid Guid { get; set; }
    public string? Name { get; set; }
    public string? Host { get; set; }
    public string? Username { get; set; }
    public byte[]? Password { get; set; }
    public int? WinboxVersion { get; set; }

    public bool TryGetPassword(out string password)
    {
      if (Password == null)
      {
        password = string.Empty;
        return false;
      }
      password = Encoding.UTF8.GetString(ProtectedData.Unprotect(Password, null, DataProtectionScope.CurrentUser));
      return true;
    }

    public string GetPassword()
    {
      if (Password == null) return string.Empty;
      return Encoding.UTF8.GetString(ProtectedData.Unprotect(Password, null, DataProtectionScope.CurrentUser));
    }

    public void SetPassword(string plain)
    {
      Password = string.IsNullOrEmpty(plain)
        ? null
        : ProtectedData.Protect(Encoding.UTF8.GetBytes(plain), null, DataProtectionScope.CurrentUser);
    }

    public override string ToString()
    {
      return ToString(true);
    }

    public string ToString(bool hideSensitive)
    {
      if (!string.IsNullOrEmpty(Name)) return Name;

      var str = Host ?? string.Empty;
      if (!string.IsNullOrEmpty(Username))
      {
        str += $" {Username}";
      }
      if (Password != null)
      {
        if (hideSensitive)
        {
          str += " *****";
        }
        else
        {
          str += $" {GetPassword()}";
        }
      }
      return str;
    }
  }
}
