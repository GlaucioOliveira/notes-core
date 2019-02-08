using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using notes.Models;

namespace notes.Hubi
{
    
public class TextHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string user, string message)
    {
        if(TextHolder.content.ContainsKey(user))
        {
           TextHolder.content[user] = message;
        }
        else
        {
            TextHolder.content.Add(user, message);
        }

        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}    
}