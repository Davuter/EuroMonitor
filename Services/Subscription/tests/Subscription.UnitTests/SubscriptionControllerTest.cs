using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Subscription.Application.Interfaces;
using Subscription.Application.Subscriptions.Command.Add;
using Subscription.Application.Subscriptions.Command.Cancel;
using Subscription.Application.Subscriptions.Query;
using Subsription.API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Subscription.UnitTests
{
    public class SubscriptionControllerTest
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<ISubscriptionDbContext> _dbcontext;
        public SubscriptionControllerTest()
        {
            _mediator = new Mock<IMediator>();
            _dbcontext = new Mock<ISubscriptionDbContext>();
            _dbcontext.Setup(p => p.Subscriptions)
           .Returns(GetQueryableMockDbSet(GetFakeSubscriptions()));
            _dbcontext.Setup(p => p.Statuses)
                .Returns(GetQueryableMockDbSet(GetFakeStatues()));

        }



        [Fact]
        public async void Get_Subscriptions_items_success()
        {
            //Arange



            SubscriptionQueryIn fakecmd = new SubscriptionQueryIn
            {
                BuyerId = "a"
            };

            SubscriptionQueryHandler handler = new SubscriptionQueryHandler(_dbcontext.Object);

            //Act
            SubscriptionQueryOut x = await handler.Handle(fakecmd, new System.Threading.CancellationToken());

            //Asert


            Assert.Equal(1, x.Subscriptions.Count);
        }
     
        [Fact]
        public async void Add_Subscriptions_item_success()
        {
            //Arange
            SubscriptionAddIn fakecmd = new SubscriptionAddIn
            {
                BuyerId = "a",
                ProductId = 5,
                ProductName = "fake5",
                SubscriptionDate = DateTime.Now
            };
            int callCount = 0;
            int addUser = 0;
            int saveChanges = 0;

            _dbcontext.Setup(x => x.Subscriptions.Add(It.IsAny<Subsricption.Domain.Entities.Subscriptions>())).Callback(() => addUser = callCount++);
            _dbcontext.Setup(x => x.SaveChanges()).Callback(() => saveChanges = callCount++);


            SubscriptionAddHandler handler = new SubscriptionAddHandler(_dbcontext.Object);

            //Act
            SubcriptionAddOut x = await handler.Handle(fakecmd, new System.Threading.CancellationToken());
            _dbcontext.Verify(x => x.Subscriptions.Add(It.IsAny<Subsricption.Domain.Entities.Subscriptions>()), Times.Once());
            _dbcontext.Verify(x => x.SaveChanges(), Times.Once());

            //Asert
            Assert.Equal(0, x.Id);
            Assert.Equal(0, addUser);
            Assert.Equal(1, saveChanges);
        }

        [Fact]
        public async void Cancel_Subscription_notfounditem_success()
        {
            //Arange
            SubscriptionCancelIn fakecmd = new SubscriptionCancelIn
            {
                BuyerId = "a",
                ProductId = 5,
                Id = 2
            };

            SubscriptionCancelHandler handler = new SubscriptionCancelHandler(_dbcontext.Object);

            //Act
            SubscriptionCancelOut x = await handler.Handle(fakecmd, new System.Threading.CancellationToken());

            //Asert
            Assert.Equal(1, x.ResultCode);
            Assert.Equal(0, x.Id);
        }

        [Fact]
        public async void Cancel_Subscription_alreadycanceled_success()
        {
            //Arange
            SubscriptionCancelIn fakecmd = new SubscriptionCancelIn
            {
                BuyerId = "c",
                ProductId = 3,
                Id = 3
            };

            SubscriptionCancelHandler handler = new SubscriptionCancelHandler(_dbcontext.Object);

            //Act
            SubscriptionCancelOut x = await handler.Handle(fakecmd, new System.Threading.CancellationToken());

            //Asert
            Assert.Equal(2, x.ResultCode);
            Assert.Equal(0, x.Id);
        }


        [Fact]
        public async void Cancel_Subscription_item_failure()
        {
            //Arange
            SubscriptionCancelIn fakecmd = new SubscriptionCancelIn
            {
                BuyerId = "a",
                ProductId = 1,
                Id = 1
            };

            SubscriptionCancelHandler handler = new SubscriptionCancelHandler(_dbcontext.Object);

            //Act
            SubscriptionCancelOut x = await handler.Handle(fakecmd, new System.Threading.CancellationToken());

            //Asert
            Assert.Equal(99, x.ResultCode);
        }
        [Fact]
        public async void Cancel_Subscription_item_success()
        {
            //Arange
            SubscriptionCancelIn fakecmd = new SubscriptionCancelIn
            {
                BuyerId = "a",
                ProductId = 1,
                Id = 1
            };
            int callCount = 1;
            int updateUser = 0;
            int saveChanges = 0;

            SubscriptionCancelHandler handler = new SubscriptionCancelHandler(_dbcontext.Object);
            _dbcontext.Setup(x => x.Subscriptions.Attach(It.IsAny<Subsricption.Domain.Entities.Subscriptions>())).Callback(() => updateUser = callCount++);
            _dbcontext.Setup(x => x.SaveChanges()).Callback(() => saveChanges = callCount++);

            //Act
            SubscriptionCancelOut x = await handler.Handle(fakecmd, new System.Threading.CancellationToken());

            _dbcontext.Verify(x => x.Subscriptions.Attach(It.IsAny<Subsricption.Domain.Entities.Subscriptions>()), Times.Once());
            _dbcontext.Verify(x => x.SaveChanges(), Times.Once());


            //Asert

            Assert.Equal(1, updateUser);
            Assert.Equal(2, saveChanges);
        }
        [Fact]
        public async void Get_Subscription_Query()
        {

            SubscriptionQueryIn fakecmd = new SubscriptionQueryIn
            {
                BuyerId = "a"
            };


            _mediator.Setup(r => r.Send(It.IsAny<IRequest<SubscriptionQueryOut>>(), default(System.Threading.CancellationToken))).
                ReturnsAsync(new SubscriptionQueryOut() { Subscriptions = GetFakeSubscription() });

            var subscriptionController = new SubscriptionController(_mediator.Object);

            //Act
            var x = await subscriptionController.Query(fakecmd);
            //Asert


            Assert.Equal(3, x.Subscriptions.Count);
        }


        public static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));

            return dbSet.Object;
        }
        private List<Subsricption.Domain.Entities.Subscriptions> GetFakeSubscriptions()
        {
            List<Subsricption.Domain.Entities.Subscriptions> fakeList = new List<Subsricption.Domain.Entities.Subscriptions>
            {
                new Subsricption.Domain.Entities.Subscriptions
                {
                    BuyerId = "a",
                    Id = 1,
                    ProductId=  1,
                    ProductName ="fake1",
                    Status = new Subsricption.Domain.Entities.Status
                    {
                        Id = 1,
                        Name = "Active"
                    },
                    StatusId = 1,
                    SubscriptionDate = DateTime.Now,
                    UnSubscriptionDate = null
                },
                     new Subsricption.Domain.Entities.Subscriptions
                {
                    BuyerId = "b",
                    Id = 2,
                    ProductId=  2,
                    ProductName ="fake2",
                    Status = new Subsricption.Domain.Entities.Status
                    {
                        Id = 1,
                        Name = "Active"
                    },
                    StatusId = 1,
                    SubscriptionDate = DateTime.Now,
                    UnSubscriptionDate = null
                },
                          new Subsricption.Domain.Entities.Subscriptions
                {
                    BuyerId = "c",
                    Id = 3,
                    ProductId=  3,
                    ProductName ="fake3",
                    Status = new Subsricption.Domain.Entities.Status
                    {
                        Id = 2,
                        Name = "Passive"
                    },
                    StatusId = 2,
                    SubscriptionDate = DateTime.Now,
                    UnSubscriptionDate = null
                }
            };
            return fakeList;
        }

        private List<Subsricption.Domain.Entities.Status> GetFakeStatues()
        {
            List<Subsricption.Domain.Entities.Status> statuses = new List<Subsricption.Domain.Entities.Status>
            {
                new Subsricption.Domain.Entities.Status
                {
                    Id = 1,
                    Name = "Active"
                },
                new Subsricption.Domain.Entities.Status
                {
                    Id = 2,
                    Name = "Passive"
                },
            };
            return statuses;
        }

        private List<Subscriptions> GetFakeSubscription()
        {
            List<Subscriptions> subscriptions = new List<Subscriptions>
            {
                 new Subscriptions
                  {
                    Id = 4,
                    ProductId = 4,
                    ProductName = "fake4",
                    SubscritionDate = DateTime.Now
                  },
                 new Subscriptions
                  {
                    Id = 5,
                    ProductId = 5,
                    ProductName = "fake5",
                    SubscritionDate = DateTime.Now 
                  },
                  new Subscriptions
                  {
                    Id = 6,
                    ProductId = 6,
                    ProductName = "fake6",
                    SubscritionDate = DateTime.Now
                  },
            };


            return subscriptions;
        }

    }
}
