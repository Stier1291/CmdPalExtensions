using System;
using System.Text.Json.Nodes;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Settings = CmdPalMikrotikExtension.Model.Settings.Settings;

namespace CmdPalMikrotikExtension.Pages
{
  internal partial class SettingsPage(Settings settings, Action saveDelegate) : ContentPage
  {
    private readonly SettingsForm _form = new(settings, saveDelegate);

    public override IContent[] GetContent()
    {
      return [_form];
    }

    private partial class SettingsForm : FormContent
    {
      private readonly Settings _settings;
      private readonly Action _saveDelegate;

      public SettingsForm(Settings settings, Action saveDelegate)
      {
        _settings = settings;
        _saveDelegate = saveDelegate;
        TemplateJson = $$"""
                       {
                           "type": "AdaptiveCard",
                           "$schema": "http://adaptivecards.io/schemas/adaptive-card.json",
                           "version": "1.6",
                           "body": [
                               {
                                   "type": "TextBlock",
                                   "text": "Settings",
                                   "wrap": true,
                                   "style": "heading"
                               },
                               {
                                   "type": "Input.ChoiceSet",
                                   "choices": [
                                       {
                                           "title": "3",
                                           "value": "3"
                                       },
                                       {
                                           "title": "4",
                                           "value": "4"
                                       }
                                   ],
                                   "label": "Default Winbox Version",
                                   "style": "expanded",
                                   "id": "version",
                                   "isRequired": true,
                                   "errorMessage": "No version selected!",
                                   "value": "{{settings.WinboxVersion}}"
                               }
                           ],
                           "actions": [
                               {
                                   "type": "Action.Submit",
                                   "title": "Save",
                                   "id": "save",
                                   "style": "positive"
                               }
                           ]
                       }
                       """;
      }

      public override ICommandResult SubmitForm(string inputs, string data)
      {
        var result = JsonNode.Parse(inputs);
        if (result == null) return CommandResult.GoBack();
        var versionString = result["version"]?.ToString();
        if (string.IsNullOrEmpty(versionString)) return CommandResult.GoBack();

        if (int.TryParse(versionString, out var version))
        {
          _settings.WinboxVersion = version;
          _saveDelegate();
        }
        return CommandResult.GoBack();
      }
    }
  }
}
