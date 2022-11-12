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
    public class StaticPageClass
    {
        public string? Name { get; set; }
        public string? Content { get; set; }

        public StaticPageClass(string? name, string? content)
        {
            Name = name;
            Content = content;
        }
        public StaticPageClass(string? name)
        {
            Name = name;
        }
        public StaticPageClass() { }
    }
    public class NewsPageClass
    {
        public string? Name { get; set; }
        public string? Tag { get; set; }
        public string? Content { get; set; }
        public string? Date { get; set; }
        public string? Logo { get; set; }

        public NewsPageClass(string? name, string? tag, string? content, string? date, string? logo)
        {
            Name = name;
            Tag = tag;
            Content = content;
            Date = date;
            Logo = logo;
        }
        public NewsPageClass() { }
    }
    public class UsersClass
    {
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Access { get; set; }

        public UsersClass(string? login, string? password, string? access)
        {
            Login = login;
            Password = password;
            Access = access;
        }
        public UsersClass() { }
    }

    public static class Hashtables
    {
        public static Dictionary<string, MainPageClass> MainPages { get; set; }
            = MainFileToHashtable(Defs.FILE_HASHTABLESDIRECTORY + '/' + Defs.FILE_MAINHASHTABLE);
        public static Dictionary<string, StaticPageClass> StaticPages { get; set; }
            = StaticFileToHashtable(Defs.FILE_HASHTABLESDIRECTORY + '/' + Defs.FILE_STATICHASHTABLE);
        public static List<NewsPageClass> NewsPages { get; set; }
            = NewsFileToHashtable(Defs.FILE_HASHTABLESDIRECTORY + '/' + Defs.FILE_NEWSHASHTABLE);
        public static List<UsersClass> Users { get; set; }
            = UsersFileToHashtable(Defs.FILE_HASHTABLESDIRECTORY + '/' + Defs.FILE_USERSHASHTABLE);

        public static void HashtableToFile<T>(T table, string path)
        {
            try
            {
                string json = JsonConvert.SerializeObject(table);
                File.WriteAllText(path, json);
            }
            catch (Exception a)
            {
                Console.WriteLine(a);
            }
        }
        private static Dictionary<string, MainPageClass> MainFileToHashtable(string path)
        {
            if (!File.Exists(path))
                File.Create(path);
            var file = new FileInfo(path);
            if (file.Length == 0)
                return new Dictionary<string, MainPageClass>();
            return JsonConvert.DeserializeObject<Dictionary<string, MainPageClass>>
                (File.ReadAllText(path)) ?? new Dictionary<string, MainPageClass>();
        }
        private static Dictionary<string, StaticPageClass> StaticFileToHashtable(string path)
        {
            if (!File.Exists(path))
                File.Create(path);
            var file = new FileInfo(path);
            if (file.Length == 0)
            {
                Dictionary<string, StaticPageClass> temp = new()
                {
                    { Defs.PAGE_STATIC_INDEX, new StaticPageClass("", "") }
                };
                foreach (var item in Defs.PAGE_STATIC_PAGES)
                    temp.Add(item, new StaticPageClass("", ""));
                HashtableToFile(temp, path);
            }
            return JsonConvert.DeserializeObject<Dictionary<string, StaticPageClass>>
                (File.ReadAllText(path)) ?? new Dictionary<string, StaticPageClass>();
        }
        private static List<NewsPageClass> NewsFileToHashtable(string path)
        {
            if (!File.Exists(path))
                File.Create(path);
            var file = new FileInfo(path);
            if (file.Length == 0)
                return new List<NewsPageClass>();
            return JsonConvert.DeserializeObject<List<NewsPageClass>>
                (File.ReadAllText(path)) ?? new List<NewsPageClass>();
        }
        private static List<UsersClass> UsersFileToHashtable(string path)
        {
            if (!File.Exists(path))
                File.Create(path);
            var file = new FileInfo(path);
            if (file.Length == 0)
            {
                List<UsersClass> temp = new()
                {
                    new("admin", "admin", Defs.USER_ADMIN_ACCESS)
                };
                HashtableToFile(temp, path);
                return temp;
            }
            return JsonConvert.DeserializeObject<List<UsersClass>>
                (File.ReadAllText(path)) ?? new List<UsersClass>();
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
            Directory.CreateDirectory(Defs.FILE_HASHTABLESDIRECTORY);
            Directory.CreateDirectory(Defs.FILE_IMGDIRECTORY);
            Directory.CreateDirectory(Defs.FILE_IMGDIRECTORY + '/' + Defs.FILE_FORMIMAGESDIRECTORY);
            Directory.CreateDirectory(Defs.FILE_IMGDIRECTORY + '/' + Defs.FILE_NEWSIMAGESDIRECTORY);
        }
    }
}
