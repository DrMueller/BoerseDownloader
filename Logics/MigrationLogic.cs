using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MMU.BoerseDownloader.Common.Infrastructure;
using MMU.BoerseDownloader.DataAccess;
using MMU.BoerseDownloader.Model;
using MMU.BoerseDownloader.SqlDataAccess;

namespace MMU.BoerseDownloader.Logics
{
    public class MigrationLogic
    {
        private readonly BoerseUserAccess _boerseUserAccess;
        private readonly Repository<BoerseUser> _boerseUserRepo;
        private readonly DownloadContextAccess _contextAccess;
        private readonly Repository<DownloadContext> _downloadContextRepo;
        private readonly DownloadEntryLogic _downloadEntryLogic;
        private readonly DownloadEntryConfigurationAccess _entryConfigurationAccess;
        private readonly Repository<DownloadEntryConfiguration> _entryConfigurationRepo;
        private IProgress<string> _progress;

        public MigrationLogic(BoerseUserAccess boerseUserAccess,
            DownloadContextAccess contextAccess,
            DownloadEntryConfigurationAccess entryConfigurationAccess,
            Repository<BoerseUser> boerseUserRepo,
            Repository<DownloadContext> downloadContextRepo,
            Repository<DownloadEntryConfiguration> entryConfigurationRepo,
            DownloadEntryLogic downloadEntryLogic)
        {
            _boerseUserAccess = boerseUserAccess;
            _contextAccess = contextAccess;
            _entryConfigurationAccess = entryConfigurationAccess;
            _boerseUserRepo = boerseUserRepo;
            _downloadContextRepo = downloadContextRepo;
            _entryConfigurationRepo = entryConfigurationRepo;
            _downloadEntryLogic = downloadEntryLogic;
        }

        public void Initialize(IProgress<string> progress)
        {
            _progress = progress;
            _downloadEntryLogic.Initialize(_progress);
        }

        private void NewProgressInformationCallback(string info)
        {
            _progress.Report(info);
        }

        public Task MigrateBoerseUserAsync(CancellationToken token)
        {
            return Task.Run(() =>
            {
                _progress.Report("Checking Boerse-User...");
                if (_boerseUserAccess.UserIsConfigured)
                {
                    _progress.Report("Cleaning Boerse-User...");
                    _boerseUserRepo.DeleteAll();
                    _progress.Report("Loading Boerse-User...");
                    var boerseUser = _boerseUserAccess.Load();
                    _progress.Report("Saving Boerse-User...");
                    _boerseUserRepo.Save(boerseUser);
                    _progress.Report("Boerse-User migrated!");
                }
                else
                {
                    _progress.Report("Boerse-User not found, nothing to migrate!");
                }
            }, token);
        }

        public Task MigrateContextsAsync(CancellationToken token)
        {
            return Task.Run(() =>
            {
                _progress.Report("Cleaning Entry-Contexts...");
                _downloadContextRepo.DeleteAll();
                _progress.Report("Loading Entry-Contexts...");
                var allDownloadContexts = _contextAccess.LoadAll().AsCancellable(token);
                CleanIds(allDownloadContexts);

                foreach (var context in allDownloadContexts)
                {
                    _progress.Report($"Saving {context.Name}...");
                    _downloadContextRepo.Save(context);
                }

                _progress.Report("Entry-Contexts migrated!");
            }, token);
        }

        public Task MigrateDownloadEntryConfigurationsAsync(CancellationToken token)
        {
            return Task.Run(() =>
            {
                _progress.Report("Cleaning Downloadentry-Configurations...");
                _entryConfigurationRepo.DeleteAll();
                _progress.Report("Loading Downloadentry-Configurations...");
                var allConfigurations = _entryConfigurationAccess.LoadAll().AsCancellable(token);

                foreach (var legacyConfig in allConfigurations)
                {
                    _progress.Report($"Mapping {legacyConfig.DownloadEntryTitle}...");
                    var config = new DownloadEntryConfiguration
                    {
                        IsLinkVisited = legacyConfig.DownloadLinkIsVisited,
                        Title = legacyConfig.DownloadEntryTitle
                    };

                    _progress.Report($"Saving {config.Title}...");
                    _entryConfigurationRepo.Save(config);
                }

                _progress.Report("Downloadentry-Configurations migrated!");
            }, token);
        }

        public Task MigrateDownloadEntryIdentifiers(CancellationToken token)
        {
            return Task.Run(async() =>
            {
                _progress.Report("Downloading all Entries...");
                var allEntries = await _downloadEntryLogic.GetallEntriesAsync();

                _progress.Report("Load all Entries from DB...");
                var dbEntries = _entryConfigurationRepo.LoadAll();

                foreach (var entry in allEntries)
                {
                    _progress.Report($"Searching {entry.DownloadEntryTitleShort} in DB...");
                    var dbEntry = dbEntries.FirstOrDefault(f => f.Title == entry.DownloadEntryTitleShort);
                    if (dbEntry != null)
                    {
                        _progress.Report($"Updating {dbEntry.Title} with Link-Identifier {entry.DownloadLinkIdentifier}...");
                        dbEntry.DownloadLinkIdentifier = entry.DownloadLinkIdentifier;
                        _entryConfigurationRepo.Save(dbEntry);
                    }
                    else
                    {
                        _progress.Report($"WARNING: Entry {entry.DownloadEntryTitleShort} not found in DB.");
                    }
                }
            }, token);
        }

        private void CleanIds(IEnumerable<Model.Interfaces.IIdentifiable> entities)
        {
            _progress.Report("Cleaning IDs...");

            foreach (var entity in entities)
            {
                entity.Id = 0;
            }
        }
    }
}