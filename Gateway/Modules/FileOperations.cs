using Gateway.Interfaces;
using Gateway.Common;
using System.IO.Compression;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using System.Data;

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
        public void CreateFileFromDb(dynamic parameters)
        {
            //todo: add data from dbOperation.DataList to file if parameters.data_source == "db" 
            try
            {
                _log.LogInfo($"CreateFileFromDb(Parameters: {parameters})");
                _log.LogInfo($"{_job.DbDataList.Count} records retrieved from source database.");

                var file_name = $"{((string)parameters.file_pattern.Value)
                                    .Replace("*", DateTime.Now.ToString($"{(string)parameters.datetime_format.Value}"))}";
                var destination = string.Concat(parameters.destination.Value, file_name);
                
                File.WriteAllText(destination, JsonSerializer.Serialize(_job.DbDataList));
            }
            catch(Exception e)
            {

                _log.LogError(e);
                throw;
            }
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

            foreach (var file_info in new DirectoryInfo(_job.Task.FilePath.Value).GetFiles(_job.FileParams.pattern.Value))
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
        public void FileRouting(dynamic operation)
        {
            foreach (var file in operation.files)
            {
                _job.FileParams = file;

                var directory = new DirectoryInfo(file.source.Value);

                foreach (var fileInfo in directory.GetFiles(file.pattern.Value))
                {
                    _job.FileInfo = fileInfo;
                    _log.LogInfo($"FileInfo: {fileInfo}");

                    ProcessFileRoutes(operation, file);
                }
            }
        }
        private void ProcessFileRoutes(dynamic operation, dynamic file)
        {
            foreach (dynamic route in operation.routes)
            {
                if (!file.routes.Value.Contains(route.name.Value))
                    continue;

                _log.LogInfo($"Route Match: {route.name.Value}");

                foreach (var op in route.operations)
                    ExecuteFileOperation(op);
            }
        }
        private void ExecuteFileOperation(dynamic operation)
        {
            _log.LogInfo($"Operation: {operation.method.Value}");

            var method = typeof(FileOperations).GetMethod(operation.method.Value);
            if (method == null)
                throw new InvalidOperationException($"FileOperations '{operation.method.Value}' was not found.");

            var operations = new FileOperations(_log, _job);
            method.Invoke(operations, new object[] { Utilities.ObjectNullCheck(operation.parameters) });
        }
    }
}