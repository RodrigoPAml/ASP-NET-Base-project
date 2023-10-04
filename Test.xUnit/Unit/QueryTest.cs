using Application.Query;
using Domain.Exceptions;
using Domain.Models.Entities;
using Domain.Query;

namespace Tests.xUnit.Unit
{
    public class QueryTest
    {
        private static string Filter = "[{\"field\": \"Name\", \"operation\": \"=\", \"value\": \"rodrigo\" }, {\"field\": \"Login\", \"operation\": \"=\", \"value\": \"admin\"}]";
        private static string Order = "{\"field\": \"Name\", \"ascending\": true }";

        [Fact]
        public void TestUserFilter()
        {
            var filter = UserFilter.Interpret(Filter);

            Assert.True(filter.Count == 2);
        }

        [Fact]
        public void TestFilter()
        {
            Fields<User> fields = new Fields<User>(x => x.Name, x => x.Login);
            Filter<User> filterBy = UserFilter.Compose(Filter, fields);

            Assert.True(filterBy != null);
        }

        [Fact]
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

            Assert.True(isValid == true);
        }

        [Fact]
        public void TestUserOrderBy()
        {
            var order = UserOrderBy.Interpret(Order);

            Assert.True(order != null);
        }

        [Fact]
        public void TestOrderBy()
        {
            Fields<User> fields = new Fields<User>(x => x.Name);
            OrderBy<User> orderBy = UserOrderBy.Compose(Order, fields);

            Assert.True(orderBy != null);
        }

        [Fact]
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

            Assert.True(isValid == true);
        }
    }
}
