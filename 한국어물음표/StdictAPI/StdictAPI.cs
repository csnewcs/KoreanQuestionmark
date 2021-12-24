namespace StdictAPI
{
    using Newtonsoft.Json.Linq;
    
    class Stdict
    {
        const string apiUrl = "https://stdict.korean.go.kr/api/search.do";
        private readonly string key;
        public Stdict(string key)
        {
            this.key = key;
        }
        public SimpleWord[] Search(string q)
        {
            string url = $"{apiUrl}?key={key}&q={q}&req_type=json&num=100";
            string json = new HttpClient().GetStringAsync(url).Result;
            JArray items = JObject.Parse(json)["channel"]["item"] as JArray;
            List<SimpleWord> words = new List<SimpleWord>();
            foreach (var item in items)
            {
                words.Add(new SimpleWord(item["word"].ToString(), (int)item["sup_no"], item["pos"].ToString(), (int)item["target_code"], item["sense"]["definition"].ToString()));
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