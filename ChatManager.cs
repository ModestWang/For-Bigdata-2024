

using BlazorApp.Components.Pages;

namespace BlazorApp
{
    // Message类
    public class ChatMessage
    {
        public required string role { get; set; }
        public required string content { get; set; }
    }

    // Chat实例
    public class ChatInstance
    {
        public string? Name { get; set; } // Chat 名称
        public string? ChatId { get; set; } // Chat ID
        public ChatMessage? WellcomeMessage { get; set; } // 欢迎消息
        public bool IsEditing { get; set; } = false; // 是否正在编辑

        // 定义欢迎问候数组
        public string[] welcomeMessages = new string[]
        {
            "欢迎！我是蛋蛋，很高兴见到你😊",
            "你好小伙伴~蛋蛋希望你有美好的一天🌞",
            "我是蛋蛋，很高兴见到你😘",
            "嗨！有什么蛋蛋可以帮你的吗？🤔",
            "你好，我是蛋蛋！很高兴为你服务😃",
            "你好！今天过得怎么样？😊",
            "欢迎回来！有什么新鲜事要分享吗？📰",
            "嗨！蛋蛋希望你今天心情愉快😄",
            "你好小伙伴~很高兴再次见到你👋",
            "嗨！有什么蛋蛋能为你做的吗？🤗",
            "你好！蛋蛋希望你今天一切顺利~🍀",
            "欢迎！有什么蛋蛋能帮上忙的吗？🛠️",
            "嗨！很高兴你来了😃",
            "你好！蛋蛋希望你今天过得愉快~🌈",
            "欢迎！有什么我可以为你效劳的吗？🙏",
            "嗨~蛋蛋希望你今天充满阳光🌞",
            "你好！很高兴见到你，亲爱的小伙伴👋",
            "欢迎！希望你今天过得开心😊",
            "嗨！有什么我可以帮你解决的吗？🤔",
            "你好！希望你今天一切顺利🍀"
        };

        public ChatInstance(String CustomName = "New Chat")
        {
            Name = CustomName;
            ChatId = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".json";

            // 生成随机索引
            Random random = new Random();
            int index = random.Next(welcomeMessages.Length);

            // 设置欢迎问候
            WellcomeMessage = new ChatMessage()
            {
                content = welcomeMessages[index],
                role = "assistant"
            };
        }
    }

    // Chat管理器
    public class ChatManager
    {
        // Chat管理器实例
        public static ChatManager ChatManagerInstance { get; } = new ChatManager();

        // Chat实例列表
        public List<ChatInstance> Chats { get; } = new List<ChatInstance>();

        // 当前聊天的索引
        public int CurrentChatIndex { get; set; }

        // API
        // public static string ChatAPIKey = "none";
        // public static string ChatAPIUrl = "https://llmsapi.cpolar.top/v1";
        // public static AccessAPI ChatAPI = new AccessAPI(ChatAPIKey, ChatAPIUrl);

        // 构造函数
        public ChatManager()
        {
            CreateChat("New Chat");
        }

        // 创建新的聊天
        public void CreateChat(String chatName = "New Chat")
        {
            var newChat = new ChatInstance(chatName);
            Chats.Add(newChat);
        }

        // 删除聊天
        public void DeleteChat(int index)
        {
            if (Chats.Count > 0)
            {
                if (index >= 0 && index < Chats.Count)
                {
                    Chats.RemoveAt(index);
                }
            }
        }
    }
}
