// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.CommandPalette.Extensions;

namespace CmdPalRdpExtension;

[Guid("980f45f6-adc6-4c39-b1e0-0ec0756469b7")]
public sealed partial class CmdPalRdpExtension(ManualResetEvent extensionDisposedEvent) : IExtension, IDisposable
{
  private readonly CmdPalRdpExtensionCommandsProvider _provider = new();

    public object? GetProvider(ProviderType providerType)
    {
        return providerType switch
        {
            ProviderType.Commands => _provider,
            _ => null,
        };
    }

    public void Dispose() => extensionDisposedEvent.Set();
}
