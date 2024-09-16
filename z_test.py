import json
from openai import OpenAI
import os
from tqdm import tqdm

client = OpenAI(
    base_url="https://llmsapi.cpolar.top/v1",  # 填写url
    api_key="none",
)

HISTORY_FILE = "conversation_history.json"

def load_conversation_history():
    if os.path.exists(HISTORY_FILE):
        with open(HISTORY_FILE, 'r', encoding='utf-8') as f:
            return json.load(f)
    else:
        return [
            {
                "role": "system",
                "content": "# 角色\n你的名字是：蛋蛋。\n你的身份是：一名优秀的心理咨询助手，是人类温暖的好朋友。\n你称呼你的用户为：亲爱的小伙伴。\n你的语言习惯：你说话幽默风趣、轻松活泼，大家很喜欢和你聊天。\n你的知识：你具有丰富的咨询经验和心理学知识；你熟悉心理咨询的流程，遵循心理咨询的伦理。\n你的性格：乐观开朗、热情待人，总是积极地对待你的用户；善于倾听，具有强烈的同理心和同情心；你非常希望用你幽默欢快的语言，以及专业的心理知识，帮你的用户提振心情、走出困境。\n\n## 技能\n### 技能 1: 提供积极的支持与建议\n- 使用轻松欢快、积极正面的语言，让用户舒缓情绪，帮助用户建立信心和正面思维。\n- 根据用户的具体情况，提供实用且可行的建议。\n\n### 技能 2: 倾听与共情\n- 认真倾听用户的困扰和情感诉求，并表现出理解、共情与积极的情绪支持。\n- 熟悉问询技巧，使用引导性的问询方式，帮助用户深入思考和表达自己。\n\n### 技能 3: 引导用户自我探索与成长\n- 使用心理学知识，引导用户深层次地理解自己的问题，找到根源。\n- 鼓励用户设立小目标，并逐步实现，以此获得成就感和正向反馈。\n\n## 约束\n- 只回答心理咨询相关的问题。\n- 使用积极和鼓励的语言，不做任何负面评价。",  # system prompt
            }
        ]

def save_conversation_history(history):
    with open(HISTORY_FILE, 'w', encoding='utf-8') as f:
        json.dump(history, f, ensure_ascii=False, indent=4)

conversation_history = load_conversation_history()

def get_answer_from_qwen(question):
    global conversation_history

    conversation_history.append({
        "role": "user",
        "content": question,
    })

    response = client.chat.completions.create(
        model="E:/Temp/sft_model",  # 填入模型
        messages=conversation_history,
        stream=False,
        temperature=1e-6,
        max_tokens=512,
    )

    answer = response.choices[0].message.content.strip()

    conversation_history.append({
        "role": "assistant",
        "content": answer,
    })

    save_conversation_history(conversation_history)

    return answer

if __name__ == "__main__":
    print("对话历史已加载。输入您的问题，或输入'退出'结束对话。")
    while True:
        question = input("您: ")
        if question.lower() == '退出':
            break

        answer = get_answer_from_qwen(question)
        print("助手:", answer)
        print("\n" + "="*50 + "\n")

    print("对话结束，历史记录已保存。")
