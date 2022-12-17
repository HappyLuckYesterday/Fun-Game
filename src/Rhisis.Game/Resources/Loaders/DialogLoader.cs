﻿using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Rhisis.Core.Extensions;
using Rhisis.Abstractions.Resources;
using Rhisis.Game.Common.Resources.Dialogs;
using System.Collections.Concurrent;
using System.IO;

namespace Rhisis.Game.Resources.Loaders;

public sealed class DialogLoader : IGameResourceLoader
{
    private readonly ILogger<DialogLoader> _logger;
    private readonly IMemoryCache _cache;
    private readonly JsonLoadSettings _jsonSettings = new JsonLoadSettings { CommentHandling = CommentHandling.Ignore };

    /// <summary>
    /// Creates a new <see cref="DialogLoader"/> instance.
    /// </summary>
    /// <param name="logger">Logger</param>
    /// <param name="cache">Memory cache.</param>
    public DialogLoader(ILogger<DialogLoader> logger, IMemoryCache cache)
    {
        _logger = logger;
        _cache = cache;
    }

    /// <inheritdoc />
    public void Load()
    {
        var dialogsDirectories = Directory.GetDirectories(GameResourcesConstants.Paths.DialogsPath);

        var dialogSets = new ConcurrentDictionary<string, DialogSet>();

        foreach (var dialogPath in dialogsDirectories)
        {
            var lang = new DirectoryInfo(dialogPath).Name;
            var dialogFiles = Directory.GetFiles(dialogPath);

            var dialogSet = new DialogSet(lang);

            foreach (var dialogFile in dialogFiles)
            {
                var dialogFileContent = File.ReadAllText(dialogFile);
                var dialogsParsed = JToken.Parse(dialogFileContent, _jsonSettings);

                if (dialogsParsed.Type == JTokenType.Array)
                {
                    var dialogs = dialogsParsed.ToObject<DialogData[]>();

                    foreach (DialogData dialog in dialogs)
                        AddDialog(dialogSet, dialog);
                }
                else
                {
                    AddDialog(dialogSet, dialogsParsed.ToObject<DialogData>());
                }
            }

            dialogSets.TryAdd(lang, dialogSet);
            _logger.LogInformation($"-> {dialogSet.Count} dialogs loaded for language {lang}.");
        }

        _cache.Set(GameResourcesConstants.Dialogs, dialogSets);
    }

    /// <summary>
    /// Adds a dialog to a given <see cref="DialogSet"/>.
    /// </summary>
    /// <param name="dialogSet">Dialog set</param>
    /// <param name="dialog">Dialog to add</param>
    private void AddDialog(DialogSet dialogSet, DialogData dialog)
    {
        if (dialogSet.ContainsKey(dialog.Name))
        {
            _logger.LogDebug(GameResourcesConstants.Errors.ObjectIgnoredMessage, "Dialog", dialog.Name, "already declared");
            return;
        }

        if (dialog.Links.HasDuplicates(x => x.Id))
        {
            _logger.LogError(GameResourcesConstants.Errors.ObjectErrorMessage, "Dialog", dialog.Name, "duplicate dialog link keys.");
            return;
        }

        dialogSet.Add(dialog.Name, dialog);
    }
}
