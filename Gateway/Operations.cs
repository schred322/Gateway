using Gateway.interfaces;
using Gateway.Common;
using System;
using System.IO;
using System.IO.Compression;

namespace Gateway
{
    public class Operations 
    {
        ILogService _log;
        IJobServices _job; 
        Extensions _extentions;
          
        public Operations(ILogService log, IJobServices job) 
        {
            _log = log;
            _job = job;
            _extentions = new Extensions();

            //_job.Task = job.Task.task;
        }

        public void FileCopy(dynamic parameters)
        { 
            _log.LogInfo($"Parameters: {parameters}");

            var out_file = (parameters.append_datetime.Value)
                            ? _extentions.AppendTimeStamp(_job.FileInfo.Name, parameters.datetime_format.Value)
                            : _job.FileInfo.Name;
            File.Copy(
                _job.FileInfo.FullName,
                Path.Combine(parameters.destination.Value, out_file),
                parameters.overwrite_file.Value);
        }
        public void FileMove(dynamic parameters)
        {
            _log.LogInfo($"Parameters: {parameters}");

            var out_file = (parameters.append_datetime.Value)
                            ? _extentions.AppendTimeStamp(_job.Task.FileName.Value, parameters.datetime_format.Value)
                            : _job.Task.FileName.Value;
            File.Move(
                _job.Task.CurrentFilePathAndName.Value,
                Path.Combine(parameters.destination.Value, out_file),
                parameters.overwrite_file.Value);
        }
        public void CopyFrom(dynamic parameters)
        {
            _log.LogInfo($"Parameters: {parameters}");

            foreach (var file_info in new DirectoryInfo(parameters.source.Value).GetFiles(parameters.file_pattern.Value))
            {
                var out_file = (parameters.append_datetime.Value)
                                ? _extentions.AppendTimeStamp(_job.Task.FileName.Value, parameters.datetime_format.Value)
                                : file_info.Name;
                var file_path = Path.Combine(parameters.source.Value, file_info.Name);
                if (!File.Exists(file_path))
                {
                    File.Copy(file_path, Path.Combine(parameters.destination.Value, out_file), parameters.overwrite_file.Value);
                }
            }
        }
        public void MoveFrom(dynamic parameters)
        {
            _log.LogInfo($"Parameters: {parameters}");

            foreach (var file_info in new DirectoryInfo(parameters.source.Value).GetFiles(parameters.file_pattern.Value))
            {
                var out_file = (parameters.append_datetime.Value)
                                ? _extentions.AppendTimeStamp(_job.Task.FileName.Value, parameters.datetime_format.Value)
                                : file_info.Name;
                var file_path = Path.Combine(parameters.pickup_location.Value, file_info.Name);
                if (!File.Exists(file_path))
                {
                    File.Move(file_path, Path.Combine(parameters.destination.Value, out_file), parameters.overwrite_file.Value);
                }
            }
        }
        public void Zip(dynamic parameters)
        {
            _log.LogInfo($"Parameters: {parameters}");

            foreach (var file_info in new DirectoryInfo(_job.Task.FilePath.Value).GetFiles(parameters.file_pattern.Value))
            {
                var destination = Path.Combine(parameters.destination.Value, _extentions.AppendTimeStamp(parameters.archive_name.Value, parameters.datetime_format.Value));
                using ZipArchive zip = ZipFile.Open(destination, ZipArchiveMode.Create);
                zip.CreateEntryFromFile((string)_job.Task.CurrentFilePathAndName.Value, (string)_job.Task.CurrentFilePathAndName.Value);
            }
        }
        public void Unzip(dynamic parameters)
        {
            _log.LogInfo($"Parameters: {parameters}");
        }
        public void Validate(dynamic parameters)
        {
        }
    }
}