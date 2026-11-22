using System.Globalization;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Gateway.Sandbox;

public sealed class JsonFormGenerator
{
    /* implementation
     * 
     * 
        var json = JObject.Parse(File.ReadAllText("data/installs/install-test.json"));

        var selectedPaths = new[]
        {
            "task.name",
            "task.description",
            "task.files[0]",
            "task.routes[0].operations[0].parameters[0]"
        };
        
        var generator = new JsonFormGenerator();
        var html = generator.Generate(json, selectedPaths, "/submit");
        
        File.WriteAllText("form.html", html);
       
     */
    
    public string Generate(JObject json, IEnumerable<string> selectedPaths, string formAction = "")
    {
        ArgumentNullException.ThrowIfNull(json);
        ArgumentNullException.ThrowIfNull(selectedPaths);

        var html = new StringBuilder();
        html.AppendLine("<!DOCTYPE html>");
        html.AppendLine("<html lang=\"en\">");
        html.AppendLine("<head>");
        html.AppendLine("<meta charset=\"utf-8\">");
        html.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
        html.AppendLine("<title>JSON Form</title>");
        html.AppendLine("</head>");
        html.AppendLine($"<body><form method=\"post\" action=\"{Encode(formAction)}\">");

        foreach (var path in selectedPaths.Distinct(StringComparer.Ordinal))
        {
            var token = json.SelectToken(path)
                ?? throw new ArgumentException($"JSON path '{path}' was not found.", nameof(selectedPaths));

            RenderToken(html, token, token.Path);
        }

        html.AppendLine("<button type=\"submit\">Submit</button>");
        html.AppendLine("</form></body></html>");
        return html.ToString();
    }

    private static void RenderToken(StringBuilder html, JToken token, string name)
    {
        switch (token)
        {
            case JObject obj:
                html.AppendLine($"<fieldset><legend>{Encode(GetDisplayName(name))}</legend>");
                foreach (var property in obj.Properties())
                    RenderToken(html, property.Value, property.Path);
                html.AppendLine("</fieldset>");
                break;

            case JArray array:
                html.AppendLine($"<fieldset><legend>{Encode(GetDisplayName(name))}</legend>");
                for (var index = 0; index < array.Count; index++)
                    RenderToken(html, array[index], $"{name}[{index}]");
                html.AppendLine("</fieldset>");
                break;

            case JValue value:
                RenderValue(html, value, name);
                break;
        }
    }

    private static void RenderValue(StringBuilder html, JValue value, string name)
    {
        var id = ToHtmlId(name);
        var label = Encode(GetDisplayName(name));
        var currentValue = value.Value;

        if (value.Type == JTokenType.Boolean)
        {
            var isChecked = currentValue is true ? " checked" : string.Empty;
            html.AppendLine($"<label><input id=\"{id}\" name=\"{Encode(name)}\" type=\"checkbox\" value=\"true\"{isChecked}> {label}</label>");
            return;
        }

        var inputType = value.Type is JTokenType.Integer or JTokenType.Float ? "number" : "text";
        var text = Convert.ToString(currentValue, CultureInfo.InvariantCulture) ?? string.Empty;
        html.AppendLine($"<label for=\"{id}\">{label}</label>");
        html.AppendLine($"<input id=\"{id}\" name=\"{Encode(name)}\" type=\"{inputType}\" value=\"{Encode(text)}\">");
    }

    private static string GetDisplayName(string path)
    {
        var name = path[(path.LastIndexOf('.') + 1)..];
        return name.Trim('[', ']');
    }

    private static string ToHtmlId(string value)
    {
        var builder = new StringBuilder(value.Length);
        foreach (var character in value)
            builder.Append(char.IsLetterOrDigit(character) ? character : '_');
        return builder.ToString();
    }

    private static string Encode(string value) => WebUtility.HtmlEncode(value);
}
