

namespace BlazorApp
{
    // Message类
    public class ChatMessage
    {
        public required string Role { get; set; }
        public required string Content { get; set; }
    }

    // Chat实例
    public class ChatInstance
    {
        public string? Name { get; set; } // Chat 名称
        public ChatMessage? WellcomeMessage { get; set; } // 欢迎消息
        public ChatMessage? InputMessage { get; set; } // 输入消息
        public ChatMessage? OutputMessage { get; set; } // 输出消息
        public List<ChatMessage>? InputMessages { get; } = new List<ChatMessage>(); // 输入消息列表
        public List<ChatMessage>? OutputMessages { get; } = new List<ChatMessage>(); // 输出消息列表
        public bool IsEditing { get; set; } = false; // 是否正在编辑

        // 定义欢迎问候数组
        public string[] welcomeMessages = new string[]
        {
            "欢迎！很高兴见到你。",
            "你好！希望你有美好的一天。",
            "我是蛋蛋，很高兴见到你😘",
            "嗨！有什么我可以帮你的吗？",
            "你好，我是蛋蛋！很高兴为你服务。"
        };


        public ChatInstance()
        {
            Name = "New Chat";

            // 生成随机索引
            Random random = new Random();
            int index = random.Next(welcomeMessages.Length);

            // 设置欢迎问候
            WellcomeMessage = new ChatMessage()
            {
                Content = welcomeMessages[index],
                Role = "System"
            };
        }

        public ChatInstance(String CustomName)
        {
            Name = CustomName;

            // 生成随机索引
            Random random = new Random();
            int index = random.Next(welcomeMessages.Length);

            // 设置欢迎问候
            WellcomeMessage = new ChatMessage()
            {
                Content = welcomeMessages[index],
                Role = "System"
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
        public static string ChatAPIKey = "none";
        public static string ChatAPIUrl = "https://llmsapi.cpolar.top/v1";
        public static AccessAPI ChatAPI = new AccessAPI(ChatAPIKey, ChatAPIUrl);

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
