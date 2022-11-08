namespace FMI_web
{
    public static class Defs
    {
        #region defenitions
        //Page classes
        public const string CLASS_MAIN = "main";
        public const string CLASS_STATIC = "static";
        public const string CLASS_NEWS = "news";
        //Page types
        public const string TYPE_PAGE = "page";
        public const string TYPE_LINK = "link";
        public const string TYPE_LIST = "list";
        //Pages name
        public const string PAGE_MAINPAGE = "Mainpage";
        public const string PAGE_STATIC_INDEX = "Index";
        public const string PAGE_NEWSLIST = "Newslist";
        public const int PAGE_NEWSLIST_COUNT = 9;
        public const int PAGE_NEWSLIST_SIDEBARCOUNT = 3;
        public const string PAGE_NEWSPAGE = "Newspage";
        public static readonly string[] PAGE_STATIC_PAGES = { "Abiturient" };
        //Page type of edit
        public const string TYPE_EDIT_ADD = "add";
        public const string TYPE_EDIT_EDIT = "edit";
        //Form input names
        public const string INPUT_TYPEOFEDIT = "typeofedit";
        public const string INPUT_PAGECLASS = "pageclass";
        public const string INPUT_FULLPAGENAME = "fullpagename";
        public const string INPUT_PARENT = "parent";
        public const string INPUT_ID = "id";
        //Menu list
        public const string MENU_OSV_D = "osvitnya_diyalnist";
        public const string MENU_PRO_F = "pro_fakultet";
        public const string MENU_ONL_R = "onlayn_rozklad";
        public const string MENU_KAF = "kafedry";
        public const string MENU_SKR_D = "skrynka_doviry";
        //View Data
        public const string VIEWDATA_DEFAULTNAME = "Факультет математики та інформатики";
        public const string VIEWDATA_PAGENAME = "PageName";
        public const string VIEWDATA_PAGE = "Page";
        public const string VIEWDATA_PAGEID = "PageId";
        //File path
        public const string FILE_HASHTABLESDIRECTORY = @"wwwroot/hashtables";
        public const string FILE_MAINHASHTABLE = "MainPages.json";
        public const string FILE_STATICHASHTABLE = "StaticPages.json";
        public const string FILE_NEWSHASHTABLE = "NewsPages.json";
        public const string FILE_IMGDIRECTORY = @"wwwroot/img";
        public const string FILE_IMGDIRECTORYSHORT = @"img";
        public const string FILE_FORMIMAGESDIRECTORY = "formimages";
        public const string FILE_NEWSIMAGESDIRECTORY = "newsimages";
        #endregion
    }
}
