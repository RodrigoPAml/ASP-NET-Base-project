﻿using Application.Query;
using Domain.Exceptions;
using Domain.Models.Entities;
using Domain.Query;

namespace Tests.MS.Unit
{
    [TestClass]
    public class QueryTest
    {
        private static string Filter = "[{\"field\": \"Name\", \"operation\": \"=\", \"value\": \"rodrigo\" }, {\"field\": \"Login\", \"operation\": \"=\", \"value\": \"admin\"}]";
        private static string Order = "{\"field\": \"Name\", \"ascending\": true }";

        [TestMethod]
        public void TestUserFilter()
        {
            var filter = UserFilter.Interpret(Filter);

            Assert.IsTrue(filter.Count == 2);
        }

        [TestMethod]
        public void TestFilter()
        {
            Fields<User> fields = new Fields<User>(x => x.Name, x => x.Login);
            Filter<User> filterBy = UserFilter.Compose(Filter, fields);

            Assert.IsTrue(filterBy != null);
        }

        [TestMethod]
        public void TestNotAllowedFilter()
        {
            Fields<User> fields = new Fields<User>(x => x.Name);
            bool isValid = false;

            try
            {
                Filter<User> filterBy = UserFilter.Compose(Filter, fields);
            }
            catch (BusinessException)
            {
                isValid = true;
            }

            Assert.IsTrue(isValid == true);
        }

        [TestMethod]
        public void TestUserOrderBy()
        {
            var order = UserOrderBy.Interpret(Order);

            Assert.IsTrue(order != null);
        }

        [TestMethod]
        public void TestOrderBy()
        {
            Fields<User> fields = new Fields<User>(x => x.Name);
            OrderBy<User> orderBy = UserOrderBy.Compose(Order, fields);

            Assert.IsTrue(orderBy != null);
        }

        [TestMethod]
        public void TestNotAllowedOrderBy()
        {
            Fields<User> fields = new Fields<User>(x => x.Password);
            bool isValid = false;

            try
            {
                OrderBy<User> orderBy = UserOrderBy.Compose(Order, fields);
            }
            catch (BusinessException)
            {
                isValid = true;
            }

            Assert.IsTrue(isValid == true);
        }
    }
}
