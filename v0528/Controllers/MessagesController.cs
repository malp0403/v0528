using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using v0528.Entities;
using v0528.Extensions;
using v0528.Helpers;
using v0528.Interface;
using v0528.Models;

namespace v0528.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepostiory;
        private readonly IMapper _mapper;

        public MessagesController(IUserRepository userRepository, IMessageRepository messageRepostiory, IMapper mapper)
        {
            _userRepository = userRepository;
            _messageRepostiory = messageRepostiory;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<MessageModel>> CreateMessage(CreateMessageModel createMessageModel)
        {
            var username = User.GetUsername();
            if (username == createMessageModel.RecipientUsername.ToLower())
                return BadRequest("You cannot send messages to yourself");

            var sender = await _userRepository.GetUserByUsernameAsync(username);
            var recipient = await _userRepository.GetUserByUsernameAsync(createMessageModel.RecipientUsername);

            if (recipient == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageModel.Content
            };
            try
            {
                _messageRepostiory.AddMessage(message);
                if (await _messageRepostiory.SaveAllAsync())
                    return Ok(_mapper.Map<MessageModel>(message));
            }
            catch (Exception ex)
            {

            }
            return BadRequest("Failed to send message");
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<MessageModel>>> GetMessagesForuser([FromQuery]MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();

            var messages = await _messageRepostiory.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);

            return messages;
        }

        [HttpGet]
        [Route("[action]/{username}")]
        public async Task<ActionResult<IEnumerable<MessageModel>>> GetMessageThread(string username)
        {
            var currentUsername = User.GetUsername();
            return Ok(await _messageRepostiory.GetMessageThread(currentUsername, username));
        }

        [HttpPost]
        [Route("[action]/{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.GetUsername();

            var message = await _messageRepostiory.GetMessage(id);

            if(message.Sender.UserName != username && message.Recipient.UserName != username)
            {
                return Unauthorized();
            }

            if (message.Sender.UserName == username) message.SenderDeleted = true;
            if (message.Recipient.UserName == username) message.RecipientDeleted = true;

            if(message.SenderDeleted && message.RecipientDeleted)
            {
                _messageRepostiory.DeleteMessage(message);
            }
            if(await _messageRepostiory.SaveAllAsync())
            {
                return Ok();
            }
            return BadRequest("problem deleting the message");

        }

    }
}