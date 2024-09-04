using BlazorApp.Components.Pages;

namespace BlazorApp
{
    // Chat实例
    public class ChatInstance
    {
        public String Name { get; set; } = "New Chat"; // Chat 名称
        public String WellcomeMessage { get; set; } = "Welcome to the chat!"; // 欢迎消息
        public String InputMessage { get; set; } // 输入消息
        public String OutputMessage { get; set; } // 输出消息
        public List<String> InputMessages { get; } = new List<String>(); // 输入消息列表
        public List<String> OutputMessages { get; } = new List<String>(); // 输出消息列表

        public ChatInstance()
        {
            Name = "New Chat";
        }

        public ChatInstance(String CustomName)
        {
            Name = CustomName;
        }

        public void SendMessage(String message)
        {
            WellcomeMessage = message;
        }

        public event Action<String> OnMessageReceived;
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

        // 构造函数
        public ChatManager() {
            CreateChat("Chat 1");
        }

        // 创建新的聊天
        public void CreateChat(String chatName= "New Chat")
        {
            var newChat = new ChatInstance(chatName);
            Chats.Add(newChat);
        }

        // 删除聊天
        public void DeleteChat(int index)
        {
            if(Chats.Count > 0)
            {
                if (index >= 0 && index < Chats.Count)
                {
                    Chats.RemoveAt(index);
                }
            }
        }


    }
}
