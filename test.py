import json
from openai import OpenAI
import os
from tqdm import tqdm

client = OpenAI(
    base_url="http://localhost:9870/v1",
    api_key="none",
)


def validate_qa(qa_pair):
    question, description, answer = (
        qa_pair["question"],
        qa_pair["description"],
        qa_pair["answer"],
    )
    prompt = f"问题：{question}\n描述：{description}\n答案：{answer}\n你是一名评估员，你的用户在生活上遇到了许多问题，因此他们给出了问题和描述，并获得了一些答案。\n请你判断上述答案是否合理，返回“合理”或“不合理”。"
    messages = [
        {
            "role": "system",
            "content": "你是一个专业的评估员，请仔细分析给出的问题、描述和答案，判断给定答案是否合理。",
        },
        {"role": "user", "content": prompt},
    ]
    response = client.chat.completions.create(
        model="/data/zonepg/models/Qwen/Qwen2-72B-Instruct",
        messages=messages,
        stream=False,
        temperature=False,
        max_tokens=512,
    )
    judgement = response.choices[0].message.content.strip()
    return judgement == "合理"


def validate_qa_few_shot(qa_pair):
    question, description = (
        qa_pair["question"],
        qa_pair["description"],
    )

    few_shot_cases = [
        {
            "问题": "姜昆对中国曲艺界做过哪些贡献？\n",
            "描述": "\n",
            "判断": "不相关",
        },
        {
            "问题": "让你觉得人心真的很冷漠的事是什么？\n",
            "描述": "我就说我上一段的感情吧。和他怎么说也是分分合合一年了，是不同专业的大学同学。和他在一起的时候，我是真的觉得他很贴心，我甚至经常幻想我们的未来。可是后来他会同时和很多女生进行暧昧纠缠，我发现了以后分手，他也立马和其他女生在一起了。真的失望了，以后会更爱自己。\n",
            "判断": "不相关",
        },
        {
            "问题": "恶作剧之吻为何能让人如此难以忘怀？\n",
            "描述": "看起来很非主流，却让人一看就着迷，沉迷于剧中的爱情故事当中\n",
            "判断": "不相关",
        },
        {
            "问题": "委屈到底是什么滋味？\n",
            "描述": "\n",
            "判断": "不相关",
        },
        {
            "问题": "男朋友忙事业，压力大，不能分心恋爱，希望可以过段时间再说，怎么办？\n",
            "描述": "\n",
            "判断": "相关",

        },
        {
            "问题": "家人不同意我远嫁，我该怎么办？\n",
            "描述": "本人女，家庭条件不太好，人丁单薄，如果我嫁了，家里就祖母跟父亲了。我跟L在一起快四年了，他也很爱我，他们家人都还挺认可我的，现在家人不同意的原因大概是异地\n",
            "判断": "相关",
        },
        {
            "问题": "患躁郁症的人多吗？\n",
            "描述": "\n",
            "判断": "相关",
        },
        {
            "问题": "觉得我爸很烦怎么办？\n",
            "描述": "总想和我聊天但话题又特别无聊，明明不懂还要侃历史侃时政，一个问题要重复问无数遍却还是不记得答案\n",
            "判断": "相关",
        },
    ]

    query = f"问题：{question}\n描述：{description}\n"

    messages = [
        {
            "role": "system",
            "content": "角色：你是一个专业的评估员，具有丰富的心理学知识背景。\n背景：你的用户在生活上遇到了许多困难，因此他们给出了问题和描述。但里面混入了一些与心理学无关的问题和描述，对你造成了很大麻烦。\n任务：请仔细分析给出的问题和描述，判断问题和描述是否与心理学、心理咨询学的知识相关，返回“相关”或者“不相关”。\n文本中可能包含无关的特殊字符，请对其进行过滤。",
        },
    ]

    for case in few_shot_cases:
        messages.append(
            {
                "role": "user",
                "content": f"问题：{case['问题']}\n描述：{case['描述']}\n",
            }
        )
        messages.append({"role": "assistant", "content": f"{case['判断']}\n"})
    messages.append({"role": "user", "content": query})
    # print(messages)

    response = client.chat.completions.create(
        model="/data/zonepg/models/Qwen/Qwen2-72B-Instruct",
        messages=messages,
        stream=False,
        temperature=False,
        max_tokens=512,
    )
    
    judgement = response.choices[0].message.content.strip()
    # print(judgement)
    return judgement == "相关"


if __name__ == "__main__":
    validate_qa_few_shot(
        {
            "question": "姜昆对中国曲艺界做过哪些贡献？\n",
            "description": "\n",
        }
    )
    input_file = "/data/shihongchang/xinlishuju/test/processed_data/zhihu_processed_filtered_0.json"
    output_file = "/data/shihongchang/xinlishuju/test/processed_data/zhihu_processed_filtered_1.json"
    with open(input_file, "r", encoding="utf-8") as f:
        data = json.load(f)
    validated_data = [
        item for item in tqdm(data, desc="Validating data") if validate_qa_few_shot(item)
    ]
    with open(output_file, "w", encoding="utf-8") as f:
        json.dump(validated_data, f, ensure_ascii=False, indent=4)
    print(
        f"Validation completed. Original data count: {len(data)}, Validated data count: {len(validated_data)}"
    )
