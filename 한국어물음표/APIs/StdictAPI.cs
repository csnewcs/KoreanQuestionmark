namespace StdictAPI
{
    using Newtonsoft.Json.Linq;
    
    class Stdict
    {
        const string searchApiUrl = "https://stdict.korean.go.kr/api/search.do";
        const string viewApiUrl = "https://stdict.korean.go.kr/api/view.do";
        private readonly string key;
        public Stdict(string key)
        {
            this.key = key;
        }
        public SimpleWord[] Search(string q, long pos = 0)
        {
            string url = $"{searchApiUrl}?key={key}&q={q}&req_type=json&num=25&advanced=y&pos={pos}";
            string json = new HttpClient().GetStringAsync(url).Result;
            if(string.IsNullOrEmpty(json) || json == "{}")
            {
                return new SimpleWord[0];
            }
            JArray items = JObject.Parse(json)["channel"]["item"] as JArray;
            List<SimpleWord> words = new List<SimpleWord>();
            foreach (var item in items)
            {
                words.Add(new SimpleWord(item["word"].ToString(), (int)item["sup_no"], item["pos"].ToString(), (int)item["target_code"], item["sense"]["definition"].ToString().Replace("<sup style='font-size:11px;'>", "^").Replace("</sup>", "").Replace("<br>", "").Replace("<br/>", "")));
            }
            return words.ToArray();
        }
        public DetailWord[] MoreSearch(int targetCode)
        {
            string url = $"{viewApiUrl}?key={key}&method=target_code&req_type=json&q={targetCode}&type_search=view"; //type_search는 문서에 없는데 없으면 에러;;
            // Console.WriteLine(url);
            string json = new HttpClient().GetStringAsync(url).Result;
            if(string.IsNullOrEmpty(json))
            {
                return new DetailWord[0];
            }
            // Console.WriteLine(json);
            JObject obj = JObject.Parse(json)["channel"]["item"]["word_info"] as JObject;
            JArray means = obj["pos_info"][0]["comm_pattern_info"][0]["sense_info"] as JArray;
            List<DetailWord> words = new List<DetailWord>();
            foreach (var mean in means)
            {
                JArray exampleInfos = mean["example_info"] as JArray;
                string[] examples = exampleInfos == null ? new string[0] : new string[exampleInfos.Count];
                for (int i = 0; i < examples.Length; i++)
                {
                    examples[i] = exampleInfos[i]["example"].ToString();
                }
                words.Add(new DetailWord(obj["word"].ToString(), examples, mean["definition"].ToString().Replace("<sup style='font-size:11px;'>", "^").Replace("</sup>", "").Replace("<br>", "").Replace("<br/>", ""), obj["pronunciation_info"]?[0]?["pronunciation"]?.ToString()));
            }
            return words.ToArray();
        }
    }
    struct SimpleWord //검색결과로 나오는 단어, 적은 정보만 들어 있음
    {
        string _word; //단어
        int _supNo; //어깨번호 (동음어)
        int _targetCode; //자세한 검색 시 활용
        string _pos; //품사
        string _definition; //뜻

        public string Word
        {
            get { return _word; }
        }
        public int SupNo
        {
            get { return _supNo; }
        }
        public int TargetCode
        {
            get { return _targetCode; }
        }
        public string Pos
        {
            get { return _pos; }
        }
        public string Definition
        {
            get { return _definition; }
        }
        public SimpleWord(string word, int supNo, string pos, int targetCode , string definition)
        {
            _word = word;
            _supNo = supNo;
            _pos = pos;
            _targetCode = targetCode;
            _definition = definition;
        }
    }
    struct DetailWord //사전보기로 나오는 단어
    {
        string _word;
        string[] _examples;
        string _definition;
        string? _pronunciation; //발음
        public string Word
        {
            get { return _word; }
        }
        public string[] Examples
        {
            get { return _examples; }
        }
        public string Definition
        {
            get { return _definition; }
        }
        public string? Pronunciation
        {
            get { return _pronunciation; }
        }
        public DetailWord(string word, string[] examples, string definition, string? pronunciation)
        {
            _word = word;
            _examples = examples;
            _definition = definition;
            _pronunciation = pronunciation;
        }

    }
    // public enum 품사 //영단어를 몰?루
    // {
    //     모두 = 0,
    //     명사 = 1,
    //     대명사 = 2,
    //     수사 = 3,
    //     조사 = 4,
    //     동사 = 5,
    //     형용사 = 6,
    //     관형사 = 7,
    //     부사 = 8,
    //     감탄사 = 9,
    //     접사 = 10,
    //     의존명사 = 11,
    //     보조동사 = 12,
    //     보조형용사 = 13,
    //     어미 = 14,
    //     없음 = 15,
    // }
}