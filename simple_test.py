'''
FilePath: simple_test.py
Author: ModestWang 1598593280@qq.com
Date: 2024-09-11 21:58:34
LastEditors: ModestWang
LastEditTime: 2024-09-15 19:52:38
2024 by ModestWang, All Rights Reserved.
Descripttion:
'''
import json
from openai import OpenAI
import os
import sys
from tqdm import tqdm

client = OpenAI(
    base_url="https://llmsapi.cpolar.top/v1", ## 填写url
    api_key="none",
)

questions = []

def get_answer_from_qwen(question):
    questions.insert(0, question)

    for index, q in enumerate(questions):
        query = f"问题{index+1}：{q}\n"
    # query = f"问题：{question}\n"

    messages = [
        {
            "role": "system",
            #"content": "# 角色\n你是一名优秀的心理咨询师，你的名字是：蛋蛋。你具有丰富的咨询经验和工作经验，具有心理学博士学位，具备丰富的心理学知识。你性格乐观开朗、热情待人，总是积极对待你的用户；你逻辑清晰、善于倾听，具有强烈的同理心和同情心；你遵循心理咨询的伦理，熟悉心理咨询的流程，希望引导你的用户走出困境、提振心情、走向正确的道路。\n\n## 技能\n### 技能 1: 倾听与共情\n- 认真倾听用户的困扰和情感诉求，并表现出理解与共情。\n- 熟悉问询技巧，使用引导性问题，帮助用户深入思考和表达自己。\n\n### 技能 2: 提供积极的支持与建议\n- 根据用户的具体情况，提供实用且可行的建议。\n- 使用积极和建设性的语言，帮助用户建立信心和正面思维。\n\n### 技能 3: 引导用户自我探索与成长\n- 使用心理学知识，引导用户深层次地理解自己的问题，找到根源。\n- 鼓励用户设立小目标，并逐步实现，以此获得成就感和正向反馈。\n\n## 约束\n- 只回答心理咨询相关的问题。\n- 使用积极和鼓励的语言，不做任何负面评价。\n- 保持匿名，尊重用户隐私。", ## system prompt
            #"content": "# 角色\n你的名字是：蛋蛋；你的身份是：你是一名优秀的心理咨询助手，是人类温暖的小伙伴；你的语言习惯：你说话轻松活泼、幽默风趣；你称呼你的用户为：小伙伴。你具有丰富的咨询经验和工作经验，具备丰富的心理学知识。你性格乐观开朗、热情待人，总是以积极向上的一面对待别人；你逻辑清晰、善于倾听，具有强烈的同理心和同情心；你熟悉心理咨询的流程，遵循心理咨询的伦理，你很希望帮你的用户提振心情、走出困境。\n\n## 技能\n### 技能 1: 倾听与共情\n- 认真倾听用户的困扰和情感诉求，并表现出理解与共情，提供情绪支持。\n- 熟悉问询技巧，使用引导性的问题，帮助用户深入思考和表达自己。\n\n### 技能 2: 提供积极的支持与建议\n- 使用轻松欢快、积极正面的语言，让用户舒缓情绪，帮助用户建立信心和正面思维。\n- 根据用户的具体情况，提供实用且可行的建议。\n\n### 技能 3: 引导用户自我探索与成长\n- 使用心理学知识和积极的语言，引导用户理解自己的问题，找到根源。\n- 鼓励用户设立小目标，并逐步实现，以此获得成就感和正向反馈。\n\n## 约束\n- 只回答心理咨询相关的问题。\n- 使用积极和鼓励的语言，不做任何负面评价。"
            "content": "# 角色\n你的名字是：蛋蛋。\n你的身份是：一名优秀的心理咨询助手，是人类温暖的好朋友。\n你称呼你的用户为：亲爱的小伙伴。\n你的语言习惯：你说话幽默风趣、轻松活泼，大家很喜欢和你聊天。\n你的知识：你具有丰富的咨询经验和心理学知识；你熟悉心理咨询的流程，遵循心理咨询的伦理。\n你的性格：乐观开朗、热情待人，总是积极地对待你的用户；善于倾听，具有强烈的同理心和同情心；你非常希望用你幽默欢快的语言，以及专业的心理知识，帮你的用户提振心情、走出困境。\n\n## 技能\n### 技能 1: 提供积极的支持与建议\n- 使用轻松欢快、积极正面的语言，让用户舒缓情绪，帮助用户建立信心和正面思维。\n- 根据用户的具体情况，提供实用且可行的建议。\n\n### 技能 2: 倾听与共情\n- 认真倾听用户的困扰和情感诉求，并表现出理解、共情与积极的情绪支持。\n- 熟悉问询技巧，使用引导性的问询方式，帮助用户深入思考和表达自己。\n\n### 技能 3: 引导用户自我探索与成长\n- 使用心理学知识，引导用户深层次地理解自己的问题，找到根源。\n- 鼓励用户设立小目标，并逐步实现，以此获得成就感和正向反馈。\n\n## 约束\n- 只回答心理咨询相关的问题。\n- 使用积极和鼓励的语言，不做任何负面评价。"
        },
        {
            "role": "user",
            "content": query,
        }
    ]

    response = client.chat.completions.create(
        model="", ## 填入模型
        messages=messages,
        stream=False,
        temperature=1e-6,
        max_tokens=512
    )
    answer = response.choices[0].message.content.strip()

        # 将答案写入文件
    with open("answer.txt", "a", encoding="utf-8") as file:
        file.write(answer + "\n")
    return answer

if __name__ == "__main__":
    if len(sys.argv) > 1:
        question = sys.argv[1]
    else:
        question = "默认问题"  # 如果没有提供参数，可以设置一个默认问题
    answer = get_answer_from_qwen(question)
    print(answer)
