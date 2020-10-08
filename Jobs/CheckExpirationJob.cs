using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quartz;
using Stock.Models;

namespace Stock.Jobs
{
    public class CheckExpirationJob : IJob
    {
        private IConfiguration _configuration;
        //public CheckExpirationJob(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}

        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            string connectionString = dataMap.GetString("connectionString");


            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseSqlServer(connectionString);
            using (var db = new ApplicationContext(optionsBuilder.Options))
            {
                var today = DateTime.UtcNow.Date;
                var beforeTwoMonths = today.AddMonths(-2);
                // понижаем цену - скоро истекает срок годности
                var olds = db.Stocks.Where(x => x.State == Stocks.Сonditions.Default && x.Expiration <= beforeTwoMonths);
                foreach (var stock in olds)
                {
                    stock.Cost *= 0.9M;
                    stock.State = Stocks.Сonditions.Less2MonthExpiration;
                }
                db.SaveChanges();
                // списываем, срок годности истёк
                var expired = db.Stocks.Where(x => x.State == Stocks.Сonditions.Less2MonthExpiration && x.Expiration.Date <= today);
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var invoice = new SalesInvoice()
                        {
                            When = DateTime.UtcNow
                        };
                        var salesInfos = new List<SalesInfo>();
                        foreach (var stock in expired)
                        {
                            salesInfos.Add(new SalesInfo
                            {
                                Cost = 0,
                                Count = stock.Count,
                                ProductId = stock.ProductId
                            });
                        }

                        invoice.ProductInfos = salesInfos;
                        db.AddRange(invoice);
                        db.Stocks.RemoveRange(expired);
                    }
                    catch (Exception ex)
                    {
                        // transaction RollBack
                    }
                }
            }
        }
    }
}
