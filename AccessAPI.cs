using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using OpenAI_API;
using OpenAI_API.Chat;

namespace BlazorApp
{
    public class AccessAPI
    {
        private readonly OpenAIAPI _api;

        public AccessAPI(APIAuthentication? apiKey = null, string baseUrl = "https://api.openai.com/")
        {
            _api = new OpenAIAPI(apiKey)
            {
                ApiUrlFormat = baseUrl,
            };
        }

        public async Task<string> GetAnswerFromQwen(string question)
        {
            var query = $"问题：{question}\n";

            var messages = new List<ChatMessage>
            {
                new ChatMessage()
                {
                    Role = ChatMessageRole.System,
                    Content = "### 角色 ###"
                }, // system prompt
                new ChatMessage()
                {
                    Role = ChatMessageRole.User,
                    Content = query
                } // user prompt
            };

            var response = await _api.Chat.CreateChatCompletionAsync(new ChatRequest
            {
                Model = "/data/shihongchang/train_lf/model", // 填入模型
                Messages = (IList<OpenAI_API.Chat.ChatMessage>)messages,
                // Stream = false, // Removed because it is read-only
                Temperature = 0,
                MaxTokens = 1024
            });

            return response.Choices[0].Message.TextContent.Trim();
        }

        public async Task ProcessData(string inputFile, string outputFile)
        {
            // 检查并创建输出文件的目录
            var outputDir = Path.GetDirectoryName(outputFile);
            if (!Directory.Exists(outputDir))
            {
                if (outputDir != null)
                {
                    Directory.CreateDirectory(outputDir);
                }
            }

            var jsonData = await File.ReadAllTextAsync(inputFile, Encoding.UTF8);
            var data = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(jsonData);
            if (data == null)
            {
                throw new InvalidOperationException("The data deserialized from the input file is null.");
            }

            foreach (var item in data)
            {
                var question = item.GetValueOrDefault("question", "");
                var answer = await GetAnswerFromQwen(question);
                item["answer"] = answer;
            }

            var outputJson = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
            await File.WriteAllTextAsync(outputFile, outputJson, Encoding.UTF8);

            Console.WriteLine($"Processing completed. Processed data count: {data.Count}");
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var apiKey = "your-api-key"; // 替换为您的API密钥
            var accessApi = new AccessAPI(new APIAuthentication(apiKey), "http://localhost:9870/v1");

            var inputFile = "/data/shihongchang/xinlishuju/test/processed_data/zhihu_processed_filtered_0.json"; // 输入json文件
            var outputFile = "/data/shihongchang/xinlishuju/test/processed_data/zhihu_processed_filtered_1.json"; // 输出json文件

            await accessApi.ProcessData(inputFile, outputFile);
        }
    }
}
