using Newtonsoft.Json.Linq;

namespace KoreanQuestionMark.BotConfig
{
    struct Config //봇의 설정들읠 제어하는 곳
    {
        string _token;
        string _stdictKey;
        ulong[] _testers;
        ulong[] _testGuilds;
        bool _forTest; //true: testGuilds, owners만 사용 가능
        
        public string Token
        {
            get { return _token; }
        }
        public string StdictKey
        {
            get { return _stdictKey; }
        }
        public ulong[] Testers
        {
            get { return _testers; }
        }
        public ulong[] TestGuilds
        {
            get { return _testGuilds; }
        }
        public bool ForTest
        {
            get { return _forTest; }
        }

        public Config(string token, string stdictKey , ulong[] testers, ulong[] testGuilds, bool forTest)
        {
            _token = token;
            _stdictKey = stdictKey;
            _testers = testers;
            _testGuilds = testGuilds;
            _forTest = forTest;
        }
        public static Config Load(string path)
        {
            if(File.Exists(path))
            {
                JObject configJson = JObject.Parse(File.ReadAllText(path));
                Config config = new Config(
                    configJson["token"].ToString(),
                    configJson["stdictKey"].ToString(),
                    configJson["testers"].ToObject<ulong[]>(),
                    configJson["testGuilds"].ToObject<ulong[]>(),
                    (bool)configJson["forTest"]
                );
                return config;
            }
            else
            {
                throw new FileNotFoundException("BotConfig.json 파일을 찾을 수 없습니다.");
            }
        }
        public static Config Make()
        {
            Console.WriteLine("디스코드 봇의 토큰을 입력하세요.");
            string token = Console.ReadLine();
            Console.WriteLine("표준국어대사전의 API 키를 입력하세요.");
            string stdictKey = Console.ReadLine();
            Console.WriteLine("봇의 테스터들의 ID를 입력하세요. (여러 개의 ID를 입력하려면 /로 구분하세요.)");
            string[] testers = Console.ReadLine().Split('/');
            ulong[] uTesters = new ulong[testers.Length];
            for(int i = 0; i < testers.Length; i++)
            {
                uTesters[i] = ulong.Parse(testers[i]);
            }
            Console.WriteLine("봇의 테스트 서버들의 ID를 입력하세요. (여러 개의 ID를 입력하려면 /로 구분하세요.)");
            string[] testGuilds = Console.ReadLine().Split('/');
            ulong[] uTestGuilds = new ulong[testGuilds.Length];
            for(int i = 0; i < testGuilds.Length; i++)
            {
                uTestGuilds[i] = ulong.Parse(testGuilds[i]);
            }
            Console.WriteLine("이 봇을 테스터들만 테스트용 서버에서만 사용할 수 있도록 설정할까요? (y/N)");
            bool onlyTest = Console.ReadLine() == "y" ? true : false;

            JObject json = new JObject();
            json["token"] = token;
            json["stdictKey"] = stdictKey;
            json["testers"] = JArray.FromObject(uTesters);
            json["testGuilds"] = JArray.FromObject(uTestGuilds);
            json["forTest"] = onlyTest;
            File.WriteAllText("BotConfig.json", json.ToString());
            Console.WriteLine("설정이 완료되었습니다.");
            return new Config(token, stdictKey, uTesters, uTestGuilds, onlyTest);
        }
    }
}