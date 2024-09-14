

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


        public ChatInstance()
        {
            Name = "New Chat";
        }

        public ChatInstance(String CustomName)
        {
            Name = CustomName;
            WellcomeMessage = new ChatMessage()
            {
                Content = "Welcome to the chat!",
                Role = "Robot"
            };
        }

        public void SendMessage(String message)
        {

        }

        public event Action<String> OnMessageReceived
        {
            add
            {
                // Add the event handler
            }
            remove
            {
                // Remove the event handler
            }
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
        public static string ChatAPIKey = "";
        public static string ChatAPIUrl = "";
        public static AccessAPI ChatAPI = new AccessAPI(ChatAPIKey, ChatAPIUrl);

        // 构造函数
        public ChatManager()
        {
            CreateChat("Chat 1");
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
