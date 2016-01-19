using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using VendingMachine.Domain.Erros;
using VendingMachine.Domain.Models;
using VendingMachine.Domain.Services.Mef;
using VendingMachine.Domain.Services.Common;
using VendingMachine.Domain.Services.Domain;

namespace VendingMachine.Tests
{
    [TestClass]
    public class DomainUnitTest
    {
        public DomainUnitTest()
        {
            Container = CreateContainer();
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get;
            set;
        }

        public IServiceContainer Container
        {
            get;
            private set;
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        private IServiceContainer CreateContainer()
        {
            return new MefServiceContainer(GetType().Assembly, typeof(User).Assembly);
        }

        [TestMethod]
        public void TestVendingMachine()
        {
            /// Создать тестовый счет пользователя
            var user = new User();
            user.CreateDefaults();

            /// Создать торговый автомат для тестирования
            var vm = Container.Resolve<IVMService>();
            Assert.IsNotNull(vm);

            vm.CreateDefaults();

            /// Внести деньги на счет автомата
            var money = user.Account.Get(1, 1, 1, 5, 10);
            Assert.AreEqual(money.Length, 5);

            vm.UserAccount.Add(money);
            Assert.AreEqual(vm.UserAccount.Sum, new Money(18));
            Assert.AreEqual(vm.UserAccount.TotalSum, new Money(18));

            money = user.Account.Get(2, 1, 1, 1);
            Assert.AreEqual(money.Length, 4);

            vm.UserAccount.Add(money);
            Assert.AreEqual(vm.UserAccount.Sum, new Money(23));
            Assert.AreEqual(vm.UserAccount.TotalSum, new Money(23));

            /// Купить чай за 13 рублей
            try
            {
                var tea = vm.Buy(Product.Tea.Name);
                Assert.IsNotNull(tea);
                Assert.AreEqual(vm.UserAccount.TotalSum, new Money(10)); // 23 - 13 = 10

                user.Products.Add(tea);
                Assert.AreEqual(user.Products.Count, 1);
            }
            catch (VMException)
            {
                /// Нет денег на чай :(((
            }

        BuyCoffee:
            /// Купить кофе за 18 рублей
            try
            {
                var coffee = vm.Buy(Product.Coffee.Name);
                Assert.IsNotNull(coffee);
                Assert.AreEqual(vm.UserAccount.TotalSum, new Money(2)); 

                user.Products.Add(coffee);
                Assert.AreEqual(user.Products.Count, 2);
            }
            catch (VMException)
            {
                money = user.Account.Get(Money.Five);
                if (money.Length > 0)
                {
                    vm.UserAccount.Add(money);
                    goto BuyCoffee;
                }
            }

            /// Запросить остаток
            var rest = vm.GetRest();
            Assert.AreEqual(rest.Length, 1);
            Assert.AreEqual(rest[0], Money.Two);

            user.Account.Add(rest);

            Assert.IsTrue(user.Account.Sum > 0);
            Assert.AreEqual(user.Account.Sum, user.Account.TotalSum);
        }
    }
}
