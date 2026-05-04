using Newtonsoft.Json;
using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text; 

namespace Gateway
{
    public class Gateway
    {
        const string gateway_operations = "Gateway.Operations";
        private static readonly string _incoming_path = @"C:\Users\bokbok\source\repos\schred322\Gateway\Gateway\install.json";
        public static dynamic _job = new ExpandoObject();
        public static dynamic _instance = new ExpandoObject();
        public Gateway()
        {
            using StreamReader file_stream = File.OpenText(_incoming_path);
            _job = new JsonSerializer().Deserialize(file_stream, typeof(object));
        }
        //public static void Main(string[] args) { new Gateway(); new Gateway().Run(); }
        public void Run()
        {
            try
            {
                foreach (var file in _job.task.files)
                {
                    _instance.FilePattern = file.pattern.Value;
                    foreach (var file_info in new DirectoryInfo(_job.task.incoming_path.Value).GetFiles(file.pattern.Value))
                    {
                        _instance.FileInfo = file_info;
                        foreach (var route in _job.task.routes)
                        {
                            if (file.routes.Value.Contains(route.name.Value))
                            {
                                foreach (var op in route.operations)
                                {
                                    foreach (var c in Assembly.GetExecutingAssembly().GetTypes().Where(m => m.Name.Contains(typeof(Operations).Name)))
                                    {
                                        var instance = Type.GetType(gateway_operations).GetConstructor(Type.EmptyTypes).Invoke(new object[] { });
                                        c.GetMethod(op.method.Value, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                                            .Invoke(instance, new object[] { op.parameters[0] });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e) { Services.LogError(e); }
        }
    }
}
