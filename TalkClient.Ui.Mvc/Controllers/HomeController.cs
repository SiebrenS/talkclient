﻿using Microsoft.AspNetCore.Mvc;
using TalkClient.Ui.Mvc.Models;
using TalkClient.Ui.Mvc.Services;

namespace TalkClient.Ui.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ChatChannelService _chatChannelService;
        private readonly ChatMessageService _chatMessageService;

        public HomeController(
            ChatChannelService chatChannelService,
            ChatMessageService chatMessageService)
        {
            _chatChannelService = chatChannelService;
            _chatMessageService = chatMessageService;
        }
        public async Task<IActionResult> Index(string? channel)
        {
            
            var channels = await _chatChannelService.FindAsync();

            if (string.IsNullOrWhiteSpace(channel) && channels.Any())
            {
                channel = channels.First().Name;
            }

            var messages = await _chatMessageService.FindAsync(channel);

            var viewModel = new ChatViewModel
            {
                Channels = channels,
                Messages = messages,
                CurrentChannel = channel
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateChannel(ChatChannelRequest request)
        {
            var channel = await _chatChannelService.CreateAsync(request);

            if (channel is not null)
            {
                return RedirectToAction("Index", new { channel = channel.Name });
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMessage(ChatMessageRequest request)
        {
            await _chatMessageService.CreateAsync(request);
            
            return RedirectToAction("Index", new { channel = request.Channel });
        }


        public IActionResult Privacy()
        {
            return View();
        }
    }
}