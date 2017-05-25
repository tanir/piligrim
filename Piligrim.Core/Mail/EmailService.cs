using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentEmail.Core;
using FluentEmail.Razor;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Piligrim.Core.Models;
using Piligrim.Web.ViewModels.Emails;
using RazorLight;
using RazorLight.Extensions;

namespace Piligrim.Core.Mail
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<MailConfiguration> configuration;

        public EmailService(IOptions<MailConfiguration> configuration)
        {
            this.configuration = configuration;
        }

        public async Task Send(Order order, string shopEmail, string shopPhoneNumber, string templatePath, string rootPath)
        {
            var conf = this.configuration.Value;

            var model = new NewOrderViewModel
            {
                Id = order.Id,
                PaymentMethod = order.Payment,
                Email = shopEmail,
                DeliveryMethod = order.Delivery,
                CustomerName = order.CustomerName,
                PhoneNumber = shopPhoneNumber,
                Created = order.Timestamp,
                Total = order.OrderItems.Sum(x => x.Count * x.Price),
                CustomerPhoneNumber = order.PhoneNumber,
                CustomerAddress = order.Address,
                Items = order.OrderItems.Select(x => new NewOrderItemViewModel
                {
                    Name = x.Product.Title,
                    Color = x.Color,
                    Size = x.Size,
                    Id = x.Product.Id,
                    Price = x.Price,
                    Count = x.Count
                }).ToList()
            };

            var template = File.ReadAllText(templatePath);

            var letter = EngineFactory.CreatePhysical(rootPath).ParseString(template, model, typeof(NewOrderViewModel));

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(conf.From.Name, conf.From.Address));
            emailMessage.To.Add(new MailboxAddress(order.CustomerName, order.Email));
            emailMessage.Subject = $"Оформлен новый заказ № {order.Id} на сайте Piligrim";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = letter
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(conf.Smtp.Host, conf.Smtp.Port, conf.Smtp.UseSsl);
                await client.AuthenticateAsync(conf.Smtp.Login, conf.Smtp.Password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }

        public async Task Send(string email, string subject, string body)
        {
            var conf = this.configuration.Value;
            var emailMessage = new MimeMessage();


            emailMessage.From.Add(new MailboxAddress(conf.From.Name, conf.From.Address));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(conf.Smtp.Host, conf.Smtp.Port, conf.Smtp.UseSsl);
                await client.AuthenticateAsync(conf.Smtp.Login, conf.Smtp.Password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}