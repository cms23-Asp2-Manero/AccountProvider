using System;
using System.Threading.Tasks;
using AccountProvider.Data.Entities;
using AccountProvider.Helpers;
using AccountProvider.Interfaces;
using AccountProvider.Repositories;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AccountProvider.Functions
{
    public class Create
    {
        private readonly ILogger<Create> _logger;
        private readonly IAccountRepository _accountRepository;


        public Create(ILogger<Create> logger, IAccountRepository accountRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
        }

        [Function(nameof(Create))]
        public async Task Run([ServiceBusTrigger("create_account_request", Connection = "ServiceBus")] ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

            try
            {
                AccountEntity? account = JsonConvert.DeserializeObject<AccountEntity>(message.Body.ToString());

                if (ValidModel.IsValid(account))
                {
                    if (!await _accountRepository.ExistsAsync(x => x.Email == account!.Email))
                    {
                        AccountEntity entity = await _accountRepository.CreateAsync(account!);
                        await messageActions.CompleteMessageAsync(message);
                    }
                    await messageActions.DeadLetterMessageAsync(message, null, "already exists");
                }
                await messageActions.DeadLetterMessageAsync(message, null, "letter failed due to invalid data");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception was raised");
            }
        }
    }
}
