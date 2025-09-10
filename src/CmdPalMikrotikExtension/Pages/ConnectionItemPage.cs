using System;
using System.Text.Json.Nodes;
using CmdPalMikrotikExtension.Model.ConnectionItem;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CmdPalMikrotikExtension.Pages
{
  public partial class ConnectionItemPage : ContentPage
  {
    private readonly ConnectionItemForm _form;

    public ConnectionItemPage(ConnectionItem item, Action<ConnectionItem> saveDelegate, Action<ConnectionItem> deleteDelegate)
    {
      _form = new ConnectionItemForm(item, saveDelegate, deleteDelegate);

      Title = "Edit item";
      Name = "Edit";
    }

    public override IContent[] GetContent()
    {
      return [_form];
    }

    private partial class ConnectionItemForm : FormContent
    {
      private readonly ConnectionItem _item;
      private readonly Action<ConnectionItem> _saveDelegate;
      private readonly Action<ConnectionItem> _deleteDelegate;

      public ConnectionItemForm(ConnectionItem item, Action<ConnectionItem> saveDelegate, Action<ConnectionItem> deleteDelegate)
      {
        _item = item;
        _saveDelegate = saveDelegate;
        _deleteDelegate = deleteDelegate;

        var winboxVersion = item.WinboxVersion.HasValue ? $"{item.WinboxVersion.Value}" : "settings";
        TemplateJson = $$"""
                       {
                           "type": "AdaptiveCard",
                           "$schema": "http://adaptivecards.io/schemas/adaptive-card.json",
                           "version": "1.6",
                           "body": [
                               {
                                   "type": "TextBlock",
                                   "size": "Medium",
                                   "weight": "Bolder",
                                   "text": "Edit item"
                               },
                               {
                                   "type": "Input.Text",
                                   "label": "Name",
                                   "value": "{{item.Name}}",
                                   "id": "name"
                               },
                               {
                                   "type": "Input.Text",
                                   "label": "Host",
                                   "value": "{{item.Host}}",
                                   "id": "host"
                               },
                               {
                                   "type": "Input.Text",
                                   "label": "Username",
                                   "value": "{{item.Username}}",
                                   "id": "username"
                               },
                               {
                                   "type": "Input.Text",
                                   "label": "Password",
                                   "value": "{{item.GetPassword()}}",
                                   "id": "password",
                                   "style": "Password"
                               },
                               {
                                   "type": "Input.ChoiceSet",
                                   "choices": [
                                       {
                                           "title": "Settings",
                                           "value": "settings"
                                       },
                                       {
                                           "title": "Version 3",
                                           "value": "3"
                                       },
                                       {
                                           "title": "Version 4",
                                           "value": "4"
                                       }
                                   ],
                                   "label": "Winbox Version",
                                   "value": "{{winboxVersion}}",
                                   "id": "version"
                               }
                           ],
                           "actions": [
                               {
                                   "type": "Action.Submit",
                                   "title": "Save",
                                   "style": "positive",
                                   "data": "save"
                               },
                               {
                                   "type": "Action.Submit",
                                   "title": "Delete",
                                   "style": "destructive",
                                   "data": "delete"
                               }
                           ]
                       }
                       """;
      }

      public override ICommandResult SubmitForm(string inputs, string data)
      {
        var action = JsonNode.Parse(data)?.ToString();
        switch (action)
        {
          case "save":
            var json = JsonNode.Parse(inputs);
            if (json == null) return CommandResult.GoBack();

            _item.Name = json["name"]?.ToString() ?? string.Empty;
            _item.Host = json["host"]?.ToString() ?? string.Empty;
            _item.Username = json["username"]?.ToString() ?? string.Empty;
            _item.SetPassword(json["password"]?.ToString() ?? string.Empty);

            if (int.TryParse(json["version"]?.ToString(), out var winboxVersion))
            {
              _item.WinboxVersion = winboxVersion;
            }
            _saveDelegate(_item);
            break;
          case "delete":
            _deleteDelegate(_item);
            break;
        }
        return CommandResult.GoBack();
      }
    }
  }
}
