using System;
using System.IO;
using System.IO.Compression;

namespace Gateway
{
    public class Operations : Gateway
    {
        public Operations() { }
        public void FileCopy(dynamic parameters)
        {
            var out_file = (parameters.append_datetime.Value)
                            ? Services.AppendTimeStamp(_instance.FileInfo.Name, parameters.datetime_format.Value)
                            : _instance.FileInfo.Name;
            File.Copy(
                Path.Combine(_job.task.incoming_path.Value, _instance.FileInfo.Name),
                Path.Combine(parameters.destination.Value, out_file),
                parameters.overwrite_file.Value);
        }
        public void FileMove(dynamic parameters)
        {
            var out_file = (parameters.append_datetime.Value)
                            ? Services.AppendTimeStamp(_instance.FileInfo.Name, parameters.datetime_format.Value)
                            : _instance.FileInfo.Name;
            File.Move(
                Path.Combine(_job.task.incoming_path.Value, _instance.FileInfo.Name),
                Path.Combine(parameters.destination.Value, out_file),
                parameters.overwrite_file.Value);
        }
        public static void CopyFrom(dynamic parameters)
        {
            foreach (var file_info in new DirectoryInfo(parameters.source.Value).GetFiles(parameters.file_pattern.Value))
            {
                var out_file = (parameters.append_datetime.Value)
                                ? Services.AppendTimeStamp(_instance.FileInfo.Name, parameters.datetime_format.Value)
                                : file_info.Name;
                var file_path = Path.Combine(parameters.source.Value, file_info.Name);
                if (!File.Exists(file_path))
                {
                    File.Copy(file_path, Path.Combine(parameters.destination.Value, out_file), parameters.overwrite_file.Value);
                }
            }
        }
        public static void MoveFrom(dynamic parameters)
        {
            foreach (var file_info in new DirectoryInfo(parameters.source.Value).GetFiles(parameters.file_pattern.Value))
            {
                var out_file = (parameters.append_datetime.Value)
                                ? Services.AppendTimeStamp(_instance.FileInfo.Name, parameters.datetime_format.Value)
                                : file_info.Name;
                var file_path = Path.Combine(parameters.pickup_location.Value, file_info.Name);
                if (!File.Exists(file_path))
                {
                    File.Move(file_path, Path.Combine(parameters.destination.Value, out_file), parameters.overwrite_file.Value);
                }
            }
        }
        public static void Zip(dynamic parameters)
        {
            foreach (var file_info in new DirectoryInfo(_job.task.incoming_path.Value).GetFiles(_instance.FilePattern))
            {
                var destination = Path.Combine(parameters.destination.Value, Services.AppendTimeStamp(parameters.archive_name.Value, parameters.datetime_format.Value));
                using ZipArchive zip = ZipFile.Open(destination, ZipArchiveMode.Create);
                zip.CreateEntryFromFile((string)_instance.FileInfo.FullName, (string)_instance.FileInfo.FullName);
            }
        }
        public static void Unzip(dynamic parameters)
        {
        }
        public static void Validate(dynamic parameters)
        {
        }
    }
}