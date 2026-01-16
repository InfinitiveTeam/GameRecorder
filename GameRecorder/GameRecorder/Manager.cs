using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class Manager
{
    static async Task Main()
    {
        var apiKey = "DeepSeek API Key";

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        var userMessage =  "Hello";

        try
        {
            // 准备请求数据
            var requestData = new
            {
                model = "deepseek-chat",
                messages = new[]
                {
                    new { role = "system", content = "你是一个AI助手，你需要分析用户提供的Among Us游戏对局信息，给出每个玩家的相应的评分" },
                    new { role = "user", content = userMessage }
                },
                max_tokens = 2048,
                temperature = 0.7
            };

            // 序列化为 JSON
            var jsonContent = JsonSerializer.Serialize(requestData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            System.Console.WriteLine("\n正在发送请求...");

            // 发送请求
            var response = await client.PostAsync(
                "https://api.deepseek.com/chat/completions",
                content
            );

            // 确保请求成功
            if (!response.IsSuccessStatusCode)
            {
                System.Console.WriteLine($"错误: {response.StatusCode}");
                var errorContent = await response.Content.ReadAsStringAsync();
                System.Console.WriteLine($"详情: {errorContent}");
                return;
            }

            // 读取响应
            var responseJson = await response.Content.ReadAsStringAsync();

            // 解析响应
            using var doc = JsonDocument.Parse(responseJson);
            var root = doc.RootElement;

            // 获取 content
            if (root.TryGetProperty("choices", out var choices) &&
                choices.GetArrayLength() > 0)
            {
                var choice = choices[0];
                if (choice.TryGetProperty("message", out var message))
                {
                    if (message.TryGetProperty("content", out var contentElement))
                    {
                        System.Console.WriteLine("\nAI 回复:");
                        System.Console.WriteLine(new string('-', 50));
                        System.Console.WriteLine(contentElement.GetString());
                    }
                    else
                    {
                        System.Console.WriteLine("响应中没有 content 字段");
                    }
                }
                else
                {
                    System.Console.WriteLine("响应中没有 message 字段");
                }
            }
            else
            {
                System.Console.WriteLine("响应中没有 choices 或 choices 为空");
            }

            // 显示 token 使用情况
            if (root.TryGetProperty("usage", out var usage))
            {
                System.Console.WriteLine(new string('-', 50));
                System.Console.WriteLine("Token 使用情况:");
                if (usage.TryGetProperty("prompt_tokens", out var promptTokens))
                    System.Console.WriteLine($"  提示词: {promptTokens}");
                if (usage.TryGetProperty("completion_tokens", out var completionTokens))
                    System.Console.WriteLine($"  补全词: {completionTokens}");
                if (usage.TryGetProperty("total_tokens", out var totalTokens))
                    System.Console.WriteLine($"  总计: {totalTokens}");
            }
        }
        catch (HttpRequestException httpEx)
        {
            System.Console.WriteLine($"网络错误: {httpEx.Message}");
        }
        catch (JsonException jsonEx)
        {
            System.Console.WriteLine($"JSON 解析错误: {jsonEx.Message}");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"错误: {ex.Message}");
        }
    }
}