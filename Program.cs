/*
 * @FilePath: Program.cs
 * @Author: ModestWang 1598593280@qq.com
 * @Date: 2024-09-04 09:48:00
 * @LastEditors: ModestWang
 * @LastEditTime: 2024-09-05 10:35:17
 * 2024 by ModestWang, All Rights Reserved.
 * @Descripttion:
 */
using BlazorApp.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
