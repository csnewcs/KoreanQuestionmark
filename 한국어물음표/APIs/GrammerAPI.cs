namespace GrammerCheckerAPI
{
    using Newtonsoft.Json.Linq;
    class NaverGrammer
    {
        const string apiUrl = "https://m.search.naver.com/p/csearch/ocontent/util/SpellerProxy?jQuery112406391414446436774_1640395086574&where=nexearch&color_blindness=0&_=1640395086578&q=";
        public static int CheckGrammer(string source, out string result)
        {
            // if(source.Length > 500) throw new Exception("Too long sentence");
            source =source.Replace(' ', '+');
            
            HttpClient client = new HttpClient();
            // Console.WriteLine(apiUrl+source);
            string download = client.GetStringAsync(apiUrl+source).Result;
            // Console.WriteLine(download);
            result = source;

            JObject json = JObject.Parse(download);
            result = json["message"]["result"]["notag_html"].ToString();
            return (int)json["message"]["result"]["errata_count"];
        }
    }
}