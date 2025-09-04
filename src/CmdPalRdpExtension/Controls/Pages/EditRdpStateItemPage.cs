using System;
using CmdPalRdpExtension.Model;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CmdPalRdpExtension.Controls.Pages
{
  public partial class EditRdpStateItemPage : ContentPage
  {
    private readonly EditRdpStateItemForm _form;
    
    public EditRdpStateItemPage(RdpStateItem item, Action<RdpStateItem> saveDelegate, Action<RdpStateItem> deleteDelegate)
    {
      _form = new EditRdpStateItemForm(item, saveDelegate, deleteDelegate);

      Title = "Edit item";
      Name = "Edit";
    }

    public override IContent[] GetContent() => [_form];
  }
}
