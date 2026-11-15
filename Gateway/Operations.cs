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
        }

        public void FileCopy(dynamic parameters)
        {
            var out_file = (parameters.append_datetime.Value)
                            ? _extentions.AppendTimeStamp(_job.Instance.FileName.Value, parameters.datetime_format.Value)
                            : _job.Instance.FileName.Value;
            File.Copy(
                _job.Instance.CurrentFilePathAndName.Value,
                Path.Combine(parameters.destination.Value, out_file),
                parameters.overwrite_file.Value);
        }
        public void FileMove(dynamic parameters)
        {
            var out_file = (parameters.append_datetime.Value)
                            ? _extentions.AppendTimeStamp(_job.Instance.FileName.Value, parameters.datetime_format.Value)
                            : _job.Instance.FileName.Value;
            File.Move(
                _job.Instance.CurrentFilePathAndName.Value,
                Path.Combine(parameters.destination.Value, out_file),
                parameters.overwrite_file.Value);
        }
        public void CopyFrom(dynamic parameters)
        {
            foreach (var file_info in new DirectoryInfo(parameters.source.Value).GetFiles(parameters.file_pattern.Value))
            {
                var out_file = (parameters.append_datetime.Value)
                                ? _extentions.AppendTimeStamp(_job.Instance.FileName.Value, parameters.datetime_format.Value)
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
            foreach (var file_info in new DirectoryInfo(parameters.source.Value).GetFiles(parameters.file_pattern.Value))
            {
                var out_file = (parameters.append_datetime.Value)
                                ? _extentions.AppendTimeStamp(_job.Instance.FileName.Value, parameters.datetime_format.Value)
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
            foreach (var file_info in new DirectoryInfo(_job.Instance.FilePath.Value).GetFiles(_job.Instance.FilePattern))
            {
                var destination = Path.Combine(parameters.destination.Value, _extentions.AppendTimeStamp(parameters.archive_name.Value, parameters.datetime_format.Value));
                using ZipArchive zip = ZipFile.Open(destination, ZipArchiveMode.Create);
                zip.CreateEntryFromFile((string)_job.Instance.CurrentFilePathAndName.Value, (string)_job.Instance.CurrentFilePathAndName.Value);
            }
        }
        public void Unzip(dynamic parameters)
        {
        }
        public void Validate(dynamic parameters)
        {
        }
    }
}