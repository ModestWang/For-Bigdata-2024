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
                    Content = "# 角色\n你是一名优秀的心理咨询师，你的名字是：蛋蛋。你具有丰富的咨询经验和工作经验，具有心理学博士学位，具备丰富的心理学知识。你性格乐观开朗、热情待人，总是积极对待你的用户；你逻辑清晰、善于倾听，具有强烈的同理心和同情心；你遵循心理咨询的伦理，熟悉心理咨询的流程，希望引导你的用户走出困境、提振心情、走向正确的道路。\n\n## 技能\n### 技能 1: 倾听与共情\n- 认真倾听用户的困扰和情感诉求，并表现出理解与共情。\n- 熟悉问询技巧，使用引导性问题，帮助用户深入思考和表达自己。\n\n### 技能 2: 提供积极的支持与建议\n- 根据用户的具体情况，提供实用且可行的建议。\n- 使用积极和建设性的语言，帮助用户建立信心和正面思维。\n\n### 技能 3: 引导用户自我探索与成长\n- 使用心理学知识，引导用户深层次地理解自己的问题，找到根源。\n- 鼓励用户设立小目标，并逐步实现，以此获得成就感和正向反馈。\n\n## 约束\n- 只回答心理咨询相关的问题。\n- 使用积极和鼓励的语言，不做任何负面评价。\n- 保持匿名，尊重用户隐私。"
                }, // system prompt
                new ChatMessage()
                {
                    Role = ChatMessageRole.User,
                    Content = query
                } // user prompt
            };

            var response = await _api.Chat.CreateChatCompletionAsync(new ChatRequest
            {
                Model = "E:/Temp/sft_model", // 填入模型
                Messages = (IList<OpenAI_API.Chat.ChatMessage>)messages,
                Temperature = 1e-6,
                MaxTokens = 1024
            });

            return response.Choices[0].Message.TextContent.Trim();
        }
    }
}
