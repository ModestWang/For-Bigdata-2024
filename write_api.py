import json
from openai import OpenAI
import os
from tqdm import tqdm

client = OpenAI(
    base_url="http://localhost:9870/v1", ## 填写url
    api_key="none",
)

def get_answer_from_qwen(question):
    query = f"问题：{question}\n"

    messages = [
        {
            "role": "system",
            "content": "### 角色 ###", ## system prompt
        },
        {
            "role": "user",
            "content": query,
        }
    ]

    response = client.chat.completions.create(
        model="/data/shihongchang/train_lf/model", ## 填入模型
        messages=messages,
        stream=False,
        temperature=False,
        max_tokens=1024,
    )
    answer = response.choices[0].message.content.strip()
    return answer

if __name__ == "__main__":
    input_file = "/data/shihongchang/xinlishuju/test/processed_data/zhihu_processed_filtered_0.json" ## 输入json文件
    output_file = "/data/shihongchang/xinlishuju/test/processed_data/zhihu_processed_filtered_1.json" ## 输出json
    
    # 检查并创建输出文件的目录
    output_dir = os.path.dirname(output_file)
    if not os.path.exists(output_dir):
        os.makedirs(output_dir)
    
    with open(input_file, "r", encoding="utf-8") as f:
        data = json.load(f)
    
    for item in tqdm(data, desc="Processing data"):
        question = item.get("question", "")
        answer = get_answer_from_qwen(question)
        item["answer"] = answer
    
    with open(output_file, "w", encoding="utf-8") as f:
        json.dump(data, f, ensure_ascii=False, indent=4)
    
    print(f"Processing completed. Processed data count: {len(data)}")
