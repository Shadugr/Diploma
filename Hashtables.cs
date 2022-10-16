using Newtonsoft.Json;

namespace FMI_web
{
    public class MainPageClass
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Content { get; set; }

        public MainPageClass(string? name, string? type, string? content)
        {
            Name = name;
            Type = type;
            Content = content;
        }
        public MainPageClass(string? name, string? type)
        {
            Name = name;
            Type = type;
        }
        public MainPageClass() {}
    }
    public static class Hashtables
    {
        public static Dictionary<string, MainPageClass> MainPages { get; set; }
            = FileToHashtable(Defs.FILE_HASHTABLESDIRECTORY + '/' + Defs.FILE_MAINHASHTABLE);

        public static void HashtableToFile(Dictionary<string, MainPageClass> table, string path)
        {
            string json = JsonConvert.SerializeObject(table);
            File.WriteAllText(path, json);
        }
        public static Dictionary<string, MainPageClass> FileToHashtable(string path)
        {
            if (!File.Exists(path))
                File.Create(path);
            var file = new FileInfo(path);
            if (file.Length == 0)
                return new Dictionary<string, MainPageClass>();
            return JsonConvert.DeserializeObject<Dictionary<string, MainPageClass>>
                (File.ReadAllText(path)) ?? new Dictionary<string, MainPageClass>();
        }

        public static string ConvertToLatin(string source)
        {
            string result = source.ToLower();
            result = result.Replace("а", "a");
            result = result.Replace("б", "b");
            result = result.Replace("в", "v");
            result = result.Replace("г", "h");
            result = result.Replace("ґ", "g");
            result = result.Replace("д", "d");
            result = result.Replace("е", "e");
            result = result.Replace("є", "ye");
            result = result.Replace("ж", "zh");
            result = result.Replace("з", "z");
            result = result.Replace("и", "y");
            result = result.Replace("і", "i");
            result = result.Replace("ї", "yi");
            result = result.Replace("й", "j");
            result = result.Replace("к", "k");
            result = result.Replace("л", "l");
            result = result.Replace("м", "m");
            result = result.Replace("н", "n");
            result = result.Replace("о", "o");
            result = result.Replace("п", "p");
            result = result.Replace("р", "r");
            result = result.Replace("с", "s");
            result = result.Replace("т", "t");
            result = result.Replace("у", "u");
            result = result.Replace("ф", "f");
            result = result.Replace("х", "kh");
            result = result.Replace("ц", "c");
            result = result.Replace("ч", "ch");
            result = result.Replace("ш", "sh");
            result = result.Replace("щ", "sch");
            result = result.Replace("\'", "`");
            result = result.Replace("ь", "\'");
            result = result.Replace("ю", "yu");
            result = result.Replace("я", "ya");   
            result = result.Replace(" ", "_");
            result = result.Replace("&", "");
            result = result.Replace("ё", "");
            result = result.Replace("ъ", "yi");
            result = result.Replace("ы", "i");
            result = result.Replace("э", "ye");
            return result;
        }
        public static void FirstStart()
        {
            if (!Directory.Exists(Defs.FILE_HASHTABLESDIRECTORY))
                Directory.CreateDirectory(Defs.FILE_HASHTABLESDIRECTORY);
        }
    }
}
