using Gateway.Interfaces;
using Gateway.Common;
using System.IO.Compression;

namespace Gateway.Modules
{
    public class FileOperations 
    {
        ILogService _log;
        IJobService _job; 
        Extensions _extentions;
          
        public FileOperations(ILogService log, IJobService job) 
        {
            _log = log;
            _job = job;
            _extentions = new Extensions(); 
        }

        public void FileCopy(dynamic parameters)
        { 
            _log.LogInfo($"FileCopy(Parameters: {parameters})");
             
            var filename = (parameters.rename != null && !string.IsNullOrEmpty(parameters.rename.Value)) 
                            ? parameters.rename.Value : _job.FileInfo.Name;
            var output = (parameters.append_datetime.Value) 
                            ? _extentions.AppendTimeStamp(filename, parameters.datetime_format.Value) : filename;

            File.Copy(
                _job.FileInfo.FullName,
                Path.Combine(parameters.destination.Value, output),
                parameters.overwrite_file.Value);
        }
        public void FileMove(dynamic parameters)
        {
            _log.LogInfo($"FileMove(Parameters: {parameters})");

            var filename = (parameters.rename != null && !string.IsNullOrEmpty(parameters.rename.Value))
                            ? parameters.rename.Value : _job.FileInfo.Name;
            var output = (parameters.append_datetime.Value)
                            ? _extentions.AppendTimeStamp(filename, parameters.datetime_format.Value) : filename;

            File.Move(
                _job.FileInfo.FullName,
                Path.Combine(parameters.destination.Value, output),
                parameters.overwrite_file.Value);
        }

        public void Zip(dynamic parameters)
        {
            _log.LogInfo($"Zip(Parameters: {parameters})");

            foreach (var file_info in new DirectoryInfo(_job.Task.FilePath.Value).GetFiles(_job.FileParam.pattern.Value))
            {
                var destination = Path.Combine(parameters.destination.Value, _extentions.AppendTimeStamp(parameters.archive_name.Value, parameters.datetime_format.Value));
                using ZipArchive zip = ZipFile.Open(destination, ZipArchiveMode.Create);
                zip.CreateEntryFromFile((string)_job.FileInfo.Name, (string)_job.FileInfo.Name);
            }
        }
        public void Unzip(dynamic parameters)
        {
            _log.LogInfo($"Unzip(Parameters: {parameters})");
        }
        public void Validate(dynamic parameters)
        {
            _log.LogInfo($"Validate(Parameters: {parameters})");
        }
    }
}