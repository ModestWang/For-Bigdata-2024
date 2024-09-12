using System;
using System.Threading.Tasks;
using OpenAI_API;
using OpenAI_API.Completions;

namespace BlazorApp
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="apiKey">OpenAI API密钥</param>
    public class AccessAPI
    {
        private readonly OpenAIAPI _api;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="apiKey">OpenAI API密钥</param>
        /// <param name="baseUrl">OpenAI API的基础URL</param>
        public AccessAPI(APIAuthentication? apiKey = null, string baseUrl = "https://api.openai.com/")
        {
            _api = new OpenAIAPI(apiKey)
            {
                ApiUrlFormat = baseUrl,
            };
        }

        /// <summary>
        /// 验证问答对的合理性
        /// </summary>
        /// <param name="question">问题文本</param>
        /// <param name="description">问题的描述</param>
        /// <param name="answer">提供的答案</param>
        /// <returns>返回“合理”或“不合理”</returns>
        /// <exception cref="Exception">当API调用失败时抛出异常</exception>
        public async Task<string> ValidateQAAsync(QA_Pair qaPair)
        {
            var question = qaPair.Question;
            var description = qaPair.Description;
            var answer = qaPair.Answer;
            var prompt = $"问题：{question}\n描述：{description}\n答案：{answer}\n你是一名评估员，你的用户在生活上遇到了许多问题，因此他们给出了问题和描述，并获得了一些答案。\n请你判断上述答案是否合理，返回“合理”或“不合理”。";

            var completionRequest = new CompletionRequest
            {
                Model = "text-davinci-003",
                Prompt = prompt,
                MaxTokens = 512,
                Temperature = 0.5
            };

            var result = await _api.Completions.CreateCompletionAsync(completionRequest);

            if (result != null && result.Completions.Count > 0)
            {
                var judgement = result.Completions[0].Text.Trim();
                return judgement.Contains("合理") ? "合理" : "不合理";
            }
            else
            {
                throw new Exception("Error validating QA: No response from OpenAI API");
            }
        }

        /// <summary>
        /// 验证问答对的合理性
        /// </summary>
        /// <param name="qaPair">问题-答案对</param>
        /// <returns>返回“相关”或“不相关”</returns>
        /// <exception cref="Exception">当API调用失败时抛出异常</exception>
        public async Task<string> ValidateQAFewShotAsync(QA_Pair qaPair)
        {
            var question = qaPair.Question;
            var description = qaPair.Description;

            var fewShotCases = new List<QA_Pair>
            {
                new QA_Pair { Question = "姜昆对中国曲艺界做过哪些贡献？", Description = "", Answer = "不相关" },
                new QA_Pair { Question = "让你觉得人心真的很冷漠的事是什么？", Description = "我就说我上一段的感情吧。和他怎么说也是分分合合一年了，是不同专业的大学同学。和他在一起的时候，我是真的觉得他很贴心，我甚至经常幻想我们的未来。可是后来他会同时和很多女生进行暧昧纠缠，我发现了以后分手，他也立马和其他女生在一起了。真的失望了，以后会更爱自己。", Answer = "不相关" },
                new QA_Pair { Question = "恶作剧之吻为何能让人如此难以忘怀？", Description = "看起来很非主流，却让人一看就着迷，沉迷于剧中的爱情故事当中", Answer = "不相关" },
                new QA_Pair { Question = "委屈到底是什么滋味？", Description = "", Answer = "不相关" },
                new QA_Pair { Question = "男朋友忙事业，压力大，不能分心恋爱，希望可以过段时间再说，怎么办？", Description = "", Answer = "相关" },
                new QA_Pair { Question = "家人不同意我远嫁，我该怎么办？", Description = "本人女，家庭条件不太好，人丁单薄，如果我嫁了，家里就祖母跟父亲了。我跟L在一起快四年了，他也很爱我，他们家人都还挺认可我的，现在家人不同意的原因大概是异地", Answer = "相关" },
                new QA_Pair { Question = "患躁郁症的人多吗？", Description = "", Answer = "相关" },
                new QA_Pair { Question = "觉得我爸很烦怎么办？", Description = "总想和我聊天但话题又特别无聊，明明不懂还要侃历史侃时政，一个问题要重复问无数遍却还是不记得答案", Answer = "相关" }
            };

            var query = $"问题：{question}\n描述：{description}\n";

            var messages = new List<ChatMessage>
            {
                new ChatMessage
                {
                    Role = "system",
                    Content = "角色：你是一个专业的评估员，具有丰富的心理学知识背景。\n背景：你的用户在生活上遇到了许多困难，因此他们给出了问题和描述。但里面混入了一些与心理学无关的问题和描述，对你造成了很大麻烦。\n任务：请仔细分析给出的问题和描述，判断问题和描述是否与心理学、心理咨询学的知识相关，返回“相关”或者“不相关”。\n文本中可能包含无关的特殊字符，请对其进行过滤。"
                }
            };

            foreach (var caseItem in fewShotCases)
            {
                messages.Add(new ChatMessage { Role = "user", Content = $"问题：{caseItem.Question}\n描述：{caseItem.Description}\n" });
                messages.Add(new ChatMessage { Role = "assistant", Content = $"{caseItem.Answer}\n" });
            }

            messages.Add(new ChatMessage { Role = "user", Content = query });

            var completionRequest = new CompletionRequest
            {
                Model = "text-davinci-003",
                MultiplePrompts = messages.Select(m => m.Content).ToArray(),
                MaxTokens = 512,
                Temperature = 0.5
            };

            var result = await _api.Completions.CreateCompletionAsync(completionRequest);

            if (result != null && result.Completions.Count > 0)
            {
                var judgement = result.Completions[0].Text.Trim();
                return judgement.Contains("合理") ? "合理" : "不合理";
            }
            else
            {
                throw new Exception("Error validating QA: No response from OpenAI API");
            }
        }
    }

    /// <summary>
    /// 问题-答案对
    /// </summary>
    public class QA_Pair
    {
        public string Question { get; set; } = "";
        public string Description { get; set; } = "";
        public string Answer { get; set; } = "";
    }
}
