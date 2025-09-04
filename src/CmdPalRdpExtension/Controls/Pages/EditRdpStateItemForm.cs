using CmdPalRdpExtension.Model;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Text.Json.Nodes;

namespace CmdPalRdpExtension.Controls.Pages
{
  public partial class EditRdpStateItemForm : FormContent
  {
    private readonly RdpStateItem _item;
    private readonly Action<RdpStateItem> _saveDelegate;
    private readonly Action<RdpStateItem> _deleteDelegate;

    public EditRdpStateItemForm(RdpStateItem item, Action<RdpStateItem> saveDelegate, Action<RdpStateItem> deleteDelegate)
    {
      _item = item;
      _saveDelegate = saveDelegate;
      _deleteDelegate = deleteDelegate;
      TemplateJson =
        $$"""
        {
            "type": "AdaptiveCard",
            "body": [
                {
                    "type": "TextBlock",
                    "size": "Medium",
                    "weight": "Bolder",
                    "text": "Edit item"
                },
                {
                    "type": "Input.Text",
                    "label": "Name:",
                    "id": "name",
                    "value" : "{{item.Name}}"
                },
                {
                    "type": "Input.Text",
                    "label": "Host:",
                    "id": "host",
                    "value": "{{item.Host}}"
                }
            ],
            "$schema": "http://adaptivecards.io/schemas/adaptive-card.json",
            "version": "1.6",
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
      var jsonData = JsonNode.Parse(inputs);
      if (action == null || jsonData == null) return CommandResult.GoBack();

      switch (action)
      {
        case "save":
          {
            var name = jsonData["name"]?.ToString();
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name)) return CommandResult.GoBack();
            var host = jsonData["host"]?.ToString();
            if (string.IsNullOrEmpty(host) || string.IsNullOrWhiteSpace(host)) return CommandResult.GoBack();

            _item.Name = name;
            _item.Host = host;
            _saveDelegate(_item);
            break;
          }
        case "delete":
          _deleteDelegate(_item);
          break;
      }

      return CommandResult.GoBack();
    }
  }
}
